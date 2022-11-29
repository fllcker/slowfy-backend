using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using slowfy_backend.Services;

namespace slowfy_backend.Controllers;

public class FileController : Controller
{
    private IUsersService _usersService;

    public FileController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpGet]
    public IActionResult Mp3(string mp3)
    {
        try
        {
            FileStream fs = new FileStream("./static/tracks/" + mp3, FileMode.Open);
            return File(fs, "audio/mpeg", mp3 + "_mp3.mp3");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProfilePicture()
    {
        try
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            if (email == null) throw new Exception("Not authorized");
            string src = await _usersService.GetProfilePicture(email);
            FileStream fs = new FileStream("./static/images/" + src, FileMode.Open);
            return File(fs, "image/png", src + ".png");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> UploadTrack(IFormFile file)
    {
        if (file.Length > 0)
        {
            var targetDirectory = "./static/tracks/";
            var fileName = GetFileName(file);
            var savePath = Path.Combine(targetDirectory, fileName);
            
            await using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            
            return Json(new { Source = fileName });
        }
        return BadRequest(new { Status = "Error" });
    }
    
    [HttpPost]
    public async Task<ActionResult> UploadImage(IFormFile file)
    {
        if (file.Length > 0)
        {
            var targetDirectory = "./static/images/";
            var fileName = GetFileName(file);
            var savePath = Path.Combine(targetDirectory, fileName);
            
            await using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            
            return Json(new { Source = fileName });
        }
        return BadRequest(new { Status = "Error" });
    }
    
    private static string GetFileName(IFormFile file) => file.ContentDisposition.Split(';')
        .Select(x => x.Trim())
        .Where(x => x.StartsWith("filename="))
        .Select(x => x.Substring(9).Trim('"'))
        .First();
}