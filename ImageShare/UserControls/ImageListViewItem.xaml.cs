using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ImageShare.Helpers;

namespace ImageShare.UserControls;

public partial class ImageListViewItem {
  private static readonly RoutedEvent RemoveClickEvent = EventManager.RegisterRoutedEvent(
    nameof(RemoveClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(ImageListViewItem)
  );

  public event RoutedEventHandler RemoveClick {
    add => AddHandler(RemoveClickEvent, value);
    remove => RemoveHandler(RemoveClickEvent, value);
  }

  private static readonly RoutedEvent EditClickEvent = EventManager.RegisterRoutedEvent(
    nameof(EditClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(ImageListViewItem)
  );

  public event RoutedEventHandler EditClick {
    add => AddHandler(EditClickEvent, value);
    remove => RemoveHandler(EditClickEvent, value);
  }

  public static readonly DependencyProperty ImageItemProperty = DependencyProperty.Register(
    nameof(ImageItem), typeof(ImageThumb), typeof(ImageListViewItem),
    new PropertyMetadata(null, (o, args) => OnProcessingImage((ImageListViewItem)o, (ImageThumb)args.NewValue))
  );

  private static void OnProcessingImage(ImageListViewItem control, ImageThumb image) {
    var buttonVisibility = image.ApiResponse != null || image.LastError != null || image.IsProcessing
      ? Visibility.Hidden
      : Visibility.Visible;

    control.EditBorder.Visibility = buttonVisibility;
    control.RemoveBorder.Visibility = buttonVisibility;

    // Process the image area
    control.ProcessingArea.Visibility = image.IsProcessing ? Visibility.Visible : Visibility.Collapsed;
    control.FailedArea.Visibility = image.LastError != null ? Visibility.Visible : Visibility.Collapsed;
    control.ViewArea.Visibility = image.ApiResponse != null ? Visibility.Visible : Visibility.Collapsed;
  }

  public ImageThumb ImageItem {
    get => (ImageThumb)GetValue(ImageItemProperty);
    set => SetValue(ImageItemProperty, value);
  }

  public ImageListViewItem() {
    InitializeComponent();
    ViewArea.MouseEnter += (_, _) => {
      ViewAreaIcon.Text = "\uE310";
      ViewAreaIcon.Foreground = Brushes.SlateGray;
    };
    
    ViewArea.MouseLeave += (_, _) => {
      ViewAreaIcon.Text = "\uE182";
      ViewAreaIcon.Foreground = Brushes.ForestGreen;
    };
  }

  private void ImageEditButton_MouseDown(object sender, RoutedEventArgs e) {
    // Create a RoutedEventArgs instance.
    RoutedEventArgs routedEventArgs = new(routedEvent: EditClickEvent);

    // Raise the event, which will bubble up through the element tree.
    RaiseEvent(routedEventArgs);
  }

  private void ImageRemoveButton_MouseDown(object sender, RoutedEventArgs e) {
    // Create a RoutedEventArgs instance.
    RoutedEventArgs routedEventArgs = new(routedEvent: RemoveClickEvent);

    // Raise the event, which will bubble up through the element tree.
    RaiseEvent(routedEventArgs);
  }
}