using System.Windows;
using System.Windows.Controls;
using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service.Ui;

using SchemaSpecs = SchemaSpecs;

public partial class FormField {
  public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
    nameof(ItemSource), typeof(SchemaSpecs.Variable), typeof(FormField),
    new PropertyMetadata(null, (o, args) => {
      var obj = (FormField)o;
      obj.ItemSource = (SchemaSpecs.Variable)args.NewValue;
      obj.InitializeInputs();
    }));

  public SchemaSpecs.Variable ItemSource {
    get => (SchemaSpecs.Variable)GetValue(ItemSourceProperty);
    set => SetValue(ItemSourceProperty, value);
  }

  public FormField() {
    InitializeComponent();
  }

  public void InitializeInputs() {
    var container =  MainGrid.Children;
    var templateControl = ItemSource.InputField.Type switch {
      SchemaSpecs.InputFieldType.Toggle => Resources["ControlCheckbox"],
      SchemaSpecs.InputFieldType.List => Resources["ControlComboBox"],
      _ => Resources["ControlText"],
    };

    var control = (ContentControl)templateControl!;
    container.Add(control);
  }
}