// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Windows;
using System.Windows.Input;
using Dumpify;

namespace PixPost.Objects.Service.Dialogs;

public partial class SettingsDialog {
  public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register(
    nameof(Service), typeof(ImageService), typeof(SettingsDialog), new PropertyMetadata(default(ImageService)));

  public ImageService Service {
    get => (ImageService)GetValue(ServiceProperty);
    set => SetValue(ServiceProperty, value);
  }

  public SettingsDialog(ImageService service) {
    InitializeComponent();
    Service = service;
    DataContext = this;
    InputFieldsContainer.ItemsSource = Service.GetVariablesMap();
  }

  private void SettingsDialog_OnMouseDown(object sender, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void SettingsDialog_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }

  private void UpdateButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }

  private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }

  private void Dialog_CloseClick(object sender, RoutedEventArgs e) {
    Close();
  }
}