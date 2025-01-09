using System.ComponentModel;
using PixPost.Helpers;
using PixPost.Objects;

namespace PixPost;

/// <summary>
/// Interaction logic for App.xaml
/// https://icon-icons.com/icon/upload-pictures/93667
/// </summary>
public partial class App {
  public static BindingList<Logging.LogItem> LogItems { get; set; } = [];

  public App() {
    ConfigLoader.Load();
  }
}