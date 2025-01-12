// ReSharper disable All

namespace PixPost.Objects.Service;

public partial class SchemaSpecs {
  public class Menu {
    public required string Text { get; set; }
    public required string Icon { get; set; }
  }

  public class Size {
    public long Minimum { get; set; }
    public long Maximum { get; set; }
  }

  public class FileInfo {
    public required IList<string> AllowedTypes { get; set; }
    public required Size Size { get; set; }
  }

  public class ExpirationRange {
    public required string Label { get; set; }
    public required string Value { get; set; }
  }

  public class Variable {
    public string Key { get; set; } = string.Empty;
    public object? Value { get; set; } = string.Empty;
    public required string Label { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public bool IsRequired { get; set; }
  }

  public class Image {
    public required string Key { get; set; }
    public bool IsRequired { get; set; }
    public bool UseInRequest { get; set; }
  }

  public class Api {
    public required string Key { get; set; }
    public bool IsRequired { get; set; }
    public bool UseInRequest { get; set; }
  }

  public class Caption {
    public required string Key { get; set; }
    public bool IsRequired { get; set; }
    public bool UseInRequest { get; set; }
  }

  public class Expiration {
    public required string Key { get; set; }
    public bool IsRequired { get; set; }
    public bool UseInRequest { get; set; }
  }

  public class KeyConfig {
    public required Image Image { get; set; }
    public required Api Api { get; set; }
    public required Caption Caption { get; set; }
    public required Expiration Expiration { get; set; }
  }

  public class RequestProps {
    public required string Endpoint { get; set; }
    public required FileInfo FileInfo { get; set; }
    public required KeyConfig KeyConfig { get; set; }
  }
}

public class ImageSchema {
  public required string ServiceName { get; set; }
  public required string Title { get; set; }
  public required SchemaSpecs.Menu Menu { get; set; }
  public required string Logo { get; set; }
  public required string DocsUrl { get; set; }
  public required string Description { get; set; }
  public required IList<SchemaSpecs.ExpirationRange> ExpirationRange { get; set; }
  public required string VariablePrefix { get; set; }
  public required IList<SchemaSpecs.Variable> Variables { get; set; }
  public required SchemaSpecs.RequestProps RequestProps { get; set; }
}