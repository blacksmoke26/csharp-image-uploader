using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Media.Imaging;

namespace PixPost.Helpers;

public static class ResourceManager {
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
  /// <param name="directory">(optional) Resources directry e.g., "Resources"</param>
  /// <returns>Whatever exist or not</returns>
  public static bool Exists(string fileName, string? directory = null) {
    return GetList().Contains(ToResourceName(fileName, directory));
  }

  /// <summary>
  /// Normalize the given file name into fully qualified assembly resource name
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <param name="directory">(optional) Resources directry e.g., "Resources"</param>
  /// <returns>Absolute resource name</returns>
  private static string ToResourceName(string fileName, string? directory = null) {
    var resDir = directory ?? ResourceDirectory;
    var assembly = Assembly.GetExecutingAssembly();
    return $"{assembly.GetName().Name}.{resDir}.{fileName}";
  }

  /// <summary>
  /// Get resource stream
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <param name="directory">(optional) Resources directry e.g., "Resources"</param>
  /// <returns>The stream</returns>
  public static Stream ReadResourceStream(string fileName, string? directory = null) {
    if (!Exists(fileName, directory)) {
      throw new MissingManifestResourceException($"Could not resolve {fileName}");
    }

    return Assembly.GetExecutingAssembly().GetManifestResourceStream(ToResourceName(fileName, directory))!;
  }

  /// <summary>
  /// Reads the specified resource as a UTF-8 encoded string.
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <param name="directory">(optional) Resources directry e.g., "Resources"</param>
  /// <see cref="ToResourceName"/>
  /// <returns>The resource content</returns>
  public static string GetTextResource(string fileName, string? directory = null) {
    using var stream = ReadResourceStream(fileName, directory);
    using var streamReader = new StreamReader(stream, Encoding.UTF8);
    return streamReader.ReadToEnd();
  }

  /// <summary>
  /// Reads the specified resource as bitmap image.
  /// </summary>
  /// <param name="fileName">Filename e.g., "some.txt"</param>
  /// <param name="directory">(optional) Resources directry e.g., "Resources"</param>
  /// <see cref="ToResourceName"/>
  /// <returns>The bitmap resource</returns>
  public static BitmapImage GetImageResource(string fileName, string? directory = null) {
    using var stream = ReadResourceStream(fileName, directory);
    using var streamReader = new StreamReader(stream, Encoding.UTF8);
    return new BitmapImage {
      StreamSource = stream
    };
  }

  public static class ResourceList {
    /// <summary>
    /// Get initial env content.
    /// </summary>
    /// <see cref="GetTextResource"/>
    /// <returns>The content</returns>
    public static string GetInitialEnv() => GetTextResource(".env-initial");
    
    /*/// <summary>
    /// Get initial env content.
    /// </summary>
    /// <see cref="GetTextResource"/>
    /// <returns>The bitmap</returns>
    public static BitmapImage SpinnerImage() => GetImageResource("Spinner.png");*/
  }
}