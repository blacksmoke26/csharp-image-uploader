// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Collections.ObjectModel;
using System.Windows;
using PixPost.Dialogs;
using PixPost.Services;
using PixPost.UserControls;

namespace PixPost.Utils;

using ItemClicked = SidebarMenuItem.ItemClicked;

public struct MenuId {
  public const string Close = "Close";
  public const string Notifications = "Notifications";
}

public static class SidebarUtils {
  public static void MenuItemClickHandler(SidebarMenuItem menuItem) {
    if (menuItem.Id == MenuId.Notifications) {
      var dialog = new HistoryDialog() {
        Owner = Application.Current.MainWindow,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
      };
      dialog.ShowDialog();
    }
    else if (menuItem.Id.StartsWith("Service_")) {
      var service = ServiceManager.Locate(menuItem.Id[8..]);
      service.OpenSettingsDialog();
    }
  }

  private static ObservableCollection<SidebarMenuItem> CreateServicesMenu() {
    ObservableCollection<SidebarMenuItem> list = [];

    foreach (var service in ServiceManager.GetAll()) {
      var menuItem = new SidebarMenuItem {
        Id = "Service_" + service.ServiceName,
        Icon = service.GetMenu().Icon,
        Label = service.GetMenu().Caption,
      };

      list.Add(menuItem);
    }

    return list;
  }

  public static ObservableCollection<SidebarMenuItem> GetMenuItems(ItemClicked onItemClick) {
    ObservableCollection<SidebarMenuItem> list = CreateServicesMenu();

    list.Add(new() {
      Id = MenuId.Notifications,
      Label = "Notifications",
      Icon = "\ue0ce"
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