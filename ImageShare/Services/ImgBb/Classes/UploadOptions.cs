using PixPost.Core;

namespace PixPost.Services.ImgBb.Classes;

public record UploadOptions : IServiceUploadOptions {
  /// <summary>
  /// The name of the file, this is automatically detected if uploading a file
  /// </summary>
  public string Name { get; init; } = string.Empty;

  /// <summary>
  /// Enable this if you want to force uploads to be auto deleted after a certain time (in seconds 60-15552000)
  /// </summary>
  public int? Expiration { get; init; } = null;
}