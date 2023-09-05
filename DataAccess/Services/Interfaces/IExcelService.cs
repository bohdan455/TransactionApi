using BLL.Dto;

namespace BLL.Services.Interfaces
{
    public interface IExcelService
    {
        Task<List<TransactionDto>> LoadDataFromExcel(Stream file);
    }
}