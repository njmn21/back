using back.Models.DTO;

namespace back.Service.Interface
{
    public interface ITopographicLandmark
    {
        // POST
        Task<ApiResponse<int>> AddTailingDepositWithLandmarks(TopographicLandmarkDto hitoDto);
        Task<ApiResponse<int>> AddMeasurement(TopographicMeasurementsDto measurementsDto);

        // GET
        Task<ApiResponse> GetAllLandmarksWithTailings();
        Task<ApiResponse> GetAllLandmarksWithCoordinates();
        Task<ApiResponse> GetAllMeasurementsWithLandmark();
        Task<ApiResponse> GetMeasurementsByLandmarkId(GetMeasurementsByLandmarkIdDto landmarkIdDto);
        Task<ApiResponse> GetMeasurementsByLandmarkIds(GetMeasurementsByLandmarkIdsDto getMeasurementsByLandmarkIdsDto);
        Task<ApiResponse> GetMeasurementWithMaxTotalLandmarkId(GetMeasurementsByLandmarkIdDto landmarkId);

        // PUT
        Task<ApiResponse> EditMeasurementAndRecalculate(int medicionId, TopographicMeasurementsDto measurementsDto);
    }
}
