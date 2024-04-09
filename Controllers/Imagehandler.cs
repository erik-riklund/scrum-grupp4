using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class Imagehandler
    {
        public async Task UploadImage(IFormFile file, string path)
        {
            if (file != null && file.Length > 0)
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
        }

        public string GetPath(IFormFile file, string id)
        {
            string filename = file.FileName;
            int index = filename.LastIndexOf(".");
            string format = filename.Substring(index);
            string path = "wwwroot/Images/" + id + format;
            return path;
        }
    }
}
