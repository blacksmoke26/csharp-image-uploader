using Newtonsoft.Json;
using RestSharp;

namespace ImageShare.Helpers;

public sealed class ImageBb {
  private const string ApiKey = "d5c3b51bde505c7c0de168937f998f6d";
  private const string UploadEndpoint = "https://api.imgbb.com/1/upload";

  public readonly Dictionary<string, string> Expirations = new() {
    { "", "Don't autodelete" },
    { "PT5M", "After 5 minutes" },
    { "PT15M", "After 15 minutes" },
    { "PT30M", "After 30 minutes" },
    { "PT1H", "After 1 hour" },
    { "PT3H", "After 3 hours" },
    { "PT6H", "After 6 hours" },
    { "PT12H", "After 12 hours" },
    { "P1D", "After 1 day" },
    { "P2D", "After 2 days" },
    { "P3D", "After 3 days" },
    { "P4D", "After 4 days" },
    { "P5D", "After 5 days" },
    { "P6D", "After 6 days" },
    { "P1W", "After 1 week" },
    { "P2W", "After 2 weeks" },
    { "P3W", "After 3 weeks" },
    { "P1M", "After 1 month" },
    { "P2M", "After 2 months" },
    { "P3M", "After 3 months" },
    { "P4M", "After 4 months" },
    { "P5M", "After 5 months" },
    { "P6M", "After 6 months" },
  };

  public sealed class UploadImageOptions {
    /// <summary>
    /// The name of the file, this is automatically detected if uploading a file
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Enable this if you want to force uploads to be auto deleted after a certain time (in seconds 60-15552000)
    /// </summary>
    public int Expiration { get; init; }
  }

  public static async Task<ImageApiResponse?> UploadImageAsync(string filename,
    UploadImageOptions? uploadOptions = null, CancellationToken cancellationToken = default) {
    using RestClient client = new(UploadEndpoint);

    RestRequest request = new() {
      AlwaysMultipartFormData = true,
    };

    if (uploadOptions is not null) {
      if (!string.IsNullOrEmpty(uploadOptions.Name))
        request.AddParameter("name", uploadOptions.Name);

      if (uploadOptions.Expiration > 0)
        request.AddParameter("expiration", uploadOptions.Expiration);
    }

    request.AddParameter("key", ApiKey);
    request.AddFile("image", filename);

    var response = await client.PostAsync(request, cancellationToken);

    if (!response.IsSuccessful) {
      return null;
    }

    return response.Content == null
      ? null
      : JsonConvert.DeserializeObject<ImageApiResponse>(response.Content);
  }
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

public sealed class ImageApiResponse {
  [JsonProperty("data")] public DataResponse Data { get; set; } = new();
  [JsonProperty("success")] public bool Success { get; set; }
  [JsonProperty("status")] public int Status { get; set; }
}