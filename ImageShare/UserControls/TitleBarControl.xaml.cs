using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PixPost.UserControls;

public partial class TitleBarControl {
  public TitleBarControl() {
    InitializeComponent();
  }

  private static readonly RoutedEvent NotificationsClickEvent = EventManager.RegisterRoutedEvent(
    nameof(NotificationsClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler NotificationsClick {
    add => AddHandler(NotificationsClickEvent, value);
    remove => RemoveHandler(NotificationsClickEvent, value);
  }

  private static readonly RoutedEvent MinimizeClickEvent = EventManager.RegisterRoutedEvent(
    nameof(MinimizeClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler MinimizeClick {
    add => AddHandler(MinimizeClickEvent, value);
    remove => RemoveHandler(MinimizeClickEvent, value);
  }

  private static readonly RoutedEvent SidebarClickEvent = EventManager.RegisterRoutedEvent(
    nameof(SidebarClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler SidebarClick {
    add => AddHandler(SidebarClickEvent, value);
    remove => RemoveHandler(SidebarClickEvent, value);
  }

  private static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent(
    nameof(CloseClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(Button)
  );

  public event RoutedEventHandler CloseClick {
    add => AddHandler(CloseClickEvent, value);
    remove => RemoveHandler(CloseClickEvent, value);
  }

  private void Notifications_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: NotificationsClickEvent));
  }

  private void ShowSidebar_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: SidebarClickEvent));
  }

  private void Minimize_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: MinimizeClickEvent));
  }

  private void Close_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: CloseClickEvent));
  }
}