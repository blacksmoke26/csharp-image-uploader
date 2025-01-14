// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using PixPost.Helpers;
using PixPost.Objects.Service.Interfaces;
using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service;

public partial class ImageService : IServiceConfiguration {
  private List<SchemaSpecs.Variable> _variablesMap = [];
  
  /// <inheritdoc/>
  public string ToVariableName(string key) {
    return key.StartsWith(VariablePrefix + "_")
      ? key
      : string.Concat(VariablePrefix, "_", key);
  }
  
  /// <inheritdoc/>
  public void RefreshVariables() => _variablesMap.Clear();

  /// <inheritdoc/>
  public List<string> GetVariablesName() {
    List<string> list = [];

    list.AddRange(GetVariablesMap()
      .Select(variable => ToVariableName(variable.Key)));

    return list;
  }

  /// <inheritdoc/>
  public List<SchemaSpecs.Variable> GetVariablesMap() {
    if (_variablesMap.Count != 0) return _variablesMap;

    List<SchemaSpecs.Variable> variables = [
      new() {
        Key = "ENABLED",
        Type = "toggle",
        InputProps = new () {
          Label = "Enabled",
          Type = "bool",
          IsRequired = false,
        },
      }
    ];

    variables.AddRange(GetSchema().Variables);

    return _variablesMap = variables.ToList().Select(x => {
      x.Key = ToVariableName(x.Key);
      x.Value = ConfigHelper.ParseValue<object?>(x.Key, x.Type);
      return x;
    }).ToList();
  }
  
  /// <inheritdoc/>
  public void SaveConfig(Dictionary<string, object?> configuration, bool reload = false) {
    var config = GetVariablesMap()
      .Select(x => new KeyValuePair<string, string>(x.Key, ConfigHelper.ConvertToString(x.Type, x.Value)))
      .ToDictionary();

    ConfigHelper.Save(config, null, reload);
  }

  /// <inheritdoc/>
  public Dictionary<string, object?> ReadConfig() {
    Dictionary<string, object?> dict = [];

    foreach (var variable in GetVariablesMap()) {
      dict.Add(variable.Key, variable.Value);
    }

    return dict;
  }

  /// <inheritdoc/>
  public bool IsVariableRequired(string key) {
    return GetVariablesMap()
      .FirstOrDefault(x => x.Key == key)?.InputProps.IsRequired ?? false;
  }

  /// <inheritdoc/>
  public bool IsEnabled() => ConfigHelper.GetBool(ToVariableName("ENABLED"));
}