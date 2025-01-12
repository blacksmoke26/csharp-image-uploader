using System.Collections.ObjectModel;
using PixPost.Helpers;
using PixPost.Objects;

// ReSharper disable All

namespace PixPost;

/// <summary>
/// Interaction logic for App.xaml
/// https://icon-icons.com/icon/upload-pictures/93667
/// </summary>
public partial class App {
  public static ObservableCollection<Logging.LogItem> HistoryLogs { get; set; } = [];
  public static string CurrentService { get; set; } = string.Empty;

  public App() {
    ConfigHelper.Load();
    //ServiceManager.Load();
  }
}