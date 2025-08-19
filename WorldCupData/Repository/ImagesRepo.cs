namespace DAL.Repository;

public class ImagesRepo : IImagesRepo
{
    private const string IMAGES_FOLDER_PATH = @"UserData\Images";
    private const string DEFAULT_IMAGE_PATH = @"Images\Avatar.png";

    public async Task<string> UploadImageAsync(byte[] imageData, string fileName)
    {
        try
        {
            if (!Directory.Exists(IMAGES_FOLDER_PATH))
            {
                Directory.CreateDirectory(IMAGES_FOLDER_PATH);
            }

            if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                && !fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                && !fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("File name must end with .png or .jpg");
            }

            var filePath = Path.Combine(IMAGES_FOLDER_PATH, fileName);

            await File.WriteAllBytesAsync(filePath, imageData);

            return filePath;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload image: {ex.Message}", ex);
        }
    }

    public async Task<byte[]> GetImageAsync(string fileName)
    {
        var filePath = Path.Combine(IMAGES_FOLDER_PATH, fileName);
        filePath = File.Exists(filePath + ".png") ? filePath + ".png" : filePath + ".jpg";

        if (File.Exists(filePath))
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        if (File.Exists(DEFAULT_IMAGE_PATH))
        {
            return await File.ReadAllBytesAsync(DEFAULT_IMAGE_PATH);
        }

        //ovo se ne bi trebalo nikad dogoditi
        throw new InvalidOperationException($"No '{fileName}' nor default image found. Directory error. Check the file paths in DAL");
    }

    public async Task RemoveImageAsync(string fileName)
    {
        try
        {
            var filePath = Path.Combine(IMAGES_FOLDER_PATH, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to remove image: {ex.Message}", ex);
        }
    }
}
