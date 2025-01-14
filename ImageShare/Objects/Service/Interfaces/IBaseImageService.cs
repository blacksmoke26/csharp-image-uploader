// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service.Interfaces;

public interface IBaseImageService {
  protected ImageSchema GetSchema();

  /// <summary>
  /// Icon (Unicode)
  /// </summary>
  /// <example>MY_SERVICE</example>
  public (string Caption, string Icon) GetMenu ();

  /// <summary>
  /// Opens configuration dialog window
  /// </summary>
  public void OpenSettingsDialog();
}
