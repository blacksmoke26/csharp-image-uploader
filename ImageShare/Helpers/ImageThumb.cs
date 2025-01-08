using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PixPost.Helpers;

public class ImageThumb {
  public ImageApiResponse? ApiResponse { get; set; }
  public string Source { get; private set; }

  public string Title { get; set; } = string.Empty;
  
  public bool IsLoaded { get; private set; }
  public bool IsProcessing { get; set; }
  public object? LastError { get; set; } 
  
  public string ThumbnailPath { get; private set; } = string.Empty;

  public int OriginalWidth { get; private set; }
  public int OriginalHeight { get; private set; }

  public int Width { get; set; }
  public int Height { get; set; }

  public ImageThumb(string source) {
    Source = source;
    FromFile(source);
  }

  private bool FromFile(string filename) {
    if (!ImageHelper.IsImageMime(filename)) {
      return false;
    }

    var imageInfo = Image.Identify(filename)!;

    OriginalWidth = Width = imageInfo.Width;
    OriginalHeight = Height = imageInfo.Height;
    Title = Path.GetFileNameWithoutExtension(filename);
    ThumbnailPath = GenerateThumbnail(110, 110);
    IsLoaded = true;
    Source = filename;
    return true;
  }

  public async Task<bool> FromUrl(string url) {
    if (!url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)) {
      return false;
    }

    var filePath = await ImageHelper.DownloadAsync(url);
    return filePath != null && FromFile(filePath);
  }

  public string GenerateThumbnail(int width = 120, int height = 120) {
    var outPath = Path.GetTempFileName();

    using var image = Image.Load(Source);
    image.Mutate(x => x.Resize(width, height));

    image.SaveAsJpeg(outPath);
    return outPath;
  }

  public void Resize(int width, int height) {
    if (width == Width && height == Height) return;
    Width = width;
    Height = height;
    Source = GenerateThumbnail(width, height);
  }

  public async Task UploadAsync() {
    ApiResponse = await ImageBb.UploadImageAsync(Source, new ImageBb.UploadImageOptions() {
      Name = Title,
      Expiration = 0
    });
  }
}