using System.Windows;
using System.Windows.Input;
using ImageShare.Helpers;
using Microsoft.Win32;

namespace ImageShare;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
  private readonly List<ImageThumb> _uploadedImages = [];

  public MainWindow() {
    InitializeComponent();
    FileAreaBorder.DragEnter += (_, _) => { DragPreviewPanelVisible(); };
    FileAreaBorder.DragLeave += (_, _) => { FileInfoPanelVisible(); };
    FileAreaBorder.Drop += FileAreaBorder_Drop;
  }

  private void DragPreviewPanelVisible() {
    DragPreviewPanel.Visibility = Visibility.Visible;
    FileInfoPanel.Visibility = Visibility.Collapsed;
  }

  private void FileInfoPanelVisible() {
    DragPreviewPanel.Visibility = Visibility.Collapsed;
    FileInfoPanel.Visibility = Visibility.Visible;
  }

  private void FileAreaBorder_Drop(object sender, DragEventArgs e) {
    FileInfoPanelVisible();

    if (!e.Data.GetDataPresent(DataFormats.FileDrop)) {
      return;
    }

    var filePaths = e.Data.GetData(DataFormats.FileDrop, false) as string[];

    if (filePaths != null)
      ProcessUploadedImageList(filePaths);
  }

  private void ProcessUploadedImageList(string[] files) {
    if (files.Length == 0) return;

    foreach (var path in files) {
      var imageThumb = new ImageThumb(path);
      if (imageThumb.IsProcessed) _uploadedImages.Add(imageThumb);
    }

    /*ControlImageGridView.ImagesList = _uploadedImages;*/
  }

  private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e) {
    if (e.ButtonState == MouseButtonState.Pressed) {
      DragMove();
    }
  }

  private void ImageDropArea_OnUploadClick(object sender, RoutedEventArgs e) {
    var extensions = ImageHelper.ImageExtTypes.Select(x => $"*.{x}").ToArray();

    OpenFileDialog fileDialog = new() {
      Filter = $"Image Files ({string.Join(",", extensions)})|{string.Join(";", extensions)}",
      Multiselect = true,
      InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
    };

    if (fileDialog.ShowDialog() != null)
      ProcessUploadedImageList(fileDialog.FileNames);
  }
}