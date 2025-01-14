// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Text.RegularExpressions;
using System.Windows;
using PixPost.Objects.Service.Interfaces;
using PixPost.Objects.Service.Objects;
using PixPost.Objects.Service.Dialogs;

namespace PixPost.Objects.Service;

public partial class ImageService(ImageSchema schema) : IBaseImageService {
  private ImageSchema Schema { get; } = schema;

  /// <inheritdoc/>
  public ImageSchema GetSchema() => Schema;

  /// <summary>
  /// Service name
  /// </summary>
  public string ServiceName => GetSchema().ServiceName;

  /// <summary>
  /// Get title / caption
  /// </summary>
  public string Title => GetSchema().Title;
  
  /// <summary>
  /// Service description
  /// </summary>
  public string Description => GetSchema().Description;
  
  /// <summary>
  /// Logo image absolute path
  /// </summary>
  public string LogoPath => GetSchema().GetLogoSource();
  
  /// <summary>
  /// API docs url
  /// </summary>
  public string DocsUrl => GetSchema().DocsUrl;
  
  /// <summary>
  /// Environment variable prefix
  /// </summary>
  /// <example>MY_SERVICE</example>
  public string VariablePrefix => GetSchema().VariablePrefix;
  
  /// <summary>
  /// List of expiration timespans
  /// </summary>
  public Dictionary<string, string> Expirations {
    get {
      return GetSchema().ExpirationRange
        .Select(x => new KeyValuePair<string, string>(x.Value, x.Label))
        .ToDictionary();
    }
  }

  /// <inheritdoc/>
  public (string Caption, string Icon) GetMenu() {
    return (GetSchema().Menu.Text, Regex.Unescape(GetSchema().Menu.Icon));
  }

  public void OpenSettingsDialog() {
    var dialog = new SettingsDialog(this) {
      Owner = Application.Current.MainWindow,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
    };

    dialog.ShowDialog();
  }
}