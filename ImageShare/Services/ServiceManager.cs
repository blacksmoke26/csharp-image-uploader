// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Collections.ObjectModel;
using System.IO;
using PixPost.Helpers;
using PixPost.Objects.Service;

namespace PixPost.Services;

public static class ServiceManager {
  private const string Protocol = "services://";
  private const string ConfigKey = "CURRENT_SERVICE";
  private const string ResourcePath = "PixPost.Services.Available.";

  private static readonly ObservableCollection<ImageService> Services = [];

  /// <summary>
  /// Determines whatever the service exists.
  /// </summary>
  /// <param name="serviceName">The service name to check</param>
  /// <returns>True when found, false otherwise</returns>
  public static bool Exists(string serviceName) {
    return Services.Any(x => x.GetServiceName() == serviceName);
  }

  /// <summary>
  /// Returns all available/registered services
  /// </summary>
  /// <returns>The service collection</returns>
  public static ObservableCollection<ImageService> GetAll() {
    return Services;
  }

  /// <summary>
  /// Locates a service by name and returns if found.
  /// </summary>
  /// <param name="serviceName">The service name to file</param>
  /// <returns>The service instance</returns>
  /// <exception cref="KeyNotFoundException">When the service is not found</exception>
  public static ImageService Locate(string serviceName) {
    var service = Services.FirstOrDefault(s => s.GetServiceName() == serviceName);
    return service ?? throw new KeyNotFoundException($"The service '{serviceName}' does not exist");
  }

  /// <summary>
  /// Sets the given service as the default service
  /// </summary>
  /// <param name="serviceName">The service name to set</param>
  /// <exception cref="KeyNotFoundException">When the service is not found</exception>
  public static void SetCurrent(string serviceName) {
    if (!Exists(serviceName))
      throw new KeyNotFoundException($"The service '{serviceName}' does not exist");

    App.CurrentService = serviceName;
    ConfigHelper.Save(new() { { ConfigKey, serviceName } });
  }

  /// <summary>
  /// Returns the current service instance
  /// </summary>
  /// <returns>The service instance</returns>
  public static ImageService GetCurrent() {
    return Locate(App.CurrentService);
  }

  /// <summary>
  /// Determines whatever the current service is set or not
  /// </summary>
  /// <returns>True upon set, false otherwise</returns>
  public static bool HasCurrent() {
    return !string.IsNullOrWhiteSpace(App.CurrentService);
  }

  /// <summary>
  /// Populates services definitions and register globally.
  /// </summary>
  private static void PopulateServices() {
    var schemaFiles = FindDefinitions();

    foreach (var schemaFile in schemaFiles) {
      ImageService service;

      if (schemaFile.StartsWith(Protocol)) {
        var text = schemaFile.Replace(Protocol, ResourcePath);
        service = ServiceFactory
          .CreateFromText(ResourceHelper.GetTextResource(text), out _);
      }
      else
        service = ServiceFactory
        .CreateFromFile(schemaFile, out _);

      if (Services.All(x => x.GetServiceName() != service.GetServiceName()))
        Services.Add(service);
    }
  }

  /// <summary>
  /// Returns the list of native services' name
  /// </summary>
  /// <returns></returns>
  public static string[] GetNativeDefinitions() {
    return ResourceHelper.GetList()
      .Where(x => x.StartsWith(ResourcePath))
      .Select(x => x.Replace(ResourcePath, Protocol))
      .ToArray();
  }

  /// <summary>
  /// Gets all the active services name
  /// </summary>
  /// <returns>List of services' name</returns>
  public static string[] GetNames() {
    return Services.Select(x => x.GetServiceName()).ToArray();
  }

  public static void Load() {
    // Initialize service registry
    if (Services.Count == 0) PopulateServices();

    // Load from configuration
    var serviceName = ConfigHelper.GetString(ConfigKey, string.Empty);
    if (Exists(serviceName)) SetCurrent(serviceName);

    // Fallback to default service
    if (!HasCurrent()) SetCurrent(Services.First().GetServiceName());
  }

  /// <summary>
  /// File all services definitions (native, application services, and other in given directories)
  /// </summary>
  /// <param name="directories">Absolute directories path</param>
  /// <returns>Absolute files list of services definitions</returns>
  /// <exception cref="DirectoryNotFoundException"></exception>
  public static IList<string> FindDefinitions(string[]? directories = null) {
    List<string> definitions = [];

    definitions.AddRange(GetNativeDefinitions());

    var folders = new List<string> { Path.Join(Environment.CurrentDirectory, "Services") };
    folders.AddRange(directories ?? []);

    List<string> schemaFiles = [];

    foreach (var dirPath in folders) {
      if (!Directory.Exists(dirPath))
        throw new DirectoryNotFoundException($"Directory {dirPath} does not exist.");

      var files = Directory
        .GetFiles(dirPath)
        .Where(f => f.EndsWith(".service.json")).ToArray();
      schemaFiles.AddRange(files);
    }

    try {
      definitions
        .AddRange(schemaFiles
          .Where(schemaFile => ServiceFactory
            .TryParseSchemaFile(schemaFile, out _)));
    }
    catch (Exception e) {
      Console.WriteLine(e.Message);
    }

    return definitions;
  }
}