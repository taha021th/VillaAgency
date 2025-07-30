using Microsoft.AspNetCore.Http;

namespace VillaAgency.Application.Common.Interfaces.Services
{
    public interface IVideoStorageService
    {
        Task<string> SaveVideoAsync(IFormFile file, string directoryPath);
        Task<List<string>> SaveVideosAsync(IEnumerable<IFormFile> files, string directoryPath);
        Task DeleteVideoAsync(string fileName, string directoryPath);
    }
}
