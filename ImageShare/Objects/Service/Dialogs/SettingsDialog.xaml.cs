// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Dumpify;
using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service.Dialogs;

public partial class SettingsDialog {
  public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register(
    nameof(Service), typeof(ImageService), typeof(SettingsDialog), new PropertyMetadata(default(ImageService)));

  public ImageService Service {
    get => (ImageService)GetValue(ServiceProperty);
    set => SetValue(ServiceProperty, value);
  }

  public ObservableCollection<FormFieldInfo> VariableFormFields { get; set; } = [];

  private void InitialVariableFormFields() {
    if (VariableFormFields.Count > 0) {
      return;
    }

    foreach (var variable in Service.GetVariablesMap()) {
      FormFieldInfo field = new() {
        Id = variable.Key,
      };
      field.FromSchemaVariable(variable);
      VariableFormFields.Add(field);
    }
  }

  public SettingsDialog(ImageService service) {
    InitializeComponent();
    Service = service;
    DataContext = this;
    InitialVariableFormFields();
  }

  private void SettingsDialog_OnMouseDown(object sender, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void SettingsDialog_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }

  private void UpdateButton_OnClick(object sender, RoutedEventArgs e) {
    VariableFormFields[1].ErrorMessage = VariableFormFields[1].ErrorMessage == null ? "This is an error message" : null;
    VariableFormFields[1].Dump();
    //Close();
  }

  private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }

  private void CloseIcon_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Close();
  }
}