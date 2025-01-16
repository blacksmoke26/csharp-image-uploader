// Licensed to the end users under one or more agreements.
// Copyright (c) 2024-2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Windows;
using System.Windows.Controls;
using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service.Ui;

public partial class FormField {
  public static readonly DependencyProperty SchemaProperty = DependencyProperty.Register(
    nameof(Schema), typeof(FormFieldInfo), typeof(FormField),
    new PropertyMetadata(null, (o, args) => {
      var obj = (FormField)o;
      obj.Schema = (FormFieldInfo)args.NewValue;
      obj.DataContext = args.NewValue;
      obj.InitializeInputs();
    }));

  public FormFieldInfo Schema {
    get => (FormFieldInfo)GetValue(SchemaProperty);
    set => SetValue(SchemaProperty, value);
  }

  public FormField() {
    InitializeComponent();
  }

  public void InitializeInputs() {
    if (Schema.Type switch {
          InputFieldType.Toggle => Resources["ControlToggle"]!,
          InputFieldType.List => Resources["ControlComboBox"]!,
          _ => Resources["ControlText"]!,
        } is not ContentControl templateControl) {
      return;
    }

    foreach (var child in LogicalTreeHelper.GetChildren(templateControl)) {
      if (Schema.Type == InputFieldType.Double)
        InitDoubleControl((TextBox)child);
      else if (Schema.Type == InputFieldType.Url)
        InitUrlControl((TextBox)child);
      else if (Schema.Type == InputFieldType.Toggle)
        InitToggleControl((CheckBox)child);
      else if (Schema.Type == InputFieldType.Integer)
        InitIntegerControl((TextBox)child);
      else if (Schema.Type == InputFieldType.List)
        InitListControl((ComboBox)child);
      else
        InitTextControl((TextBox)child);
    }

    FieldContentControl.Content = templateControl;
  }

  private void InitTextControl(TextBox control) {
    if (Schema.MaxLength != null)
      control.MaxLength = (int)Schema.MaxLength;
    if (Schema.ExactLength != null)
      control.MaxLength = (int)Schema.ExactLength;
  }

  private void InitDoubleControl(TextBox control) {
    control.MinWidth = 200;
    control.MaxWidth = 200;
    control.HorizontalAlignment = HorizontalAlignment.Left;
  }

  private void InitIntegerControl(TextBox control) {
    control.MinWidth = 200;
    control.MaxWidth = 200;
    control.HorizontalAlignment = HorizontalAlignment.Left;
  }

  private void InitUrlControl(TextBox control) {
    // Implement logic here
  }

  private void InitToggleControl(CheckBox control) {
    // Implement logic here
  }

  private void InitListControl(ComboBox control) {
    // Implement logic here
  }
}