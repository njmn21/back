using back.Service.Interface;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace back.Service.Implementation
{
    public class CoordinateConverterImpl : ICoordinateConverter
    {
        public async Task<(double Latitude, double Longitude)> ConvertUtmToLatLon(double east, double north, int zone, bool isNorthernHemisphere)
        {
            return await Task.Run(() =>
            {
                var utm = ProjectedCoordinateSystem.WGS84_UTM(zone, isNorthernHemisphere);
                var geographic = GeographicCoordinateSystem.WGS84;

                var transformFactory = new CoordinateTransformationFactory();
                var transformation = transformFactory.CreateFromCoordinateSystems(utm, geographic);

                double[] latLon = transformation.MathTransform.Transform(new double[] { east, north });

                return (latLon[1], latLon[0]);
            });
        }
    }
}
