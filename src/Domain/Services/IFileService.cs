namespace Domain.Services
{
    public interface IFileService
    {
        Task<string> SaveBase64Image(string base64Image, string fileName);
        bool DeleteBase64Image(string fileName);
    }
}
