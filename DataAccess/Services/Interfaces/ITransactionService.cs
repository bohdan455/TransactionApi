namespace BLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task ChangeStatus(int transactionId, string userId, string newStatus);
        Task LoadDataFromExcel(Stream file, string userId);
    }
}