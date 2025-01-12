using System.Collections.ObjectModel;
using System.Windows;
using PixPost.Dialogs;
using PixPost.Objects.Service;
using PixPost.Services;
using PixPost.UserControls;

namespace PixPost.Utils;

using ItemClicked = SidebarMenuItem.ItemClicked;

public struct MenuId {
  public const string History = "History";
  public const string Close = "Close";
}

public static class SidebarUtils {
  public static void MenuItemClickHandler(SidebarMenuItem menuItem) {
    if (menuItem.Id == MenuId.History) {
      var dialog = new HistoryDialog() {
        Owner = Application.Current.MainWindow,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
      };
      dialog.ShowDialog();
    }
    else if (menuItem.Id.StartsWith("Service_")) {
      var service = ServiceManager.Locate<ImageService>(menuItem.Id[8..]);
      service.OpenSettingsDialog();
    }
  }

  private static ObservableCollection<SidebarMenuItem> CreateServicesMenu() {
    ObservableCollection<SidebarMenuItem> list = [];

    foreach (var service in ServiceManager.GetAll()) {
      var menuItem = new SidebarMenuItem {
        Id = "Service_" + service.GetServiceName(),
        Icon = service.GetMenu().Icon,
        Label = service.GetMenu().Text,
      };

      list.Add(menuItem);
    }

    return list;
  }

  public static ObservableCollection<SidebarMenuItem> GetMenuItems(ItemClicked onItemClick) {
    ObservableCollection<SidebarMenuItem> list = [];

    /*foreach (var menuItem in CreateServicesMenu()) list.Add(menuItem);*/

    list.Add(new() {
      Id = MenuId.History,
      Label = "Notifications",
      Icon = "\ue834"
    });

    list.Add(new() {
      Id = MenuId.Close,
      Label = "Close",
      Icon = "\ue3da"
    });

    foreach (var menuItem in list) {
      menuItem.ItemClick += onItemClick;
    }

    return list;
  }
}