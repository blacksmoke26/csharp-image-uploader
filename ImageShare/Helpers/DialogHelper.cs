using System.Windows;
using PixPost.Dialogs;

namespace PixPost.Helpers;

public static class DialogHelper {
  public static void ShowErrorDialog(Window window, string description, string heading, Action<SimpleDialog>? callback) {
    var dialog = new SimpleDialog() {
      HeadingText = heading,
      DescriptionText = description,
      Owner = window,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
    };

    callback?.Invoke(dialog);
    dialog.ShowDialog();
  }
}