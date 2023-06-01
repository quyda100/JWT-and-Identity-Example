namespace auth.Interfaces
{
    public interface IUtilityService
    {
        string UploadImage(IFormFile filem, string prefix, string type);
        List<string> UploadImages(List<IFormFile> files, string prefix, string type);
        Byte[] GetImage(string fileName);
        Task<string> CallHttp();
    }
}
