using BlobTestApp.Models;
using BlobTestApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlobTestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlobStorageService _blobStorageService;

        public HomeController(ILogger<HomeController> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var videoUrls = await _blobStorageService.ListFilesAsync();
            return View(videoUrls);

        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Only video files are allowed.");
            }

            using var stream = file.OpenReadStream();
            var fileName = Guid.NewGuid().ToString() + extension;
            var videoUrl = await _blobStorageService.UploadAsync(stream, fileName);
            return RedirectToAction("Index");
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadVideo(string fileName)
        {
            var stream = await _blobStorageService.DownloadAsync(fileName);
            return File(stream, "application/octet-stream", fileName); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
