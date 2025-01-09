using System.Windows;
using System.Windows.Input;

namespace PixPost.Ui.Dialogs;

public class BaseDialogWindow: Window {
  private protected void BaseWindow_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }

  private protected void BaseCloseButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }

  private protected void BaseWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }
}