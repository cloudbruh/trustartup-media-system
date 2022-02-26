namespace CloudBruh.Trustartup.MediaSystem.Services;

public class MediaFileService
{
    private readonly string _storagePath;
    private readonly string[] _allowedExtensions = {".png", ".jpg"};
    
    public MediaFileService(IConfiguration configuration)
    {
        _storagePath = configuration.GetValue<string>("Storage:Path");
    }

    public async Task<string> Save(IFormFile file)
    {
        if (file.Length > 1024*1024)
        {
            throw new ArgumentException("File is too large.");
        }

        string filename = GenerateRandomFileName(file.FileName);
        string path = Path.Combine(_storagePath, filename);

        await using FileStream stream = File.Create(path);
        await file.CopyToAsync(stream);

        return filename;
    }

    public Task<Stream> Download(string filename)
    {
        string path = Path.Combine(_storagePath, filename);
        return Task.FromResult<Stream>(File.OpenRead(path));
    }

    private string GenerateRandomFileName(string originalFilename)
    {
        string extension = Path.GetExtension(originalFilename).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            throw new ArgumentException("Invalid file extension.");
        }
        
        string filename = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);
        
        return filename;
    }
}