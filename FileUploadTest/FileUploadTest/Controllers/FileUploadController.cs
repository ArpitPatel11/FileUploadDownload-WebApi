using FileUploadTest.Data;
using FileUploadTest.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace FileUploadTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FirstDBContext _dBContext;

        public FileUploadController(IHostingEnvironment hostingEnvironment, FirstDBContext dBContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _dBContext = dBContext;
        }
        [HttpPost]
        public ActionResult<string> UploadFile(List<IFormFile> forms)

        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file.FileName);
                        var newfilename = "File" + DateTime.Now.TimeOfDay.Milliseconds + fi.Extension;
                        var path = Path.Combine(_hostingEnvironment.ApplicationName + "Image" + newfilename);
                        //var path = Path.Combine("_", _hostingEnvironment.ContentRootPath + "Image" + newfilename);
                        //var path = Path.Combine(_hostingEnvironment.ApplicationName  +newfilename+ "FileUploadTest"+ "Image");
                        //var path = Path.Combine(_dBContext.Tblfileuploads + "Image");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        Tblfileupload fileupload = new Tblfileupload();
                        fileupload.FilePath = path;
                        fileupload.FileName = newfilename.ToString();
                        _dBContext.Tblfileuploads.Add(fileupload);
                        _dBContext.SaveChanges();

                    }
                    return Ok("File Successfully Uploaded");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpGet]
        public ActionResult<List<Tblfileupload>> GetFileUpload()
        {
            var result = _dBContext.Tblfileuploads.ToList();
            return result;
        }

        [HttpGet("ById")]
        public ActionResult<List<Tblfileupload>> GetByID(int id)
        {
            //var result = _dBContext.Tblfileuploads.ToList();
            //return result;

            if (_dBContext.Tblfileuploads == null)
            {
                return NotFound("Please Enter id");
            }
            var tblfileuploads = _dBContext.Tblfileuploads.FindAsync(id);

            if (tblfileuploads == null)
            {
                return NotFound();
            }

            return Ok(tblfileuploads);

        }



        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(_hostingEnvironment.ApplicationName + "Image" + filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contentType, Path.GetFileName(filename));
        }
    }
}


//[HttpPost]
//public async Task<ActionResult<string>> upload(IFormFile file)
//{
//    string filename = "";
//    try
//    {
//        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
//        filename = DateTime.Now.Ticks + extension;
//        var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Image", filename);
//        if (!Directory.Exists(pathBuilt))
//        {
//            Directory.CreateDirectory(pathBuilt);
//        }
//        var path = Path.Combine(Directory.GetCurrentDirectory(), "Image", filename);
//        using (var stream = new FileStream(path, FileMode.Create))
//        {
//            await file.CopyToAsync(stream);
//        }
//        Tblfileupload fileupload = new Tblfileupload();
//        fileupload.FilePath = pathBuilt;
//        _dBContext.Tblfileuploads.Add(fileupload);
//        _dBContext.SaveChanges();

//    }
//    catch (Exception e)
//    {
//    }
//    return filename;


//[HttpPost]
//[Route("Download")]
//public async Task<IActionResult> Download(int id)
//{
//    var provider = new FileExtensionContentTypeProvider();
//    var document = await _dBContext.ImageUploads.FindAsync(id);

//    if (document == null)
//        return NotFound();
//    var file = Path.Combine(_hostingEnvironment.ContentRootPath, "Image", document.filename);

//    string contentType;
//    if (!provider.TryGetContentType(file, out contentType))
//    {
//        contentType = "application/octet-stream";
//    }
//    byte[] fileBytes;
//    if (System.IO.File.Exists(file))
//    {
//        fileBytes = System.IO.File.ReadAllBytes(file);
//    }
//    else
//    {
//        return NotFound();
//    }
//    return File(fileBytes, contentType, document.filename);

//}

