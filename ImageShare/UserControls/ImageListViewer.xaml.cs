using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dumpify;
using ImageShare.Helpers;

namespace ImageShare.UserControls;

public partial class ImageListViewer : UserControl {
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

  private void ImageEdit_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(EditClickEvent) {
      Source = (sender as Border)?.DataContext
    });
  }

  private void ImageRemove_OnMouseDown(object sender, MouseButtonEventArgs e) {
    ImagesList.Remove((ImageThumb)(sender as Border)?.DataContext!);
    RaiseEvent(new RoutedEventArgs(RemoveClickEvent));
  }
}