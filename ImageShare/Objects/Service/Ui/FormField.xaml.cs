using System.Windows;
using Dumpify;
using PixPost.Objects.Service.Objects;

namespace PixPost.Objects.Service.Ui;

using SchemaSpecs = SchemaSpecs;

public partial class FormField {
  public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
    nameof(ItemSource), typeof(SchemaSpecs.Variable), typeof(FormField),
    new PropertyMetadata(default(SchemaSpecs.Variable), (o, args) => {
      ((o as FormField)!).ItemSource = (SchemaSpecs.Variable)args.NewValue;
    }));

  public SchemaSpecs.Variable ItemSource {
    get => (SchemaSpecs.Variable)GetValue(ItemSourceProperty);
    set => SetValue(ItemSourceProperty, value);
  }

  public FormField() {
    InitializeComponent();
  }
}