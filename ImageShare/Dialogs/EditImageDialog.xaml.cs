using System.Windows;
using System.Windows.Input;

namespace ImageShare.Dialogs;

public partial class EditImageDialog : Window {
  public EditImageDialog() {
    InitializeComponent();
    Owner = Application.Current.MainWindow;
  }

  private void CloseTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }
}