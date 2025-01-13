using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Media.Imaging;

namespace PixPost.Helpers;

public static class ResourceHelper {
  private static readonly Dictionary<string, string> FileRegistry = [];
  public const string ResourceDirectory = "Resources";

  /// <summary>
  /// Get the FQN names of existing resources.
  /// </summary>
  /// <returns>Resources name list</returns>
  public static string[] GetList() {
    return Assembly.GetExecutingAssembly().GetManifestResourceNames();
  }

  /// <summary>
  /// Verify that the specified resource exists in the specified assembly.
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <returns>Whatever exist or not</returns>
  public static bool Exists(string fileName) {
    return GetList().Contains(ToResourceName(fileName));
  }

  /// <summary>
  /// Normalize the given file name into fully qualified assembly resource name
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <returns>Absolute resource name</returns>
  private static string ToResourceName(string fileName) {
    var assembly = Assembly.GetExecutingAssembly();
    return $"{assembly.GetName().Name}.{fileName}".Replace("/", ".");
  }

  /// <summary>
  /// Get resource stream
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <returns>The stream</returns>
  public static Stream ReadResourceStream(string fileName) {
    if (!Exists(fileName)) {
      throw new MissingManifestResourceException($"Could not resolve {fileName}");
    }

    return Assembly.GetExecutingAssembly().GetManifestResourceStream(ToResourceName(fileName))!;
  }

  /// <summary>
  /// Reads the specified resource as a UTF-8 encoded string.
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <see cref="ToResourceName"/>
  /// <returns>The resource content</returns>
  public static string GetTextResource(string fileName) {
    using var stream = ReadResourceStream(fileName);
    using var streamReader = new StreamReader(stream, Encoding.UTF8);
    return streamReader.ReadToEnd();
  }

  /// <summary>
  /// Reads the specified resource as bitmap image.
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <see cref="ToResourceName"/>
  /// <returns>The bitmap resource</returns>
  public static BitmapImage GetImageResource(string fileName) {
    using var stream = ReadResourceStream(fileName);
    return new BitmapImage { StreamSource = stream };
  }

  /// <summary>
  /// Stores the resource contents into disk file
  /// </summary>
  /// <param name="fileName">Resource file name</param>
  /// <returns>The absolute file path</returns>
  public static string GetFileResource(string fileName) {
    using var stream = ReadResourceStream(fileName);
    return FileHelper.WriteStreamToFile(stream);
  }

  /// <summary>
  /// Stores and cache the resource contents into disk file.<br/>
  /// <b>Note:</b> It will always return the same file if exists.
  /// </summary>
  /// <param name="fileName">Resource file name</param>
  /// <returns>The absolute file path</returns>
  public static string GetCachedFileResource(string fileName) {
    if (FileRegistry.TryGetValue(fileName, out var value)) {
      return value;
    }

    var filePath = GetFileResource(fileName);
    FileRegistry.Add(fileName, filePath);
    return filePath;
  }

  public static class ResourceList {
    /// <summary>
    /// Get initial env content.
    /// </summary>
    /// <see cref="GetTextResource"/>
    /// <returns>The content</returns>
    public static string GetInitialEnv() => GetTextResource($"{ResourceDirectory}/.env-initial");

    /// <summary>
    /// Gets the spinner apng file
    /// </summary>
    /// <see cref="GetTextResource"/>
    /// <returns>The file path</returns>
    public static string GetSpinnerImageFile() => GetCachedFileResource($"{ResourceDirectory}/Spinner.png");
  }
}