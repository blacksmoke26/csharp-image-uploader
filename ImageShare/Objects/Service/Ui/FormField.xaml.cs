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
    control.PreviewTextInput += (sender, e) => {
      /*
      var isNum = e.Text == "." || int.TryParse(ctrl.Text, out _) || decimal.TryParse(ctrl.Text, out _);
        Console.WriteLine(isNum);
        e.Handled = r.IsMatch(ctrl.Text);//!isNum || !decimal.TryParse(e.Text, out var _));*/
      //e.Handled = !e.Text.All(cc => Char.IsNumber(cc) || cc == '.' || cc == '-');
      //ctrl.Text = Regex.Replace(ctrl.Text,  @"[^\d\.]+", "");
      //base.OnPreviewTextInput(e);
    };
    /*control.PreviewKeyDown += (sender, e) => {
      var ctrl = (TextBox)sender;
      ctrl.Text = e.Key switch {
        Key.Up when double.TryParse(ctrl.Text, out var uv) => $"{uv + 1}",
        Key.Down when double.TryParse(ctrl.Text, out var dv) => $"{dv - 1}",
        _ => ctrl.Text
      };
    };
    control.MouseWheel += (sender, e) => {
      var ctrl = (TextBox)sender;
      ctrl.Text = e.Delta switch {
        > 0 => $"{double.Parse(ctrl.Text) + 1}",
        < 0 => $"{double.Parse(ctrl.Text) - 1}",
        _ => ctrl.Text
      };
    };*/
    /*DataObject.AddPastingHandler(control, (sender, e) => {
      if (e.DataObject.GetDataPresent(typeof(string))) {
        var text = (string)e.DataObject.GetData(typeof(string))!;
        if (!double.TryParse(text, out _)) e.CancelCommand();
      }
      else e.CancelCommand();
    });*/
  }

  private void InitUrlControl(TextBox control) {
  }

  private void InitToggleControl(CheckBox control) {
  }

  private void InitIntegerControl(TextBox control) {
  }

  private void InitListControl(ComboBox control) {
  }
}