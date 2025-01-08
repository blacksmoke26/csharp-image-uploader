using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PixPost.Dialogs;
using PixPost.Helpers;
using Microsoft.Win32;
using System.IO;

namespace PixPost;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {
  private readonly BindingList<ImageThumb> _uploadedImages = new() {
    AllowRemove = true,
    AllowEdit = true,
    RaiseListChangedEvents = true,
  };

  public MainWindow() {
    InitializeComponent();
    PreviewHeaderFilesInfo.Text = string.Concat(
      string.Join(" ", ImageHelper.ImageExtTypes),
      " ", ImageBb.SizeToMb(ImageBb.MaxSize), " MB").ToUpper();
    FileInfoPanel.Visibility = Visibility.Visible;
    ExpiryComboBox.ItemsSource = ImageBb.Expirations;
    PreviewDrop += MainWindow_Drop;
    MouseDown += MainWindow_OnMouseDown;
    ImagesViewer.ImagesList = _uploadedImages;
    PreviewDragEnter += (_, _) => DragPreviewPanel.Visibility = Visibility.Visible;
    PreviewDragLeave += (_, _) => DragPreviewPanel.Visibility = Visibility.Collapsed;
    _uploadedImages.ListChanged += (_, _) => {
      FileInfoPanel.Visibility = _uploadedImages.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
      ImagesViewer.ImagesList = _uploadedImages;
    };
    ImagesViewer.EditClick += (_, e) => {
      var dialog = new EditImageDialog((ImageThumb)e.OriginalSource);
      dialog.ShowDialog();
    };
  }

  private void MainWindow_Drop(object sender, DragEventArgs e) {
    DragPreviewPanel.Visibility = Visibility.Collapsed;

    if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

    if (e.Data.GetData(DataFormats.FileDrop, true) is string[] filePaths)
      ProcessUploadedImageList(filePaths);
  }

  private bool ValidateFile(string filePath, List<string> errors) {
    if (_uploadedImages.Any(f => string.Equals(f.Source, filePath, StringComparison.CurrentCultureIgnoreCase)))
      return false;

    var info = new FileInfo(filePath);

    if (info.Length <= 0) {
      errors.Add($"{Path.GetFileName(filePath)} - Empty file.");
      return false;
    }

    if (info.Length > ImageBb.MaxSize) {
      errors.Add($"{Path.GetFileName(filePath)} - File is too big.");
      return false;
    }

    return true;
  }

  private void ProcessUploadedImageList(string[] files) {
    if (files.Length == 0) return;
    List<string> errors = [];

    foreach (var path in files) {
      if (!ValidateFile(path, errors)) continue;

      var imageThumb = new ImageThumb(path);

      if (!imageThumb.IsLoaded) {
        errors.Add($"{Path.GetFileName(path)} - Invalid or supported file format.");
        continue;
      }

      _uploadedImages.Add(imageThumb);
    }

    if (errors.Count <= 0) return;

    var dialog = new SimpleDialog {
      HeadingText = "Some files couldn't be added",
      DescriptionText = string.Join("\n", errors.ToArray()),
      Owner = this,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
    };
    dialog.ShowDialog();
  }

  private void MainWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void ImageDropArea_OnUploadClick(object _, RoutedEventArgs e) {
    var extensions = ImageHelper.ImageExtTypes.Select(x => $"*.{x}").ToArray();

    OpenFileDialog fileDialog = new() {
      Filter = $"Image Files ({string.Join(",", extensions)})|{string.Join(";", extensions)}",
      Multiselect = true,
      InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
    };

    if (fileDialog.ShowDialog() != null)
      ProcessUploadedImageList(fileDialog.FileNames);
  }

  private void ContentElement_OnMouseDown(object sender, MouseButtonEventArgs e) {
    _uploadedImages.Clear();
  }

  private void WindowMinimize_OnMouseDown(object sender, MouseButtonEventArgs e) {
    WindowState = WindowState.Minimized;
  }

  private void WindowClose_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }

  private void UploadButton_OnClick(object sender, RoutedEventArgs e) {
    var img = _uploadedImages.FirstOrDefault()!;
    var idx = _uploadedImages.IndexOf(img);
    img.IsProcessing = true;
    _uploadedImages.ResetItem(idx);
  }
}