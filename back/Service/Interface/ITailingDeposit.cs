using back.Models.DTO;

namespace back.Service.Interface;

public interface ITailingDeposit
{
    //POST
    Task<ApiResponse<int>> AddTailingDeposit(TailingDepositDto depositDto);
    
    //GET
    Task<ApiResponse> GetAllTailingDeposits();

    //PUT
    Task<ApiResponse> EditTailingDeposit(int id, TailingDepositDto depositDto);
}