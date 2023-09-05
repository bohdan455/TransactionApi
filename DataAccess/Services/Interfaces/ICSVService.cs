namespace BLL.Services.Interfaces
{
    public interface ICSVService
    {
        Task<Stream> GetCsvFileStream(string userId);
    }
}