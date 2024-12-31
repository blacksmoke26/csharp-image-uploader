using System.Windows;
using Dumpify;
using ImageShare.Dialogs;
using ImageShare.Helpers;

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

    if (filePaths == null || filePaths.Length == 0) return;

    foreach (var path in filePaths) {
      var imageThumb = new ImageThumb(path);
      if (imageThumb.IsProcessed) _uploadedImages.Add(imageThumb);
    }

    _uploadedImages[0].Dump();
  }

  private void StartUploadingButton_OnClick(object sender, RoutedEventArgs e) {
    var dialog = new EditImageDialog();
    //dialog.Owner = this;
    dialog.ShowDialog();
  }
}