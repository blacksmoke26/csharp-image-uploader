using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageShare.Helpers;

public class ImageThumb {
  public string Source { get; init; }

  public string Title { get; set; } = string.Empty;
  public bool IsProcessed { get; private set; }
  public string HostedUrl { get; private set; } = string.Empty;
  public string ThumbnailPath { get; private set; } = string.Empty;
  public string Description { get; set; } = string.Empty;

  public int Width { get; set; }
  public int Height { get; set; }

  public ImageThumb(string source) {
    Source = source;
    Initialize();
  }

  public string GenerateThumbnail(int width = 120, int height = 120) {
    var outPath = Path.GetTempFileName();

    using var image = Image.Load(Source);
    image.Mutate(x => x.Resize(width, height));

    image.SaveAsJpeg(outPath);
    return outPath;
  }

  private void Initialize() {
    if (!ImageHelper.IsImageExt(Source)) return;

    ImageInfo imageInfo;
    try {
      imageInfo = Image.Identify(Source);
    }
    catch {
      return;
    }

    var imgExt = imageInfo.Metadata.DecodedImageFormat?.Name ?? "";
    if (!ImageHelper.ImageExtTypes.Contains(imgExt.ToLowerInvariant())) return;

    Width = imageInfo.Width;
    Height = imageInfo.Height;
    Title = Path.GetFileNameWithoutExtension(Source);
    ThumbnailPath = GenerateThumbnail(110, 110);
    IsProcessed = true;
  }
}