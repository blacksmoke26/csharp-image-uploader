using System.Windows;
using System.Windows.Input;

namespace PixPost.Dialogs;

public partial class SettingsDialog {
  public SettingsDialog() {
    InitializeComponent();
  }

  private void CloseTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }

  private void MainWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }
}