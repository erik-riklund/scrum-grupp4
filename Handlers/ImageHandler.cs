﻿namespace App.Handlers
{
  public class ImageHandler
  {
    public async Task UploadImage(IFormFile file, string path)
    {
      if (file != null && file.Length > 0)
      {
        var newpath = "wwwroot" + path.Substring(2);

        using var fileStream = new FileStream(newpath, FileMode.Create);
        await file.CopyToAsync(fileStream);
      }
    }

    public string GetPath(IFormFile file, string id)
    {
      string filename = file.FileName;
      int index = filename.LastIndexOf(".");
      string format = filename.Substring(index);
      string path = "../Images/" + id + format;
      
      return path;
    }
  }
}
