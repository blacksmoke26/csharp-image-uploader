// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using PixPost.Helpers;

namespace PixPost.Objects.Service.Objects;

public class SchemaSpecs {
  public enum InputFieldType {
    Text = 0,
    Url = 1,
    Toggle = 2,
    Number = 3,
    Integer = 4,
    List = 5,
  }

  public enum VariableValueType {
    String = 0,
    Bool = 1,
    Double = 2,
    Int = 3,
  }

  public class Menu {
    public required string Text { get; set; }
    public required string Icon { get; set; }
  }

  public class ListItem {
    public required string Label { get; set; }
    public required string Value { get; set; }
  }

  public class InputField {
    public required string Label { get; set; }
    public required InputFieldType Type { get; set; }
    public string? Pattern { get; set; }
    public int? ExactLength { get; set; }
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public bool IsRequired { get; set; }
    public IList<ListItem>? Items { get; set; }
  }

  public class Variable {
    public required string Key { get; set; }
    public object? Value { get; set; }
    public required VariableValueType Type { get; set; }
    public required InputField InputField { get; set; }
  }

  public class FileSize {
    public int Minimum { get; set; }
    public int Maximum { get; set; }
  }

  public class FileInfo {
    public required IList<string> AllowedTypes { get; set; }
    public required FileSize Size { get; set; }
  }

  public class VariableInfo {
    public required string Key { get; set; }
    public bool IsRequired { get; set; }
    public bool UseInRequest { get; set; }
  }

  public class KeyConfig {
    public required VariableInfo Image { get; set; }
    public required VariableInfo Api { get; set; }
    public required VariableInfo Caption { get; set; }
    public required VariableInfo Expiration { get; set; }
  }

  public class RequestProps {
    public required string Endpoint { get; set; }
    public required FileInfo FileInfo { get; set; }
    public required KeyConfig KeyConfig { get; set; }
  }
}

public class ImageSchema {
  private string? _logoFile;

  public required string ServiceName { get; set; }
  public required string Title { get; set; }
  public required SchemaSpecs.Menu Menu { get; set; }
  public required string Logo { get; set; }
  public required string DocsUrl { get; set; }
  public required string Description { get; set; }
  public required IList<SchemaSpecs.ListItem> ExpirationRange { get; set; }
  public required string VariablePrefix { get; set; }
  public required IList<SchemaSpecs.Variable> Variables { get; set; }
  public required SchemaSpecs.RequestProps RequestProps { get; set; }

  /// <summary>
  /// Get logo image absolute file path
  /// </summary>
  /// <returns>The image file path</returns>
  public string GetLogoSource() {
    return _logoFile ??= FileHelper.Base64StringToFile(Logo);
  }
}