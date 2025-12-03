using back.Models.DTO;

namespace back.Service.Interface;

public interface IPiezometer
{
    // POST
    Task<ApiResponse<int>> AddPiezometer(PiezometerDto piezometerDto);
    Task<ApiResponse<int>> AddPiezometerMeasurement(PiezometerMeasurementDto piezometerMeasurementDto);

    // GET
    Task<ApiResponse> GetAllPiezometers();
    Task<ApiResponse> GetAllMeasurementsPiezometerById(int piezometerId);
    Task<ApiResponse> GetAllMeasurementsPiezometerByIds(GetMeasurementsPiezometersByIds ids);
    Task<ApiResponse> GetConvertPiezometer();

    // PUT
    Task<ApiResponse> UpdatePiezometer(int id, PutPiezometerDto piezometerDto);
    Task<ApiResponse> UpdateMeasurementPiezometer(int id, PutMeasurementPiezometerDto measurementPiezometerDto);
}