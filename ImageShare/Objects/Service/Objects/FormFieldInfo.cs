// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PixPost.Objects.Service.Objects;

public class FormFieldInfo : INotifyPropertyChanged {
  public required string Id { get; set; }
  public InputFieldType Type { get; set; }
  public string Label { get; set; } = string.Empty;
  public string? Pattern { get; set; }
  public int? ExactLength { get; set; }
  public int? MinLength { get; set; }
  public int? MaxLength { get; set; }
  public bool IsRequired { get; set; }
  public IList<SchemaSpecs.ListItem>? Items { get; set; } = [];
  private object? _value;
  private string? _errorMessage;

  public string? ErrorMessage {
    get => _errorMessage;
    set => SetField(ref _errorMessage, value);
  }

  public object? Value {
    get => _value;
    set => SetField(ref _value, value);
  }

  public void FromSchemaVariable(SchemaSpecs.Variable variable) {
    Value = variable.Value;
    Label = variable.InputField.Label;
    Type = variable.InputField.Type;
    Pattern = variable.InputField.Pattern;
    ExactLength = variable.InputField.ExactLength;
    MinLength = variable.InputField.MinLength;
    MaxLength = variable.InputField.MaxLength;
    IsRequired = variable.InputField.IsRequired;
    Items = variable.InputField.Items;
  }

  public void Validate() {
  }

  public void IsValid() {
  }

  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
    if (EqualityComparer<T>.Default.Equals(field, value)) return false;
    field = value;
    OnPropertyChanged(propertyName);
    return true;
  }
}