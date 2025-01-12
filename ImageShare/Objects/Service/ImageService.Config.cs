using PixPost.Helpers;
using PixPost.Objects.Service.Interfaces;

namespace PixPost.Objects.Service;

public partial class ImageService : IServiceConfiguration {
  /// <inheritdoc/>
  public string ToVariableName(string key) {
    return key.StartsWith(GetVariablePrefix() + "_")
      ? key
      : string.Concat(GetVariablePrefix(), "_", key);
  }
  
  /// <inheritdoc/>
  public void RefreshVariables() => _variablesMap.Clear();

  /// <inheritdoc/>
  public List<string> GetVariablesName() {
    List<string> list = [];

    list.AddRange(GetVariablesMap()
      .Select(variable => ToVariableName(variable.Name)));

    return list;
  }

  /// <inheritdoc/>
  public List<SchemaSpecs.Variable> GetVariablesMap() {
    if (_variablesMap.Count != 0) return _variablesMap;

    List<SchemaSpecs.Variable> variables = [
      new() {
        Label = "Enabled",
        Name = "ENABLED",
        Type = "bool",
        IsRequired = false,
      }
    ];

    variables.AddRange(GetSchema().Variables);

    return _variablesMap = variables.ToList().Select(x => {
      x.Key = ToVariableName(x.Name);
      x.Value = ConfigHelper.ParseValue<object?>(x.Name, x.Type);
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
      .FirstOrDefault(x => x.Name == key)?.IsRequired ?? false;
  }

  /// <inheritdoc/>
  public bool IsEnabled() => ConfigHelper.GetBool(ToVariableName("ENABLED"));
}