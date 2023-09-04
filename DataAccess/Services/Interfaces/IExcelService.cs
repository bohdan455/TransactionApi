namespace BLL.Services.Interfaces
{
    public interface IExcelService
    {
        Task LoadFile(Stream file, string userId);
    }
}