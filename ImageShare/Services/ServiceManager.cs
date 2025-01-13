using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using PixPost.Helpers;
using PixPost.Objects.Service;

namespace PixPost.Services;

public static class ServiceManager {
  private static readonly Dictionary<string, ImageService> ServicesMap = [];
  private static readonly ObservableCollection<ImageService> Services = [];
  private const string ConfigName = "CURRENT_SERVICE";

  private static T CreateInstance<T>(string ns) {
    var type = Type.GetType(ns);
    return (T)Activator.CreateInstance(type!)!;
  }

  static ServiceManager() {
    if (ServicesMap.Count == 0) {
      ServicesMap = Directory.GetDirectories(path: Path.Join(".", "Services"))
        .Select(PopulateService).ToDictionary();
    }
  }

  //TODO: Testing required
  public static string[] GetResourceServicesSchemas() {
    return ResourceManager.GetList()
      .Where(x => x.StartsWith("PixPost.Services.Available."))
      .ToArray();
  }

  private static KeyValuePair<string, ImageService> PopulateService(string path) {
    var instance = CreateInstance<ImageService>(ToNamespace(path));
    return new(instance.GetServiceName(), instance);
  }

  private static string ToNamespace(string path) {
    var list = path.Split(Path.DirectorySeparatorChar).ToList();
    list.RemoveAt(0);
    list.Insert(0, Assembly.GetExecutingAssembly().GetName().Name ?? "");
    list.Add(list.Last());
    return string.Join(".", list.ToArray());
  }

  public static string[] GetNames() {
    return Services.Select(x => x.GetServiceName()).ToArray();
  }

  public static void Load() {
    if (Services.Count > 0) return;

    var configService = ConfigHelper.GetString(ConfigName, string.Empty);

    foreach (var (serviceName, imageService) in ServicesMap) {
      Services.Add(imageService);

      if (configService == serviceName) SetCurrent(serviceName);
      if (string.IsNullOrWhiteSpace(configService)) configService = serviceName;
    }

    if (!HasCurrent()) SetCurrent(configService);
  }

  public static List<string> FindDefinitions(string[]? directories = null) {
    List<string> definitions = [];
    definitions.AddRange(GetResourceServicesSchemas());

    if (directories == null)
      return definitions;

    foreach (var dirPath in directories) {
      if (!Directory.Exists(dirPath)) {
        throw new DirectoryNotFoundException($"Directory {dirPath} does not exist.");
      }

      // TODO: Implement logic here
      //var files = Directory.GetFiles(dirPath).Where(f => f.EndsWith(".Service.json")).ToArray();
      /*if (files.Length)
        definitions.AddRange(files);*/
    }

    return definitions;
  }

  public static bool Exists(string name) => ServicesMap.ContainsKey(name);

  public static T Locate<T>(string id) where T : ImageService {
    var service = Services.FirstOrDefault(s => s.GetServiceName() == id);
    return service != null
      ? (T)service
      : throw new KeyNotFoundException($"The service '{id}' does not exist");
  }

  public static ObservableCollection<ImageService> GetAll() => Services;

  public static void SetCurrent(string name) {
    if (!Exists(name)) {
      throw new KeyNotFoundException($"The service '{name}' does not exist");
    }

    App.CurrentService = name;
    var config = new Dictionary<string, string> { { ConfigName, name } };
    ConfigHelper.Save(config);
  }

  public static T GetCurrent<T>() where T : ImageService {
    return Locate<T>(App.CurrentService);
  }

  public static bool HasCurrent() {
    return !string.IsNullOrWhiteSpace(App.CurrentService);
  }
}