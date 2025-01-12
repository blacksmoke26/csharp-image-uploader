namespace PixPost.Objects.Service.Interfaces;

public interface IServiceFile {
  public List<string> GetFileExtensions();
  public List<string> GetFileMimeTypes();
  public (long Min, long Max) GetFileSize();
  public bool IsValidFile(string fileName);
  public bool IsAllowedSize(string fileName);
}