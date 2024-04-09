namespace App.Controllers
{
  public class Imagehandler
  {
    public async Task UpploadImage(IFormFile file)
    {
      if (file != null && file.Length > 0)
      {
        var filename = Path.GetFileName(file.FileName);
        var path = "wwwroot/Images/" + filename;
        using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);
      }
    }
  }

}
