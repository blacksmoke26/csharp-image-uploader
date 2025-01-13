// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace PixPost.Objects.Service.Interfaces;

public interface IBaseImageService {
  protected ImageSchema GetSchema();

  /// <summary>
  /// Service name
  /// </summary>
  public string GetServiceName();

  /// <summary>
  /// Get Service title / caption
  /// </summary>
  public string GetTitle();

  /// <summary>
  /// Service description
  /// </summary>
  public string GetDescription();

  /// <summary>
  /// Logo image path
  /// </summary>
  public string GetLogoPath();

  /// <summary>
  /// API docs url
  /// </summary>
  public string GetDocsUrl();

  /// <summary>
  /// Environment variable prefix
  /// </summary>
  /// <example>MY_SERVICE</example>
  public string GetVariablePrefix();

  /// <summary>
  /// Icon (Unicode)
  /// </summary>
  /// <example>MY_SERVICE</example>
  public (string Caption, string Icon) GetMenu ();

  /// <summary>
  /// List of expiration timespans
  /// </summary>
  public Dictionary<string, string> GetExpirations();

  /// <summary>
  /// Opens configuration dialog window
  /// </summary>
  public void OpenSettingsDialog();
}
