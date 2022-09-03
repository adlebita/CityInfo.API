using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class Files : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;

    public Files(FileExtensionContentTypeProvider contentTypeProvider) => _contentTypeProvider = contentTypeProvider;

    [HttpGet("{index:int}")]
    public ActionResult GetFile(int index)
    {
        const string pathToFile = "Final Report.pdf";
        
        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }

        _contentTypeProvider.TryGetContentType(pathToFile, out var contentType);
        if (contentType is null)
        {
            contentType = "application/octet-stream";
        }
        
        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}