// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service.Interfaces;

public interface IServiceConfiguration {
  /// <summary>
  /// Returns that service is available or not.
  /// </summary>
  public bool IsEnabled();
  
  /// <summary>
  /// Get a list of service-specific variables name
  /// </summary>
  /// <returns>Variables name</returns>
  public List<string> GetVariablesName();

  /// <summary>
  /// Re-fetch variables from current environment configuration
  /// </summary>
  /// <returns>Variables name</returns>
  public void RefreshVariables();

  /// <summary>
  /// Get a list of service-specific variables
  /// </summary>
  /// <returns>Variables name</returns>
  public List<SchemaSpecs.Variable> GetVariablesMap();

  /// <summary>
  /// Get service-specific variable name
  /// </summary>
  /// <param name="key">The key with or without service prefix</param>
  /// <returns>Service prefixed variable name</returns>
  public string ToVariableName(string key);

  /// <summary>
  /// Checks that given variable is required or not
  /// </summary>
  /// <param name="key">The key with or without service prefix</param>
  /// <returns>True if required, false otherwise</returns>
  public bool IsVariableRequired(string key);
  
  /// <summary>
  /// Saves the service configuration
  /// </summary>
  /// <param name="configuration">Service configuration</param>
  /// <param name="reload">Upon successful file save, load configuration file into memory</param>
  public void SaveConfig(Dictionary<string, string> configuration, bool reload = false);

  /// <summary>
  /// Reads the service configuration
  /// </summary>
  /// <returns>The configuration object</returns>
  public Dictionary<string, object?> ReadConfig();
}