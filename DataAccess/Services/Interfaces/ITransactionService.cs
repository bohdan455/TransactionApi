using DataAccess.Entities;

namespace BLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task ChangeStatus(int transactionId, string userId, string newStatus);
        Task<List<Transaction>> GetTransactions(string userId, List<string>? types = null, string? transactionStatus = null);
        Task LoadDataFromExcel(Stream file, string userId);
    }
}