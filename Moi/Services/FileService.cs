using GaragesStructure.DATA.DTOs.File;

namespace GaragesStructure.Services
{
    
    public interface IFileService {
        Task<(string? file , string? error)> Upload(FileForm fileForm);
        Task<(List<string>? files , string? error)> Upload(MultiFileForm filesForm);
    }
    
    public class FileService  : IFileService
    {

        public async Task<(string? file , string? error)> Upload(FileForm fileForm
        ) {
            var id = Guid.NewGuid();
            var extension = Path.GetExtension(fileForm.File.FileName);
            var fileName = $"{id}{extension}";

            var attachmentsDir = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "Attachments");
            if (!File.Exists(attachmentsDir)) Directory.CreateDirectory(attachmentsDir);


            var path = Path.Combine(attachmentsDir, fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await fileForm.File.CopyToAsync(stream);
            var filePath = Path.Combine("Attachments", fileName);
            return (filePath , null);
        }
        public async Task<(List<string> files , string? error)> Upload(MultiFileForm filesForm)
        {
            var fileList = new List<string>();
            foreach (var file in filesForm.Files)
            { 
                var fileToAdd = await Upload(new FileForm(){File = file});
                fileList.Add(fileToAdd.file!);
            }
            return (fileList , null);
        }
    }
}