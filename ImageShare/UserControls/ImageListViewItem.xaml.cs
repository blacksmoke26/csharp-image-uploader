using System.Windows;
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

  public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
    nameof(ImageSource), typeof(ImageThumb), typeof(ImageListViewItem),
    new PropertyMetadata(default(ImageThumb))
  );

  public ImageThumb ImageSource {
    get => (ImageThumb)GetValue(ImageSourceProperty);
    set => SetValue(ImageSourceProperty, value);
  }

  public ImageListViewItem() {
    InitializeComponent();
  }

  private void ImageRemoveButton_OnClick(object sender, RoutedEventArgs e) {
    // Create a RoutedEventArgs instance.
    RoutedEventArgs routedEventArgs = new(routedEvent: RemoveClickEvent);

    // Raise the event, which will bubble up through the element tree.
    RaiseEvent(routedEventArgs);
  }
}