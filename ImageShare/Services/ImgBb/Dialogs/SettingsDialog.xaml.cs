// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using PixPost.Helpers;
using PixPost.Objects.Service;

namespace PixPost.Services.ImgBb.Dialogs;

public partial class SettingsDialog {
  private static readonly ImageService ServiceInstance = ServiceManager.GetCurrent();

  public SettingsDialog() {
    InitializeComponent();
    ApiKeyTextBox.Focus();

    var service = ServiceInstance.ReadConfig();
    //EndpointTextBox.Text = service.Endpoint;
    //ApiKeyTextBox.Text = service.ApiKey;
  }

  private void CloseTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }

  private void MainWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void WebUrlClick_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Process.Start(new ProcessStartInfo() {
      //FileName = ServiceInstance.DocsUrl,
      UseShellExecute = true
    });
  }

  private void SettingsDialog_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }

  private void UpdateButton_OnClick(object sender, RoutedEventArgs e) {
    if (!Uri.TryCreate(EndpointTextBox.Text, UriKind.Absolute, out _)) {
      DialogHelper.ShowErrorDialog(this, "Endpoint is not a valid URL.", "Validation Error",
        dialog => { dialog.Closed += (_, _) => EndpointTextBox.Focus(); });
      return;
    }

    if (string.IsNullOrWhiteSpace(ApiKeyTextBox.Text)) {
      DialogHelper.ShowErrorDialog(this, "API key should not be empty.", "Validation Error",
        dialog => { dialog.Closed += (_, _) => ApiKeyTextBox.Focus(); });
      return;
    }

    if (ApiKeyTextBox.Text.Length != 32) {
      DialogHelper.ShowErrorDialog(this, "The API key you have entered is invalid.", "Validation Error",
        dialog => { dialog.Closed += (_, _) => ApiKeyTextBox.Focus(); });
      return;
    }

    /*ServiceInstance.SaveConfig(new ServiceConfiguration {
      IsEnabled = true,
      Endpoint = EndpointTextBox.Text,
      ApiKey = ApiKeyTextBox.Text,
    });*/
    Close();
  }

  private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }
}