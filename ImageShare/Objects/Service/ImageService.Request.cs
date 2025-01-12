using PixPost.Objects.Service.Interfaces;

namespace PixPost.Objects.Service;

public partial class ImageService : IServiceRequest {
  public Task<TRes?> UploadAsync<TOpt, TRes>(string filename, TOpt? options = default,
    CancellationToken cancellationToken = default) where TOpt : IUploadOptions where TRes : IRequestResponse {
    throw new NotImplementedException();
  }

  public Task<TRes[]> UploadBulkAsync<TOpt, TRes>(string[] files, TOpt? options = default,
    CancellationToken cancellationToken = default) where TOpt : IUploadOptions where TRes : IRequestResponse {
    throw new NotImplementedException();
  }
}