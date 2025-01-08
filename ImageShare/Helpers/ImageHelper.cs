using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using HeyRed.Mime;

namespace PixPost.Helpers;

public abstract class ImageHelper {
  public static readonly string[] ImageExtTypes = ["jpg", "jpeg", "jfif", "png", "gif", "bmp", "webp"];

  public static bool IsImageExt(string fileName) {
    var ext = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
    return ImageExtTypes.Contains(ext);
  }

  public static ImageMetadata? ReadImageMetaData(string fileName) {
    return !File.Exists(fileName) ? null : Image.Identify(fileName).Metadata;
  }

  public static string GetMimeTypeExtension(string mimeType) {
    return MimeTypesMap.GetExtension(mimeType);
  }

  public static string GetExtensionMimeType(string ext) {
    return MimeTypesMap.GetMimeType(ext);
  }

  public static bool IsImageMime(string fileName, string[]? extensions = null) {
    try {
      var metadata = ReadImageMetaData(fileName);
      if (metadata == null) return false;

      var imgExt = metadata.DecodedImageFormat?.Name ?? "";

      return extensions?.Contains(imgExt.ToLowerInvariant())
             ?? ImageExtTypes.Contains(imgExt.ToLowerInvariant());
    }
    catch {
      return false;
    }
  }

  public static bool IsImageFile(string fileName) {
    return IsImageExt(fileName) && IsImageMime(fileName);
  }

  public static async Task<string?> DownloadAsync(string url, CancellationToken cancellationToken = default) {
    var filename = await FileHelper.DownloadFileAsync(url, cancellationToken);
    if (filename == null) return null;

    var mimeType = ReadImageMetaData(filename)?.DecodedImageFormat?.DefaultMimeType;
    if (mimeType == null
        || !mimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase)
        || !ImageExtTypes.Contains(GetMimeTypeExtension(mimeType).ToLowerInvariant())) {
      File.Delete(filename);
      return null;
    }

    return filename;
  }
}