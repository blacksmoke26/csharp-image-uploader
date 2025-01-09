using System.IO;
using DotNetEnv;

namespace PixPost.Helpers;

public static class ConfigLoader {
  private const string ImgBbEndpoint = "IMGBB_ENDPOINT";
  private const string ImgBbApiKey = "IMGBB_API_KEY";

  public static void LoadExisting() {
    var envFilePath = Path.Join(Environment.CurrentDirectory, ".env");
    if (!File.Exists(envFilePath)) {
      File.WriteAllText(envFilePath, ResourceLoader.ResourceList.GetInitialEnv());
    }

    Env.Load(envFilePath);
  }

  public static string GetEndpoint() {
    return Env.GetString(ImgBbEndpoint).Trim();
  }

  public static string GetApiKey() {
    return Env.GetString(ImgBbApiKey).Trim();
  }
}