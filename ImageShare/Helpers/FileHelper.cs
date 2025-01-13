using System.IO;
using RestSharp;

namespace PixPost.Helpers;

public static class FileHelper {
  /// <summary>
  /// Download content from the given uri and save it in a file
  /// </summary>
  /// <param name="url">The uri</param>
  /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
  /// <returns>The absolute file path to downloaded content</returns>
  public static async Task<string?> DownloadFileAsync(string url, CancellationToken cancellationToken = default) {
    var filename = Path.GetTempFileName();

    using RestClient client = new(url);
    await using var downloadStream = await client.DownloadStreamAsync(new RestRequest("#"), cancellationToken);

    if (downloadStream == null) return null;

    await using var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
    await downloadStream.CopyToAsync(stream, cancellationToken);

    return filename;
  }

  /// <summary>
  /// Decodes the given Base64 string and saves to the file
  /// </summary>
  /// <param name="base64String">The base64 encoded string</param>
  /// <param name="mimeTypes">List of allowed mime types (e.g., ["image/jpeg", ...]</param>
  /// <returns>The absolute file path</returns>
  /// <exception cref="InvalidOperationException">The mime type is not supported</exception>
  /// <exception cref="InvalidOperationException">Failed to decode base64 string</exception>
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
      throw new InvalidOperationException(e.Message);
    }
  }
  
  /// <summary>
  /// Write the given stream into a file
  /// </summary>
  /// <param name="input">The input stream</param>
  /// <returns>The absolute file path</returns>
  public static string WriteStreamToFile(Stream input) {
    var fileName = Path.GetTempFileName();
    var buffer = new byte[16345];
    using var fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

    int read;
    while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
      fs.Write(buffer, 0, read);
    }
  
    return fileName;
  }
}