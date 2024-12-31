using System.IO;
using SixLabors.ImageSharp;

namespace ImageShare.Helpers;

public abstract class ImageHelper {
  public static readonly string[] ImageExtTypes = ["jpg", "jpeg", "jfif", "png", "gif", "bmp", "webp"];

  public static bool IsImageExt(string fileName) {
    var ext = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
    return ImageExtTypes.Contains(ext);
  }

  public static bool IsImageFile(string fileName) {
    if (!IsImageExt(fileName))
      return false;

    try {
      var imageInfo = Image.Identify(fileName);
      var imgExt = imageInfo.Metadata.DecodedImageFormat?.Name ?? "";
      return ImageExtTypes.Contains(imgExt.ToLowerInvariant());
    }
    catch {
      return false;
    }
  }
}