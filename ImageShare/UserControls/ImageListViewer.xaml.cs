using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ImageShare.Helpers;

namespace ImageShare.UserControls;

public partial class ImageListViewer {
  public static readonly DependencyProperty ImagesListProperty = DependencyProperty.Register(
    nameof(ImagesList), typeof(BindingList<ImageThumb>), typeof(ImageListViewer),
    new PropertyMetadata(null, ImagesListChangedCallback));

  private static void ImagesListChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    if (d is not ImageListViewer control || e.NewValue == null) return;

    var list = (BindingList<ImageThumb>)e.NewValue;
    control.ThumbsListView.ItemsSource = list;
  }

  public BindingList<ImageThumb> ImagesList {
    get => (BindingList<ImageThumb>)GetValue(ImagesListProperty);
    set => SetValue(ImagesListProperty, value);
  }

  private static readonly RoutedEvent RemoveClickEvent = EventManager.RegisterRoutedEvent(
    nameof(RemoveClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler RemoveClick {
    add => AddHandler(RemoveClickEvent, value);
    remove => RemoveHandler(RemoveClickEvent, value);
  }

  private static readonly RoutedEvent EditClickEvent = EventManager.RegisterRoutedEvent(
    nameof(EditClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler EditClick {
    add => AddHandler(EditClickEvent, value);
    remove => RemoveHandler(EditClickEvent, value);
  }

  public ImageListViewer() {
    InitializeComponent();
    ThumbsListView.ItemsSource = ImagesList;
  }

  private void ImageListViewItem_OnEditClick(object sender, RoutedEventArgs e) {
    var obj = (ImageListViewItem)sender;
    RaiseEvent(new RoutedEventArgs(EditClickEvent) {
      Source = obj.ImageItem
    });
  }

  private void ImageListViewItem_OnRemoveClick(object sender, RoutedEventArgs e) {
    var obj = (ImageListViewItem)sender;
    ImagesList.Remove(obj.ImageItem);
    RaiseEvent(new RoutedEventArgs(RemoveClickEvent));
  }
}