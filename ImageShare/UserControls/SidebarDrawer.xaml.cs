using System.Windows;
using System.Windows.Input;

namespace PixPost.UserControls;

public partial class SidebarDrawer {
  private static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
    nameof(Close), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(SidebarDrawer)
  );

  public event RoutedEventHandler Close {
    add => AddHandler(CloseEvent, value);
    remove => RemoveHandler(CloseEvent, value);
  }

  private void DrawerOutside_OnMouseDown(object sender, RoutedEventArgs e) {
    RaiseEvent(new RoutedEventArgs(routedEvent: CloseEvent));
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
    if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
  }

  private void SidebarDrawer_OnPreviewKeyDown(object sender, KeyEventArgs e) {
    // TODO: Close click
  }
}