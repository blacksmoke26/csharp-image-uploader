using PixPost.Helpers;
using PixPost.Objects.Service.Interfaces;

namespace PixPost.Objects.Service;

public partial class ImageService(ImageSchema schema) : IBaseImageService {
  private ImageSchema Schema { get; } = schema;

  private string? _logoPath;

  private List<SchemaSpecs.Variable> _variablesMap = [];

  /// <inheritdoc/>
  public ImageSchema GetSchema() => Schema;

  /// <inheritdoc/>
  public string GetServiceName() => GetSchema().ServiceName;

  /// <inheritdoc/>
  public string GetTitle() => GetSchema().Title;

  /// <inheritdoc/>
  public string GetDescription() => GetSchema().Description;

  /// <inheritdoc/>
  public string GetLogoPath() {
    return _logoPath ??= FileHelper.Base64StringToFile(Schema.Logo);
  }

  /// <inheritdoc/>
  public string GetDocsUrl() => GetSchema().DocsUrl;

  /// <inheritdoc/>
  public string GetVariablePrefix() => GetSchema().VariablePrefix;

  /// <inheritdoc/>
  public SchemaSpecs.Menu GetMenu() => GetSchema().Menu;

  /// <inheritdoc/>
  public Dictionary<string, string> GetExpirations() {
    return GetSchema().ExpirationRange
      .Select(x => new KeyValuePair<string, string>(x.Value, x.Label))
      .ToDictionary();
  }

  public void OpenSettingsDialog() {
    Console.WriteLine("Opened");
  }
}