using System.Windows;
using System.Windows.Input;

namespace PixPost.Ui.Dialogs;

public partial class BaseDialog {
  public static readonly DependencyProperty DialogHeadingProperty = DependencyProperty.Register(
    nameof(DialogHeading), typeof(string), typeof(BaseDialog), new PropertyMetadata(default(string)));

  public string DialogHeading {
    get => (string)GetValue(DialogHeadingProperty);
    set => SetValue(DialogHeadingProperty, value);
  }

  private static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent(
    nameof(CloseClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(BaseDialog)
  );

  public event RoutedEventHandler CloseClick {
    add => AddHandler(CloseClickEvent, value);
    remove => RemoveHandler(CloseClickEvent, value);
  }

  public BaseDialog() {
    InitializeComponent();
    DataContext = this;
  }

  private void BaseCloseTextBox_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: CloseClickEvent));
  }
}