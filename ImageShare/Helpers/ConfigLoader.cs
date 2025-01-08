using System.IO;
using DotNetEnv;

namespace PixPost.Helpers;

public class ConfigLoader : Env {
  public static void LoadExisting() {
    var envFilePath = Path.Join(Environment.CurrentDirectory, ".env");
    if (!File.Exists(envFilePath)) {
      File.WriteAllText(envFilePath, ResourceLoader.ResourceList.GetInitialEnv());
    }

    Load(envFilePath);
  }
}