namespace RotaSeca.Integrations
{
    public class ServicoRiscoOptions
    {
        public const string Secao = "ServicoRisco";

        public bool UsarSimulado { get; set; } = true;
        public string BaseUrl { get; set; } = "http://localhost:5100/";
        public double ChuvaSimuladaMmH { get; set; } = 22.5;
        public bool AlertasAutomaticosAtivos { get; set; } = true;
        public int IntervaloAlertasMinutos { get; set; } = 15;
    }
}
