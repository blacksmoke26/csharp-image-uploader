using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImageShare.Helpers;

namespace ImageShare.UserControls;

public partial class ImageGridView : UserControl {
  public static readonly DependencyProperty ImagesListProperty = DependencyProperty.Register(
    nameof(ImagesList), typeof(List<ImageThumb>), typeof(ImageGridView),
    new PropertyMetadata(null, ImagesListChangedCallback));

  private static void ImagesListChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    if (d is not ImageGridView control || e.NewValue == null) return;
    
    var list = (List<ImageThumb>)e.NewValue;
    control.ThumbsListView.ItemsSource = list;
    Console.WriteLine(list.Count);
  }

  public List<ImageThumb> ImagesList {
    get => (List<ImageThumb>)GetValue(ImagesListProperty);
    set => SetValue(ImagesListProperty, value);
  }

  public ImageGridView() {
    InitializeComponent();
    ThumbsListView.ItemsSource = ImagesList;
  }

  private void ImageThumbControl_OnRemoveClick(object sender, RoutedEventArgs e) {
    Console.WriteLine("____CLICKED____");
  }

  private void ImageEdit_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Console.WriteLine("____CLICKED____");
  }

  private void ImageRemove_OnMouseDown(object sender, MouseButtonEventArgs e) {
    Console.WriteLine("____CLICKED____");
  }
}