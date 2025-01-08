using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;

namespace PixPost.Helpers;

public sealed class ImageBb {
  private const string ApiKey = "d5c3b51bde505c7c0de168937f998f6d";
  private const string UploadEndpoint = "https://api.imgbb.com/1/upload";
  public const long MaxSize = 33554432; // 32 MB

  public static readonly Dictionary<string, string> Expirations = new() {
    { "None", "Don't auto delete" },
    { "TM5", "After 5 minutes" },
    { "TM15", "After 15 minutes" },
    { "TM30", "After 30 minutes" },
    { "TH1", "After 1 hour" },
    { "TH3", "After 3 hours" },
    { "TH6", "After 6 hours" },
    { "TH12", "After 12 hours" },
    { "ED1", "After 1 day" },
    { "ED2", "After 2 days" },
    { "ED3", "After 3 days" },
    { "ED4", "After 4 days" },
    { "ED5", "After 5 days" },
    { "ED6", "After 6 days" },
    { "EW1", "After 1 week" },
    { "EW2", "After 2 weeks" },
    { "EW3", "After 3 weeks" },
    { "EM1", "After 1 month" },
    { "EM2", "After 2 months" },
    { "EM3", "After 3 months" },
    { "EM4", "After 4 months" },
    { "EM5", "After 5 months" },
    { "EM6", "After 6 months" },
  };

  public static int? ExpiryToSeconds(string expiry) {
    if (expiry == "None") return null;

    var value = int.Parse(Regex.Replace(expiry, @"[^\d]+", string.Empty));
    var unit = Regex.Replace(expiry, "[0-9]+", string.Empty);

    return unit switch {
      "TM" => (int)TimeSpan.FromMinutes(value).TotalSeconds,
      "TH" => (int)TimeSpan.FromHours(value).TotalSeconds,
      "ED" => (int)TimeSpan.FromDays(value).TotalSeconds,
      "EW" => (int)TimeSpan.FromDays(value * 7).TotalSeconds,
      "EM" => (int)TimeSpan.FromDays(value * 30).TotalSeconds,
      _ => null
    };
  }

  /// <summary>
  /// Convert bytes into megabytes
  /// </summary>
  /// <param name="bytesSize">Size in bytes</param>
  /// <returns>Final value</returns>
  public static double SizeToMb(double bytesSize) {
    return Math.Round(bytesSize / 1024 / 1024);
  }

  public sealed class UploadImageOptions {
    /// <summary>
    /// The name of the file, this is automatically detected if uploading a file
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Enable this if you want to force uploads to be auto deleted after a certain time (in seconds 60-15552000)
    /// </summary>
    public int? Expiration { get; init; }
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

      if (uploadOptions.Expiration is > 0)
        request.AddParameter("expiration", (int)uploadOptions.Expiration);
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