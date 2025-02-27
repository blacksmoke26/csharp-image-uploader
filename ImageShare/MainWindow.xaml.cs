﻿using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PixPost.Dialogs;
using PixPost.Helpers;
using PixPost.Objects;
using PixPost.UserControls;
using PixPost.Utils;

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
    InitializeSidebarDrawer();
    InitializeWindowTitleBar();
    InitializeUi();
  }

  private void InitializeWindowTitleBar() {
    WindowTitleBarControl.MinimizeClick += (_, _) => { WindowState = WindowState.Minimized; };
    WindowTitleBarControl.SidebarClick += (_, _) => { SidebarDrawer.Visibility = Visibility.Visible; };
    WindowTitleBarControl.CloseClick += (_, _) => { Close(); };
    WindowTitleBarControl.NotificationsClick += (_, _) => {
      var dialog = new HistoryDialog {
        Owner = Application.Current.MainWindow,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
      };
      dialog.ShowDialog();
    };
  }

  private void InitializeUi() {
    //WindowTitleBarControl.CloseClick += TitleBar_CloseClick;
    //WindowTitleBarControl.NotificationsClick += TitleBar_NotificationsClick;
    //
    /*TitleBar_MinimizeClick
      TitleBar_SidebarClick
    TitleBar_CloseClick
      TitleBar_NotificationsClick*/

    /*PreviewHeaderFilesInfo.Text = string.Concat(
      string.Join(" ", ImageHelper.ImageExtTypes),
      " ", ImageBb.SizeToMb(ImageBb.MaxSize), " MB").ToUpper();
    FileInfoPanel.Visibility = Visibility.Visible;
    ExpiryComboBox.ItemsSource = ImageBb.Expirations;
    PreviewDrop += MainWindow_Drop;
    MouseDown += MainWindow_OnMouseDown;
    ImagesViewer.ImagesList = _uploadedImages;*/

    #region Events Binding

    /*PreviewDragEnter += (_, _) => DragPreviewPanel.Visibility = Visibility.Visible;
    PreviewDragLeave += (_, _) => DragPreviewPanel.Visibility = Visibility.Collapsed;
    _uploadedImages.ListChanged += (_, _) => {
      FileInfoPanel.Visibility = _uploadedImages.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
      ImagesViewer.ImagesList = _uploadedImages;
    };
    ImagesViewer.EditClick += (_, e) => {
      var dialog = new EditImageDialog((ImageThumb)e.OriginalSource);
      dialog.ShowDialog();
    };*/

    #endregion
  }

  private void InitializeSidebarDrawer() {
    var items = SidebarUtils.GetMenuItems(menuItem => {
      if (menuItem.HideDrawer)
        SidebarDrawer.Visibility = Visibility.Collapsed;
      SidebarUtils.MenuItemClickHandler(menuItem);
    });
    
    foreach (var menuItem in items) {
      SidebarDrawer.MenuItems.Add(menuItem);
    }
  }

  private void MainWindow_Drop(object sender, DragEventArgs e) {
    /*DragPreviewPanel.Visibility = Visibility.Collapsed;

    if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

    if (e.Data.GetData(DataFormats.FileDrop, true) is string[] filePaths)
      ProcessUploadedImageList(filePaths);*/
  }

  private bool ValidateFile(string filePath, List<string> errors) {
    if (_uploadedImages.Any(f => string.Equals(f.Source, filePath, StringComparison.CurrentCultureIgnoreCase)))
      return false;

    var info = new FileInfo(filePath);

    switch (info.Length) {
      case <= 0:
        errors.Add($"{Path.GetFileName(filePath)} - Empty file.");
        return false;
      case > ImageBb.MaxSize:
        errors.Add($"{Path.GetFileName(filePath)} - File is too big.");
        return false;
      default:
        return true;
    }
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

  private void DrawerIcon_OnMouseDown(object sender, MouseButtonEventArgs e) {
    SidebarDrawer.Visibility = Visibility.Visible;
  }

  private void MainWindow_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape && SidebarDrawer.Visibility == Visibility.Visible)
      SidebarDrawer.Visibility = Visibility.Collapsed;
  }
}