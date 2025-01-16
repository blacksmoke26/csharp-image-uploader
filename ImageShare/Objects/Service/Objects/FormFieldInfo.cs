// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PixPost.Objects.Service.Objects;

public class FormFieldInfo : INotifyPropertyChanged {
  public required string Id { get; init; }
  public InputFieldType Type { get; init; }
  public string Label { get; private set; } = string.Empty;
  public int? ExactLength { get; private set; }
  public int? MinLength { get; private set; }
  public int? MaxLength { get; private set; }
  public string? Pattern { get; private set; }
  public int? Minimum { get; private set; }
  public int? Maximum { get; private set; }
  public bool IsRequired { get; private set; }
  public IList<SchemaSpecs.ListItem>? Items { get; set; } = [];

  private object? _value;
  private string? _errorMessage;

  public string? ErrorMessage {
    get => _errorMessage;
    set => SetField(ref _errorMessage, value);
  }

  public object? Value {
    get {
      if (Type != InputFieldType.List) return _value;
      return _value ?? Items?.FirstOrDefault();
    }
    set => SetField(ref _value, value);
  }

  public void FromSchemaVariable(SchemaSpecs.Variable variable) {
    Label = variable.InputField.Label;
    ExactLength = variable.InputField.ExactLength;
    MinLength = variable.InputField.MinLength;
    MaxLength = variable.InputField.MaxLength;
    IsRequired = variable.InputField.IsRequired;
    Minimum = variable.InputField.Minimum;
    Maximum = variable.InputField.Maximum;
    Pattern = variable.InputField.Pattern;
    Items = variable.InputField.Items;

    // Dynamic value
    Value = variable.Value;
  }

  private bool ValidateList() {
    if (IsRequired && (Value == null || (Value as SchemaSpecs.ListItem)?.Value == "None")) {
      ErrorMessage = $"Please provide a valid {Label.ToLower()}.";
      return false;
    }

    return true;
  }

  private bool ValidateToggle() {
    if (IsRequired && (Value == null || !(bool)Value)) {
      ErrorMessage = $"You must check before continue.";
      return false;
    }

    return true;
  }

  private bool ValidateText() {
    var currentValue = Value == null ? string.Empty : (string)Value;

    if (IsRequired && string.IsNullOrWhiteSpace(currentValue)) {
      ErrorMessage = $"Please provide a valid {Label.ToLower()}.";
      return false;
    }

    if (ExactLength != null && currentValue.Length != ExactLength) {
      ErrorMessage = $"{Label} length must be {ExactLength} chars.";
      return false;
    }

    if (MinLength != null && currentValue.Length < MinLength) {
      ErrorMessage = $"The minimum chars should be {MinLength} chars.";
      return false;
    }

    if (MaxLength != null && currentValue.Length > MaxLength) {
      ErrorMessage = $"The maximum chars should be {MinLength} chars.";
      return false;
    }

    if (Pattern != null) {
      try {
        if (!Regex.IsMatch(currentValue, Pattern)) {
          ErrorMessage = "The value you have entered is not a valid.";
          return false;
        }
      }
      catch {
        ErrorMessage = "The value you have entered is not a valid.";
        return false;
      }
    }

    return true;
  }

  private bool ValidateUrl() {
    var currentValue = Value == null ? string.Empty : (string)Value;

    if (IsRequired && string.IsNullOrWhiteSpace(currentValue)) {
      ErrorMessage = $"Please provide a valid {Label.ToLower()}.";
      return false;
    }

    if (!Uri.TryCreate(currentValue, UriKind.Absolute, out _)) {
      ErrorMessage = "Invalid url given.";
      return false;
    }

    return true;
  }

  private bool ValidateInteger() {
    var currentValue = Value == null ? string.Empty : (string)Value;

    if (IsRequired && string.IsNullOrWhiteSpace(currentValue)) {
      ErrorMessage = $"Please provide a valid {Label.ToLower()}.";
      return false;
    }

    if (!int.TryParse(currentValue, out var number)) {
      ErrorMessage = "The value must be an integer number.";
      return false;
    }

    if (Minimum != null && number < Minimum) {
      ErrorMessage = $"The value should be less than {MinLength}.";
      return false;
    }

    if (Maximum != null && number > Maximum) {
      ErrorMessage = $"The value should be greater than {Maximum}.";
      return false;
    }

    return true;
  }

  private bool ValidateDouble() {
    var currentValue = Value == null ? string.Empty : (string)Value;

    if (IsRequired && string.IsNullOrWhiteSpace(currentValue)) {
      ErrorMessage = $"Please provide a valid {Label.ToLower()}.";
      return false;
    }

    if (!double.TryParse(currentValue, out var number)) {
      ErrorMessage = "The value must be a decimal number.";
      return false;
    }

    if (Minimum != null && number < Minimum) {
      ErrorMessage = $"The value should be less than {MinLength}.";
      return false;
    }

    if (Maximum != null && number > Maximum) {
      ErrorMessage = $"The value should be greater than {Maximum}.";
      return false;
    }

    return true;
  }

  public bool Validate() {
    ErrorMessage = null;
    return Type switch {
      InputFieldType.Text => ValidateText(),
      InputFieldType.Url => ValidateUrl(),
      InputFieldType.Toggle => ValidateToggle(),
      InputFieldType.Double => ValidateDouble(),
      InputFieldType.Integer => ValidateInteger(),
      InputFieldType.List => ValidateList(),
      _ => throw new ArgumentOutOfRangeException()
    };
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