using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using PixPost.Helpers;

namespace PixPost.Dialogs;

public partial class SettingsDialog {
  public SettingsDialog() {
    InitializeComponent();
    ApiKeyTextBox.Focus();
    EndpointTextBox.Text = ConfigHelper.GetEndpoint();
    ApiKeyTextBox.Text = ConfigHelper.GetApiKey();
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
    if (!Uri.TryCreate(EndpointTextBox.Text, UriKind.Absolute, out _)) {
      DialogHelper.ShowErrorDialog(this, "Endpoint is not a valid URL.", "Validation Error",
        dialog => { dialog.Closed += (_, _) => EndpointTextBox.Focus(); });
      return;
    }

    if (string.IsNullOrWhiteSpace(ApiKeyTextBox.Text)) {
      DialogHelper.ShowErrorDialog(this, "API key should not be empty.", "Validation Error",
        dialog => { dialog.Closed += (_, _) => ApiKeyTextBox.Focus(); });
      return;
    }

    if (ApiKeyTextBox.Text.Length != 32) {
      DialogHelper.ShowErrorDialog(this, "The API key you have entered is invalid.", "Validation Error",
        dialog => { dialog.Closed += (_, _) => ApiKeyTextBox.Focus(); });
      return;
    }

    var dict = ConfigHelper.ReadFile(ConfigHelper.GetEnvFilePath());
    dict[ConfigHelper.ImgBbEndpoint] = EndpointTextBox.Text;
    dict[ConfigHelper.ImgBbApiKey] = ApiKeyTextBox.Text;
    ConfigHelper.SaveFile(ConfigHelper.GetEnvFilePath(), dict, true);
    Close();
  }

  private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }
}