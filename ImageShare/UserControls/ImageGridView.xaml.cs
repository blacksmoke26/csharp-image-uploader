using System.Windows;
using System.Windows.Controls;

namespace ImageShare.UserControls;

public partial class ImageGridView : UserControl {
  public static readonly DependencyProperty ImagesListProperty = DependencyProperty.Register(
    nameof(ImagesList), typeof(List<string>), typeof(ImageGridView), new PropertyMetadata(default(List<string>)));

  public List<string> ImagesList {
    get => (List<string>)GetValue(ImagesListProperty);
    set => SetValue(ImagesListProperty, value);
  }

  public ImageGridView() {
    InitializeComponent();
  }
}