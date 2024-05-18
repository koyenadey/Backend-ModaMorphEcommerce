using Microsoft.AspNetCore.Http;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IImageUploadService
{
    Task<List<string>> Upload(IEnumerable<IFormFile> files);
}
