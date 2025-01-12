using System.Collections.ObjectModel;
using System.Windows;

namespace PixPost.UserControls;

public class SidebarMenuItem {
  public delegate void ItemClicked(SidebarMenuItem item);

  public required string Id { get; set; } = string.Empty;
  public required string Label { get; set; } = string.Empty;
  public required string Icon { get; set; } = string.Empty;
  public bool HideDrawer { get; set; } = true;

  public event ItemClicked? ItemClick;

  public void OnItemClick() => ItemClick?.Invoke(this);
}

public partial class SidebarDrawer {
  public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(
    nameof(MenuItems), typeof(ObservableCollection<SidebarMenuItem>), typeof(SidebarDrawer),
    new PropertyMetadata(new ObservableCollection<SidebarMenuItem>(), (o, args) => {
      var obj = (SidebarDrawer)o;
      obj.SidebarMenu.ItemsSource = (ObservableCollection<SidebarMenuItem>)args.NewValue;
    }));

  public ObservableCollection<SidebarMenuItem> MenuItems {
    get => (ObservableCollection<SidebarMenuItem>)GetValue(MenuItemsProperty);
    set => SetValue(MenuItemsProperty, value);
  }

  private static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent(
    nameof(ItemClick), RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(SidebarDrawer)
  );

  public event RoutedEventHandler ItemClick {
    add => AddHandler(ItemClickEvent, value);
    remove => RemoveHandler(ItemClickEvent, value);
  }

  private void DrawerOutside_OnMouseDown(object sender, RoutedEventArgs e) {
    foreach (var item in SidebarMenu.Items) {
      var itm = (SidebarMenuItem)item;
      if (itm.Id == "Close")
        itm.OnItemClick();
    }
  }
  
  public SidebarDrawer() {
    InitializeComponent();
    SidebarMenu.ItemsSource = MenuItems;
  }
}