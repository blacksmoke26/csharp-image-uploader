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
    var templateControl = Schema.Type switch {
      InputFieldType.Toggle => Resources["ControlCheckbox"],
      InputFieldType.List => Resources["ControlComboBox"],
      _ => Resources["ControlText"],
    };

    FieldContentControl.Content = (ContentControl)templateControl!;
  }
}