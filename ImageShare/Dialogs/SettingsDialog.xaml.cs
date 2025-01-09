using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using PixPost.Helpers;

namespace PixPost.Dialogs;

public partial class SettingsDialog {
  public SettingsDialog() {
    InitializeComponent();
    EndpointTextBox.Focus();
    EndpointTextBox.Text = ConfigLoader.GetEndpoint();
    ApiKeyTextBox.Text = ConfigLoader.GetApiKey();
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

  private void UpdateButton_OnClick(object sender, RoutedEventArgs e) {
    DialogHelper.ShowErrorDialog(this, "Test message", "Caption");
  }

  private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }
}