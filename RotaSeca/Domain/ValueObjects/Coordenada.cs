
using RotaSeca.Domain.Exceptions;

namespace RotaSeca.Domain.ValueObjects
{
    public sealed record Coordenada
    {
        private const double RaioTerraKm = 6371.0;

        public double Latitude { get; }
        public double Longitude { get; }

        public Coordenada(double latitude, double longitude)
        {
            if (latitude is < -90 or > 90)
                throw new DomainException("A latitude deve estar entre -90 e 90.");
            if (longitude is < -180 or > 180)
                throw new DomainException("A longitude deve estar entre -180 e 180.");

            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>Distância em linha reta, em km</summary>
        public double DistanciaKmAte(Coordenada outra)
        {
            var dLat = ParaRadianos(outra.Latitude - Latitude);
            var dLon = ParaRadianos(outra.Longitude - Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ParaRadianos(Latitude)) * Math.Cos(ParaRadianos(outra.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            return RaioTerraKm * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private static double ParaRadianos(double graus) => graus * Math.PI / 180;
    }
}
