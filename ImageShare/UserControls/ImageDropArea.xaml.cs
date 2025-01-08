using System.Windows;
using System.Windows.Controls;

namespace PixPost.UserControls;

public partial class ImageDropArea {
  public ImageDropArea() {
    InitializeComponent();
  }

  private static readonly RoutedEvent UploadClickEvent = EventManager.RegisterRoutedEvent(
    nameof(UploadClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler UploadClick {
    add => AddHandler(UploadClickEvent, value);
    remove => RemoveHandler(UploadClickEvent, value);
  }

  private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
    // Raise the event, which will bubble up through the element tree.
    RaiseEvent(new RoutedEventArgs(routedEvent: UploadClickEvent));
  }
}