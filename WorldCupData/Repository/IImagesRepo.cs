namespace DAL.Repository;

public interface IImagesRepo
{
    Task<string> UploadImageAsync(byte[] imageData, string fileName);
    Task<byte[]> GetImageAsync(string fileName);
    Task RemoveImageAsync(string fileName);
}
