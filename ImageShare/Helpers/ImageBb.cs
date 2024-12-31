namespace ImageShare.Helpers;

public class ImageBb {
  private const string API_KEY = "d5c3b51bde505c7c0de168937f998f6d";
  private const string UPLOAD_ENDPOINT = "https://api.imgbb.com/1/upload";

  public readonly Dictionary<string, string> Expirations = new() {
    { "", "Don't autodelete" },
    { "PT5M", "After 5 minutes" },
    { "PT15M", "After 15 minutes" },
    { "PT30M", "After 30 minutes" },
    { "PT1H", "After 1 hour" },
    { "PT3H", "After 3 hours" },
    { "PT6H", "After 6 hours" },
    { "PT12H", "After 12 hours" },
    { "P1D", "After 1 day" },
    { "P2D", "After 2 days" },
    { "P3D", "After 3 days" },
    { "P4D", "After 4 days" },
    { "P5D", "After 5 days" },
    { "P6D", "After 6 days" },
    { "P1W", "After 1 week" },
    { "P2W", "After 2 weeks" },
    { "P3W", "After 3 weeks" },
    { "P1M", "After 1 month" },
    { "P2M", "After 2 months" },
    { "P3M", "After 3 months" },
    { "P4M", "After 4 months" },
    { "P5M", "After 5 months" },
    { "P6M", "After 6 months" },
  };
}