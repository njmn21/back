namespace back.Service.Interface
{
    public interface ICoordinateConverter
    {
        Task<(double Latitude, double Longitude)> ConvertUtmToLatLon(double east, double north, int zone, bool isNorthernHemisphere);
    }
}
