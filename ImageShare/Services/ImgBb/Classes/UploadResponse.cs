using Newtonsoft.Json;
using PixPost.Core;

namespace PixPost.Services.ImgBb.Classes;

public class UploadResponse : IServiceUploadResponse {
  [JsonProperty("data")] public DataResponse Data { get; set; } = new();
  [JsonProperty("success")] public bool Success { get; set; }
  [JsonProperty("status")] public int Status { get; set; }
}

public sealed class ImageInfoResponse {
  [JsonProperty("filename")] public string Filename { get; set; } = string.Empty;
  [JsonProperty("name")] public string Name { get; set; } = string.Empty;
  [JsonProperty("mime")] public string Mime { get; set; } = string.Empty;
  [JsonProperty("extension")] public string Extension { get; set; } = string.Empty;
  [JsonProperty("url")] public string Url { get; set; } = string.Empty;
}

public sealed class DataResponse {
  [JsonProperty("id")] public string Id { get; set; } = string.Empty;
  [JsonProperty("title")] public string Title { get; set; } = string.Empty;
  [JsonProperty("url_viewer")] public string UrlViewer { get; set; } = string.Empty;
  [JsonProperty("url")] public string Url { get; set; } = string.Empty;
  [JsonProperty("display_url")] public string DisplayUrl { get; set; } = string.Empty;
  [JsonProperty("width")] public string Width { get; set; } = string.Empty;
  [JsonProperty("height")] public string Height { get; set; } = string.Empty;
  [JsonProperty("size")] public string Size { get; set; } = string.Empty;
  [JsonProperty("time")] public string Time { get; set; } = string.Empty;
  [JsonProperty("expiration")] public string Expiration { get; set; } = string.Empty;
  [JsonProperty("image")] public ImageInfoResponse Image { get; set; } = new();
  [JsonProperty("thumb")] public ImageInfoResponse Thumb { get; set; } = new();
  [JsonProperty("medium")] public ImageInfoResponse Medium { get; set; } = new();
  [JsonProperty("delete_url")] public string DeleteUrl { get; set; } = string.Empty;
}