using PixPost.Helpers;

namespace PixPost.Objects;

public class Logging {
  public enum LogItemStatus {
    Unknown = 0,
    Failed = 3,
    Success = 5,
  }

  public class LogItem {
    public string Uuid { get; private set; } = string.Empty;
    public ImageThumb? ImageItem { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public string Remarks { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public LogItemStatus Status { get; set; } = LogItemStatus.Unknown;
  }
}