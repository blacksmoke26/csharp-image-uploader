using System.Windows;

namespace PixPost.Core;

public abstract class BaseImageService {
  /// <summary>
  /// Service name
  /// </summary>
  public const string ServiceName = "";

  /// <summary>
  /// Service Caption / Label
  /// </summary>
  public virtual string Title { get; protected set; } = string.Empty;

  /// <summary>
  /// Service description
  /// </summary>
  public virtual string Description { get; protected set; } = string.Empty;

  /// <summary>
  /// Logo image resource name
  /// </summary>
  public virtual string LogoSource { get; protected set; } = string.Empty;

  /// <summary>
  /// API docs url
  /// </summary>
  public virtual string DocsUrl { get; protected set; } = string.Empty;

  /// <summary>
  /// Environment variable prefix
  /// </summary>
  /// <example>MY_SERVICE</example>
  public virtual string EnvPrefix { get; init; } = string.Empty;

  /// <summary>
  /// Icon (Unicode)
  /// </summary>
  /// <example>MY_SERVICE</example>
  public virtual string MenuIcon { get; init; } = string.Empty;

  /// <summary>
  /// List of expiration timespans
  /// </summary>
  public virtual Dictionary<string, string> Expirations { get; protected set; } = [];

  /// <summary>
  /// Returns that service is available or not.
  /// </summary>
  public abstract bool IsEnabled();

  /// <summary>
  /// Returns that service is available or not.
  /// </summary>
  public abstract string GetId();

  /// <summary>
  /// Opens configuration dialog window
  /// </summary>
  public abstract void OpenSettingsDialog();
}

public interface IServiceUploadOptions {
}

public interface IServiceUploadResponse {
}

public interface IServiceConfig {
}

public interface ISettingsDialog<out TDialog> where TDialog : Window {
  public void OpenSettingsDialog(Window ownerWindow, Action<TDialog>? callback = null);
}

public interface IServiceSettings<T> where T : IServiceConfig {
  /// <summary>
  /// Saves the service configuration
  /// </summary>
  /// <param name="configuration">Service configuration</param>
  /// <param name="reload">Upon successful file save, load configuration file into memory</param>
  public void SaveConfig(T configuration, bool reload = false);

  /// <summary>
  /// Reads the service configuration
  /// </summary>
  /// <returns>The configuration object</returns>
  public T ReadConfig();
}

public interface IUploadImage<TResponse, in TOptions>
  where TResponse : IServiceUploadResponse
  where TOptions : IServiceUploadOptions {
  public Task<TResponse?> UploadAsync(string filename,
    TOptions? options = default, CancellationToken cancellationToken = default
  );

  public Task<TResponse[]> UploadBulkAsync(string[] files,
    TOptions? options = default, CancellationToken cancellationToken = default
  );
}