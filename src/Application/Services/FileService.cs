using Domain.Services;
using Serilog;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly string _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public async Task<string> SaveBase64Image(string base64Image, string fileName)
        {
            try
            {
                if (!Directory.Exists(_baseDirectory))
                    Directory.CreateDirectory(_baseDirectory);

                var fileExtension = base64Image.Substring(0, base64Image.IndexOf(";base64,")).Split(':')[1].Split('/')[1];
                string base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1);
                string filePath = Path.Combine(_baseDirectory, $"{fileName}.{fileExtension}");
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                await File.WriteAllBytesAsync(filePath, imageBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return null;
            }
        }

        public bool DeleteBase64Image(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_baseDirectory, fileName);
                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }
    }
}
