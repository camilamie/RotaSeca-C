using RotaSeca.Domain.Entities;
using RotaSeca.DTOs;
using RotaSeca.Services.Models;

namespace RotaSeca.Mappings;

public static class ClassificacaoRisco
{
    public static string Cor(double? probabilidade) => probabilidade switch
    {
        null => "cinza",
        < 0.4 => "verde",
        < 0.7 => "amarelo",
        _ => "vermelho"
    };
}

public static class MappingExtensions
{
    public static UsuarioResponse ToResponse(this Usuario u, int totalLugares) =>
        new(u.Id, u.Nome, u.ModoPreferido, totalLugares);

    public static LugarResponse ToResponse(this LugarSalvo l) =>
        new(l.Id, l.Etiqueta, l.Endereco, l.Latitude, l.Longitude, l.Dias, l.HorarioHabitual,
            l.UltimoRisco, ClassificacaoRisco.Cor(l.UltimoRisco), l.RiscoAtualizadoEm);

    public static ReporteResponse ToResponse(this Reporte r) =>
        new(r.Id, r.Latitude, r.Longitude, r.Nivel, r.Status, r.Confirmacoes, r.Comentario, r.CriadoEm);

    public static AlertaResponse ToResponse(this Alerta a) =>
        new(a.Id, a.LugarSalvoId, a.LugarSalvo?.Etiqueta, a.Probabilidade, a.Nivel, a.Mensagem, a.Lido, a.CriadoEm);

    public static RotaHistoricoResponse ToHistorico(this Rota r) =>
        new(r.Id, r.Modo, r.DescricaoRecomendada, r.DistanciaKm, r.DuracaoMinutos, r.RiscoMaximo,
            r.SemCaminhoSeco, r.DesviouDeRisco, r.CriadoEm);

    public static OpcaoRotaResponse ToResponse(this OpcaoAvaliada o, bool recomendada) =>
        new(o.Descricao, o.DistanciaKm, o.DuracaoMinutos, o.RiscoMaximo,
            (int)Math.Round(o.RiscoMaximo * 100), o.Bloqueada, recomendada,
            o.Trajeto.Select(p => new CoordenadaDto(p.Latitude, p.Longitude)).ToList());
}