using System.IO;
using RestSharp;

namespace PixPost.Helpers;

public static class FileHelper {
  public static async Task<string?> DownloadFileAsync(string url, CancellationToken cancellationToken = default) {
    var filename = Path.GetTempFileName();

    using RestClient client = new(url);
    await using var downloadStream = await client.DownloadStreamAsync(new RestRequest("#"), cancellationToken);

    if (downloadStream == null) return null;

    await using var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
    await downloadStream.CopyToAsync(stream, cancellationToken);

    return filename;
  }

  public static string Base64StringToFile(string base64String, List<string>? mimeTypes = null) {
    var parts = base64String.Split(";base64,");
    var mimeType = parts.First().Replace("data:", "");

    if (mimeTypes != null && !mimeTypes.Contains(mimeType)) {
      throw new InvalidOperationException($"The mime type '{mimeType}' is not supported");
    }

    try {
      var filePath = Path.GetTempFileName();
      File.WriteAllBytes(filePath, Convert.FromBase64String(parts.Last()));
      return filePath;
    }
    catch (Exception e) {
      Console.WriteLine($"Error while converting base64 string: {e.Message}");
      throw;
    }
  }
}