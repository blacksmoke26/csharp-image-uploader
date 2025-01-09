using System.Diagnostics;
using System.Windows.Input;

namespace PixPost.Dialogs;

public partial class SettingsDialog {
  public SettingsDialog() {
    InitializeComponent();
    EndpointTextBox.Focus();
  }

  private void CloseTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }

  private void MainWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void WebUrlClick_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Process.Start(new ProcessStartInfo {
      FileName = "https://api.imgbb.com/",
      UseShellExecute = true
    });
  }

  private void SettingsDialog_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }
}