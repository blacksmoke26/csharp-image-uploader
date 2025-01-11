using System.Data;
using static System.Text.RegularExpressions.Regex;

namespace PixPost.Helpers;

public static class SizeHelper {
  /// <summary>
  /// Convert expiry timespan into seconds
  /// </summary>
  /// <param name="expiry">Timespan</param>
  /// <returns>The seconds / Null when failed to parse</returns>
  public static long? ExpiryToSeconds(string expiry) {
    if (expiry == "None") return null;

    var value = long.Parse(Replace(expiry, @"[^\d]+", string.Empty));
    var unit = Replace(expiry, "[0-9]+", string.Empty);

    return unit switch {
      "TM" => (long)TimeSpan.FromMinutes(value).TotalSeconds,
      "TH" => (long)TimeSpan.FromHours(value).TotalSeconds,
      "ED" => (long)TimeSpan.FromDays(value).TotalSeconds,
      "EW" => (long)TimeSpan.FromDays(value * 7).TotalSeconds,
      "EM" => (long)TimeSpan.FromDays(value * 30).TotalSeconds,
      _ => throw new InvalidExpressionException($"The value '{expiry}' is not a valid")
    };
  }

  /// <summary>
  /// Convert bytes into megabytes
  /// </summary>
  /// <param name="bytesSize">Size in bytes</param>
  /// <returns>The computed value</returns>
  public static double SizeToMb(double bytesSize) {
    return bytesSize > 0 ? Math.Round(bytesSize / 1024 / 1024) : 0;
  }
}