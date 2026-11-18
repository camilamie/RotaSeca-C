using RotaSeca.Domain.Enums;

namespace RotaSeca.DTOs
{
    public record ContagemPorModo(ModoDeslocamentoTipo Modo, int Quantidade);

    public record ResumoHistorico(
        int TotalConsultas,
        int ConsultasComDesvio,
        double PercentualComDesvio,
        int SemCaminhoSeco,
        double? DuracaoMediaMinutos,
        IReadOnlyList<ContagemPorModo> PorModo);

    public record RotaHistoricoResponse(int Id, ModoDeslocamentoTipo Modo, string? DescricaoRecomendada,
                                        double? DistanciaKm, int? DuracaoMinutos, double? RiscoMaximo,
                                        bool SemCaminhoSeco, bool DesviouDeRisco, DateTime CriadoEm);

    public record HistoricoResponse(ResumoHistorico Resumo,
                                    IReadOnlyList<RotaHistoricoResponse> UltimasRotas,
                                    IReadOnlyList<ReporteResponse> UltimosReportes);
}
