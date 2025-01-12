using System.Windows;
using Newtonsoft.Json;
using PixPost.Core;
using PixPost.Helpers;
using PixPost.Services.ImgBb.Classes;
using PixPost.Services.ImgBb.Dialogs;
using RestSharp;

namespace PixPost.Services.ImgBb;

public class ImgBb : BaseImageService,
  IUploadImage<UploadResponse, UploadOptions>,
  IServiceSettings<ServiceConfiguration> {
  /// <summary>
  /// Service name
  /// </summary>
  public new const string ServiceName = "ImgBb";

  /// <inheritdoc/>
  public override string MenuIcon { get; init; } = "\uE5D4";

  /// <inheritdoc/>
  public override string Title { get; protected set; } = "ImgBB";

  /// <inheritdoc/>
  public override string LogoSource { get; protected set; } = "PixPost.Services.ImgBb.Assets.logo.png";

  /// <inheritdoc/>
  public override string EnvPrefix { get; init; } = "IMGBB";

  /// <inheritdoc/>
  public override string DocsUrl { get; protected set; } = "https://api.imgbb.com/";

  /// <inheritdoc/>
  public override string Description { get; protected set; } =
    "Free image hosting and sharing service, upload pictures, photo host.";

  public override string GetId() => ServiceName;

  /// <inheritdoc/>
  public override Dictionary<string, string> Expirations { get; protected set; } = new() {
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

  /// <inheritdoc/>
  public override bool IsEnabled() {
    return ConfigHelper.GetBool(ConfigVariables.Enabled);
  }

  /// <inheritdoc/>
  public async Task<UploadResponse?> UploadAsync(string filename, UploadOptions? options = null,
    CancellationToken cancellationToken = default) {
    var config = ReadConfig();

    using RestClient client = new(config.Endpoint);

    RestRequest request = new() {
      AlwaysMultipartFormData = true,
    };

    if (options != null) {
      if (!string.IsNullOrEmpty(options.Name))
        request.AddParameter("name", options.Name);

      if (options.Expiration is > 0)
        request.AddParameter("expiration", (int)options.Expiration);
    }

    request.AddParameter("key", config.ApiKey);
    request.AddFile("image", filename);

    var response = await client.PostAsync(request, cancellationToken);

    if (!response.IsSuccessful) {
      return null;
    }

    return response.Content == null
      ? null
      : JsonConvert.DeserializeObject<UploadResponse>(response.Content);
  }

  /// <inheritdoc/>
  public Task<UploadResponse[]> UploadBulkAsync(string[] files, UploadOptions? options = null,
    CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public void SaveConfig(ServiceConfiguration configuration, bool reload = false) {
    var config = ConfigHelper.ReadFile(ConfigHelper.GetEnvFilePath());
    config[ConfigVariables.Enabled] = ConfigHelper.BoolToString(configuration.IsEnabled);
    config[ConfigVariables.Endpoint] = configuration.Endpoint;
    config[ConfigVariables.Apikey] = configuration.ApiKey;
    ConfigHelper.SaveFile(ConfigHelper.GetEnvFilePath(), config, reload);
  }

  /// <inheritdoc/>
  public ServiceConfiguration ReadConfig() {
    return new ServiceConfiguration {
      IsEnabled = ConfigHelper.GetBool(ConfigVariables.Enabled),
      Endpoint = ConfigHelper.GetString(ConfigVariables.Endpoint),
      ApiKey = ConfigHelper.GetString(ConfigVariables.Apikey),
    };
  }

  public override void OpenSettingsDialog() {
    var dialog = new SettingsDialog {
      Owner = Application.Current.MainWindow,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
    };

    dialog.ShowDialog();
  }
}