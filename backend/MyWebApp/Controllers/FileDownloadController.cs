using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileDownloadController : ControllerBase
    {
        private readonly AppDbContext _db;

        public FileDownloadController(AppDbContext db)
        {
            _db = db;
        }
        
        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest("Filename is required");
            }

            try
            {
                var httpClient = new HttpClient();
                var fileServerUrl = $"http://file_server:8080/uploads/{filename}";

                var response = await httpClient.GetAsync(fileServerUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("File not found on the server");
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

                return File(stream, contentType, filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Error retrieving file from file server.");
            }
        }
    }
}