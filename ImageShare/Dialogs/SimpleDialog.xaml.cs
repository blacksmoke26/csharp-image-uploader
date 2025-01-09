using System.Windows;
using System.Windows.Input;

namespace PixPost.Dialogs;

public partial class SimpleDialog {
  public static readonly DependencyProperty HeadingTextProperty = DependencyProperty.Register(
    nameof(HeadingText), typeof(string), typeof(SimpleDialog), new PropertyMetadata(null,
      (o, args) => {
        var field = (SimpleDialog)o;
        field.HeadingTextBlock.Text = (string)args.NewValue;
      }));

  public string HeadingText {
    get => (string)GetValue(HeadingTextProperty);
    set => SetValue(HeadingTextProperty, value);
  }

  public static readonly DependencyProperty DescriptionTextProperty = DependencyProperty.Register(
    nameof(DescriptionText), typeof(string), typeof(SimpleDialog), new PropertyMetadata(null,
      (o, args) => {
        var field = (SimpleDialog)o;
        field.DescriptionTextBlock.Text = (string)args.NewValue;
      }));

  public string DescriptionText {
    get => (string)GetValue(DescriptionTextProperty);
    set => SetValue(DescriptionTextProperty, value);
  }

  public SimpleDialog() {
    InitializeComponent();
  }

  private void CloseTextBox_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }

  private void SimpleDialog_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }

  private void CloseButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }
}