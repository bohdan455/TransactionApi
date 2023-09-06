namespace BLL.Services.Interfaces
{
    public interface ICSVService
    {
        Task<Stream> GetCsvFileStream(string userId, List<int> columns, List<string>? types = null, string? transactionStatus = null);
    }
}