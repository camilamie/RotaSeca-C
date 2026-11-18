using Microsoft.EntityFrameworkCore;
using RotaSeca.BackgroundJobs;
using RotaSeca.Data;
using RotaSeca.Domain.Modos;
using RotaSeca.Integrations;
using RotaSeca.Integrations.Http;
using RotaSeca.Integrations.Simulados;
using RotaSeca.Middlewares;
using RotaSeca.Repositories;
using RotaSeca.Services;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ---------- Web API ----------
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// A documentação OpenAPI lê estas opções: enums aparecem como texto no Scalar
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi();

// ---------- Banco de dados (EF Core + SQLite) ----------
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------- Repositórios (Scoped: um por requisição, junto com o DbContext) ----------
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILugarRepository, LugarRepository>();
builder.Services.AddScoped<IRotaRepository, RotaRepository>();
builder.Services.AddScoped<IReporteRepository, ReporteRepository>();
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();

// ---------- Serviços de aplicação ----------
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILugarService, LugarService>();
builder.Services.AddScoped<IRotaService, RotaService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<IHistoricoService, HistoricoService>();

// ---------- Estratégias de modo de deslocamento (polimorfismo + DI) ----------
builder.Services.AddSingleton<IModoDeslocamento, APe>();
builder.Services.AddSingleton<IModoDeslocamento, MotoBike>();
builder.Services.AddSingleton<IModoDeslocamento, CarroOnibus>();
builder.Services.AddSingleton<IModoDeslocamentoFactory, ModoDeslocamentoFactory>();

// ---------- Provedores externos: simulado ou real, conforme o appsettings ----------
var secaoRisco = builder.Configuration.GetSection(ServicoRiscoOptions.Secao);
builder.Services.Configure<ServicoRiscoOptions>(secaoRisco);
var opcoesRisco = secaoRisco.Get<ServicoRiscoOptions>() ?? new ServicoRiscoOptions();

if (opcoesRisco.UsarSimulado)
{
    builder.Services.AddSingleton<IRiscoProvider, RiscoProviderSimulado>();
}
else
{
    builder.Services.AddHttpClient<IRiscoProvider, RiscoProviderHttp>(client =>
    {
        client.BaseAddress = new Uri(opcoesRisco.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(10);
    });
}

builder.Services.AddSingleton<IChuvaProvider, ChuvaProviderSimulado>();
builder.Services.AddSingleton<IRoteadorProvider, RoteadorProviderSimulado>();

// ---------- Job de alertas a cada 15 minutos ----------
if (opcoesRisco.AlertasAutomaticosAtivos)
    builder.Services.AddHostedService<AlertaBackgroundService>();

// ---------- Tratamento global de erros ----------
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Aplica as migrations ao iniciar: o professor só precisa apertar F5
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();

app.MapOpenApi();                                                // /openapi/v1.json
app.MapScalarApiReference(o => o.WithTitle("Rota Seca - API do app")); // /scalar

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();