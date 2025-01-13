using System.IO;
using Newtonsoft.Json;
using PixPost.Helpers;

namespace PixPost.Objects.Service;

public static class ServiceFactory {
  public static string GetServiceSchema() {
    return ResourceHelper.GetTextResource("Resources/Schemas/ImageService.schema.json");
  }

  private static ImageSchema ParseSchema(string schemaJson, out IList<string> errors) {
    return !TryParseSchemaText(schemaJson, out errors)
      ? throw new InvalidDataException("Invalid schema json")
      : JsonConvert.DeserializeObject<ImageSchema>(schemaJson)!;
  }

  public static ImageService CreateFromSchema(ImageSchema schema) => new(schema);

  public static ImageService CreateFromText(string json, out IList<string> errors) {
    return CreateFromSchema(ParseSchema(json, out errors));
  }
  
  public static ImageService CreateFromFile(string jsonFile, out IList<string> errors) {
    return CreateFromText(File.ReadAllText(jsonFile), out errors);
  }

  public static bool TryParseSchemaText(string schemaJson, out IList<string> errors) {
    return SchemaHelper.TryParse(GetServiceSchema(), schemaJson, out errors);
  }

  public static bool TryParseSchemaFile(string schemaJson, out IList<string> errors) {
    return SchemaHelper.TryParseFile(GetServiceSchema(), schemaJson, out errors);
  }
}