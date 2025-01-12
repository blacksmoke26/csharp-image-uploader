namespace PixPost.Objects;

public abstract class Logging {
  public enum LogItemStatus {
    Trace = 0,
    Error = 3,
    Success = 5,
  }

  public class LogItem {
    public string Id { get; private set; } = string.Empty;
    public string Heading { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public object OldValue { get; set; } = string.Empty;
    public object NewValue { get; set; } = string.Empty;
    public object? Meta { get; set; } = null;
    public LogItemStatus Status { get; set; } = LogItemStatus.Trace;
    public ImageThumb? ImageItem { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
  }
}