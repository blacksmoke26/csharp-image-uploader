using System.IO;
using RestSharp;

namespace PixPost.Helpers;

public class FileHelper {
  public static async Task<string?> DownloadFileAsync(string url, CancellationToken cancellationToken = default) {
    var filename = Path.GetTempFileName();

    using RestClient client = new(url);
    await using var downloadStream = await client.DownloadStreamAsync(new RestRequest("#"), cancellationToken);

    if (downloadStream == null) return null;

    await using var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
    await downloadStream.CopyToAsync(stream, cancellationToken);

    return filename;
  }
}