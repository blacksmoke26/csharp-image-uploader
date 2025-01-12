using PixPost.Core;

namespace PixPost.Services.ImgBb.Classes;

public record ServiceConfiguration: IServiceConfig {
  public bool IsEnabled { get; init; }
  public string Endpoint { get; init; } = "https://api.imgbb.com/1/upload";
  public string ApiKey { get; init; } = string.Empty;
  public long MaxSize { get; set; } = 33554432; // 32 MB
}