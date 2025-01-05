using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageShare.Helpers;

namespace ImageShare.Dialogs;

public partial class EditImageDialog {
  public static readonly DependencyProperty ImageItemProperty = DependencyProperty.Register(
    nameof(ImageItem), typeof(ImageThumb), typeof(EditImageDialog), new PropertyMetadata(default(ImageThumb)));

  public ImageThumb ImageItem {
    get => (ImageThumb)GetValue(ImageItemProperty);
    set => SetValue(ImageItemProperty, value);
  }

  public EditImageDialog(ImageThumb imageThumb) {
    InitializeComponent();
    Owner = Application.Current.MainWindow;
    DataContext = ImageItem = imageThumb;
    KeyDown += (_, args) => {
      if (args.Key == Key.Escape) Close();
    };

    WidthTextBox.PreviewTextInput += NumericTextBox_NumberOnly;
    WidthTextBox.PreviewKeyDown += NumericTextBox_KeyDown;
    WidthTextBox.MouseWheel += NumericTextBox_MouseWheel;
    HeightTextBox.PreviewTextInput += NumericTextBox_NumberOnly;
    HeightTextBox.PreviewKeyDown += NumericTextBox_KeyDown;
    HeightTextBox.MouseWheel += NumericTextBox_MouseWheel;

    ResetWidthIcon.MouseDown += (_, _) => WidthTextBox.Text = ImageItem.Width.ToString();
    ResetHeightIcon.MouseDown += (_, _) => HeightTextBox.Text = ImageItem.Height.ToString();
  }

  private void NumericTextBox_MouseWheel(object sender, MouseWheelEventArgs e) {
    var ctrl = (TextBox)sender;
    ctrl.Text = e.Delta switch {
      > 0 => $"{int.Parse(ctrl.Text) + 1}",
      < 0 => $"{int.Parse(ctrl.Text) - 1}",
      _ => ctrl.Text
    };
  }

  private static void NumericTextBox_KeyDown(object o, KeyEventArgs e) {
    var ctrl = (TextBox)o;

    ctrl.Text = e.Key switch {
      Key.Up when int.TryParse(ctrl.Text, out var uv) => $"{uv + 1}",
      Key.Down when int.TryParse(ctrl.Text, out var dv) => $"{dv - 1}",
      _ => ctrl.Text
    };
  }

  private void NumericTextBox_Pasting(object sender, DataObjectPastingEventArgs e) {
    if (e.DataObject.GetDataPresent(typeof(string))) {
      var text = (string)e.DataObject.GetData(typeof(string))!;
      if (!int.TryParse(text, out _)) e.CancelCommand();
    }
    else e.CancelCommand();
  }

  private static void NumericTextBox_NumberOnly(object o, TextCompositionEventArgs e) {
    e.Handled = !int.TryParse(e.Text, out var _);
  }

  private void CloseTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }

  private void MainWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void SubmitButton_OnClick(object sender, RoutedEventArgs e) {
    if (string.IsNullOrEmpty(WidthTextBox.Text) || int.Parse(WidthTextBox.Text) <= 0) {
      var dialog = new SimpleDialog {
        Width = 250,
        HeadingText = "Invalid Width",
        DescriptionText = "The width value must be greater than zero.",
        Owner = this,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
      };
      dialog.ShowDialog();
      dialog.Closing += (_, _) => { WidthTextBox.Focus(); };
      return;
    }

    if (string.IsNullOrEmpty(HeightTextBox.Text) || int.Parse(HeightTextBox.Text) <= 0) {
      var dialog = new SimpleDialog {
        Width = 250,
        HeadingText = "Invalid Height",
        DescriptionText = "The height value must be greater than zero.",
        Owner = this,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
      };
      dialog.ShowDialog();
      dialog.Closing += (_, _) => { HeightTextBox.Focus(); };
      return;
    }
    
    ImageItem.Resize(int.Parse(WidthTextBox.Text), int.Parse(HeightTextBox.Text));
    
    if (string.Equals(TitleTextBox.Text, ImageItem.Title, StringComparison.CurrentCultureIgnoreCase)) {
      ImageItem.Title = TitleTextBox.Text;
    }
    
    Close();
  }
}