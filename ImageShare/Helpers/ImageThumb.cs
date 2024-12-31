using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;

namespace ImageShare.Helpers;

public class ImageThumb {
  private string _uid = Guid.NewGuid().ToString();
  public string Title { get; set; } = string.Empty;
  public bool IsProcessed { get; private set; }
  public string Source { get; init; }
  public string HostedUrl { get; private set; } = string.Empty;

  public int Width { get; set; }
  public int Height { get; set; }
  public string Description { get; set; } = string.Empty;

  public ImageThumb(string source) {
    Source = source;
    Initialize();
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
    IsProcessed = true;
  }
}