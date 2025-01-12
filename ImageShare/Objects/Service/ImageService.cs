using System.IO;
using HeyRed.Mime;
using PixPost.Helpers;
using PixPost.Objects.Service.Interfaces;

namespace PixPost.Objects.Service;

public partial class ImageService : IServiceFile {
  public List<string> GetFileExtensions() {
    return GetSchema()
      .RequestProps.FileInfo.AllowedTypes
      .ToList();
  }

  public List<string> GetFileMimeTypes() {
    return GetFileExtensions().Select(MimeTypesMap.GetMimeType).ToList();
  }

  public (long Min, long Max) GetFileSize() {
    var size = GetSchema().RequestProps.FileInfo.Size;
    return (size.Minimum, size.Maximum);
  }

  public bool IsValidFile(string fileName) {
    return ImageHelper.IsImageMime(fileName, GetFileExtensions().ToArray());
  }

  public bool IsAllowedSize(string fileName) {
    var size = new FileInfo(fileName).Length;
    var (min, max) = GetFileSize();
    return size >= min && size <= max;
  }
}