namespace MyBudgetManagement.AppService.MD5Service.FileHandle;

public class FileHandler
{
    public async static Task<byte[]> UploadFile(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileToBytes = memoryStream.ToArray();
        return fileToBytes;
    }
    
    public async static Task<string> GetFile(byte[] file)
    {
        return Convert.ToBase64String(file);
    }
}