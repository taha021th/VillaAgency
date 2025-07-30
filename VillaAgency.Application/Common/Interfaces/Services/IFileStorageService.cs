using Microsoft.AspNetCore.Http;

namespace VillaAgency.Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {

        //Save one file
        Task<string> SaveFileAsync(IFormFile file, string directoryPath);

        //Save multi file
        Task<List<string>> SaveFilesAsync(IEnumerable<IFormFile> files, string directoryPath);
        Task DeleteFileAsync(string fileName, string directoryPath);
    }
}
