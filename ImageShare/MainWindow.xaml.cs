using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ImageShare.Dialogs;
using ImageShare.Helpers;
using Microsoft.Win32;
using System.IO;

namespace ImageShare;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {
  private readonly BindingList<ImageThumb> _uploadedImages = new BindingList<ImageThumb>() {
    AllowRemove = true,
    AllowEdit = true,
    RaiseListChangedEvents = true,
  };

  public MainWindow() {
    InitializeComponent();
    FileInfoPanel.Visibility = Visibility.Visible;

    PreviewDrop += MainWindow_Drop;
    MouseDown += MainWindow_OnMouseDown;
    ImagesViewer.ImagesList = _uploadedImages;
    PreviewDragEnter += (_, _) => DragPreviewPanel.Visibility = Visibility.Visible;
    PreviewDragLeave += (_, _) => DragPreviewPanel.Visibility = Visibility.Collapsed;
    _uploadedImages.ListChanged += (_, e) => {
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

  private void ProcessUploadedImageList(string[] files) {
    if (files.Length == 0) return;
    List<string> invalidFilesList = [];

    var i = 0;
    foreach (var path in files) {
      if (_uploadedImages.Any(x => string.Equals(x.Source, path, StringComparison.CurrentCultureIgnoreCase))) continue;

      var imageThumb = new ImageThumb(path);
      if (imageThumb.IsLoaded) {
        _uploadedImages.Add(imageThumb);
      }
      else invalidFilesList.Add($"{Path.GetFileName(path)} - Invalid or supported file format.");
    }

    if (invalidFilesList.Count <= 0) return;

    var dialog = new SimpleDialog {
      HeadingText = "Some files couldn't be added",
      DescriptionText = string.Join("\n", invalidFilesList.ToArray()),
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
}