using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using PixPost.Helpers;
using static PixPost.Objects.Logging;

namespace PixPost.Dialogs;

public class CountToVisibilityConverter : MarkupExtension, IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    var val = value == null ? 0 : (int)value;
    return val > 0 ? Visibility.Visible : Visibility.Collapsed;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    return null;
  }

  public override object? ProvideValue(IServiceProvider serviceProvider) {
    return this;
  }
}

public class DateTimeToRelativeConverter : MarkupExtension, IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    var val = value == null ? DateTime.Now : (DateTime)value;
    return DateTimeHelper.TimeAgo(val);
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    return null;
  }

  public override object? ProvideValue(IServiceProvider serviceProvider) {
    return this;
  }
}

public partial class HistoryDialog {
  public static readonly DependencyProperty HistoryItemsProperty = DependencyProperty.Register(
    nameof(HistoryItems), typeof(ObservableCollection<LogItem>), typeof(HistoryDialog),
    new PropertyMetadata(new ObservableCollection<LogItem>(), (o, args) => {
      var wdo = (HistoryDialog)o;
      wdo.HistoryListView.ItemsSource = wdo.HistoryItems = (ObservableCollection<LogItem>)args.NewValue;
    }));

  public ObservableCollection<LogItem> HistoryItems {
    get => (ObservableCollection<LogItem>)GetValue(HistoryItemsProperty);
    set => SetValue(HistoryItemsProperty, value);
  }

  public HistoryDialog() {
    InitializeComponent();
    HistoryItems.Add(new LogItem {
      Remarks = "Image has been successfully uploaded.",
      Status = LogItemStatus.Success,
      Heading = "Success",
    });
    HistoryItems.Add(new LogItem {
      Remarks = "Unknown error occurred",
      Status = LogItemStatus.Error,
      Heading = "Failed",
    });
    HistoryItems.Add(new LogItem {
      Remarks = "Application has been launched",
      Status = LogItemStatus.Trace,
      Heading = "Information",
    });
    HistoryListView.ItemsSource = HistoryItems;
    DataContext = this;
  }

  private void BaseWindow_OnKeyDown(object sender, KeyEventArgs e) {
    if (e.Key == Key.Escape) Close();
  }

  private void BaseCloseButton_OnClick(object sender, RoutedEventArgs e) {
    Close();
  }

  private void BaseWindow_OnMouseDown(object _, MouseButtonEventArgs e) {
    if (e is { LeftButton: MouseButtonState.Pressed, ButtonState: MouseButtonState.Pressed }) DragMove();
  }

  private void ClearAllTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e) {
    HistoryItems.Clear();
  }
}