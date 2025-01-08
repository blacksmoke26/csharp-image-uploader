using System.Windows;
using System.Windows.Input;

namespace PixPost.UserControls;

public partial class SidebarDrawer {
  private static readonly RoutedEvent DrawerCloseEvent = EventManager.RegisterRoutedEvent(
    nameof(DrawerClose), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(SidebarDrawer)
  );

  public event RoutedEventHandler DrawerClose {
    add => AddHandler(DrawerCloseEvent, value);
    remove => RemoveHandler(DrawerCloseEvent, value);
  }

  private void DrawerOutside_OnMouseDown(object sender, RoutedEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: DrawerCloseEvent));
  }

  private static readonly RoutedEvent ConfigurationClickEvent = EventManager.RegisterRoutedEvent(
    nameof(ConfigurationClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(SidebarDrawer)
  );

  public event RoutedEventHandler ConfigurationClick {
    add => AddHandler(ConfigurationClickEvent, value);
    remove => RemoveHandler(ConfigurationClickEvent, value);
  }

  private void ConfigurationMenuItem_OnClick(object sender, RoutedEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: ConfigurationClickEvent));
  }

  private static readonly RoutedEvent HistoryClickEvent = EventManager.RegisterRoutedEvent(
    nameof(HistoryClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(SidebarDrawer)
  );

  public event RoutedEventHandler HistoryClick {
    add => AddHandler(HistoryClickEvent, value);
    remove => RemoveHandler(HistoryClickEvent, value);
  }

  private void HistoryMenuItem_OnClick(object sender, RoutedEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: HistoryClickEvent));
  }

  public SidebarDrawer() {
    InitializeComponent();
  }

  private void ExitMenuItem_OnMouseDown(object sender, MouseButtonEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: DrawerCloseEvent));
  }
}