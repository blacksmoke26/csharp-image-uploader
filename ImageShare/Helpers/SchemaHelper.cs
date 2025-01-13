// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace PixPost.Helpers;

public static class SchemaHelper {
  /// <summary>
  /// Parse the JSON schema
  /// </summary>
  /// <param name="schemaJson">The JSON text</param>
  /// <returns>Parsed JSON schema object</returns>
  public static JSchema ParseSchema(string schemaJson) {
    return JSchema.Parse(schemaJson);
  }
  
  /// <summary>
  /// Determines whatever the JSON (text) is valid against the Schema JSON
  /// </summary>
  /// <param name="schema">The schema to test with</param>
  /// <param name="targetJson">The target json text</param>
  /// <param name="errors">When this method returns, contains any error messages generated while validating.</param>
  /// <returns>True when no errors, false otherwise</returns>
  public static bool ValidateText(string schema, string targetJson, out IList<string> errors) {
    return JObject.Parse(targetJson)
      .IsValid(ParseSchema(schema), out errors);
  }

  /// <summary>
  /// Determines whatever the JSON (text) is valid against the Schema JSON
  /// </summary>
  /// <param name="schema">The schema to test with</param>
  /// <param name="targetJson">The target json text</param>
  /// <param name="errors">When this method returns, contains any error messages generated while validating.</param>
  /// <returns>True when no errors, false otherwise</returns>
  public static bool ValidateText(JSchema schema, string targetJson, out IList<string> errors) {
    return JObject.Parse(targetJson).IsValid(schema, out errors);
  }

  /// <summary>
  /// Determines whatever the JSON file is valid against the Schema JSON
  /// </summary>
  /// <param name="schema">The schema to test with</param>
  /// <param name="jsonFile">Absolute json file path</param>
  /// <param name="errors">When this method returns, contains any error messages generated while validating.</param>
  /// <returns>True when no errors, false otherwise</returns>
  public static bool ValidateFile(string schema, string jsonFile, out IList<string> errors) {
    var jsonData = File.ReadAllText(jsonFile);
    return ValidateText(schema, jsonData, out errors);
  }
}