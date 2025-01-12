namespace PixPost.Objects.Service.Interfaces;

public interface IServiceRequest {
  public Task<TRes?> UploadAsync<TOpt, TRes>(string filename,
    TOpt? options = default, CancellationToken cancellationToken = default
  ) where TOpt : IUploadOptions
    where TRes : IRequestResponse;

  public Task<TRes[]> UploadBulkAsync<TOpt, TRes>(string[] files,
    TOpt? options = default, CancellationToken cancellationToken = default
  ) where TOpt : IUploadOptions
    where TRes : IRequestResponse;
}