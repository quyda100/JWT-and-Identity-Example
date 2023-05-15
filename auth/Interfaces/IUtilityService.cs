namespace auth.Interfaces
{
    public interface IUtilityService
    {
        string UploadImage(IFormFile filem, string prefix);
        List<string> UploadImages(List<IFormFile> files, string prefix);
        Byte[] GetImage(string fileName);
    }
}
