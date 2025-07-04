using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(AppDbContext db, ILogger<FileUploadController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                // 1. Upload file to VirusTotal
                var analysisId = await FileCheckWithVirusTotalAsync(file);
                if (analysisId == null)
                    return StatusCode(500, "VirusTotal upload failed.");

                // 2. Wait and check scan result
                var isClean = await FileResultAsync(analysisId);
                if (!isClean)
                {
                    _logger.LogWarning("File '{FileName}' was flagged as malicious or suspicious by VirusTotal.", file.FileName);
                    return BadRequest("File is flagged as malicious by VirusTotal.");
                }

                _logger.LogWarning("File '{FileName}' passed VirusTotal scan and is safe to upload.", file.FileName);
                using var content = new MultipartFormDataContent();
                using var stream = file.OpenReadStream();

                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", file.FileName);

                // Send POST to file server (change to your actual URL/IP)
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("http://file_server:8080/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("File '{FileName}' uploaded successfully to file server.", file.FileName);
                    return Ok("File uploaded successfully.");
                }
                else
                {
                    _logger.LogError("File '{FileName}' upload to file server failed. Status code: {StatusCode}", file.FileName, response.StatusCode);
                    return Ok("File upload failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Error uploading to file server.");
            }

        }

        private async Task<string?> FileCheckWithVirusTotalAsync(IFormFile file)
        {
            var apiKey = Environment.GetEnvironmentVariable("VIRUSTOTAL_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("VirusTotal API Key is missing.");
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-apikey", apiKey);

            using var filestream = file.OpenReadStream();
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(filestream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            var response = await httpClient.PostAsync("https://www.virustotal.com/api/v3/files", content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            string? analysisId = doc.RootElement.GetProperty("data").GetProperty("id").GetString();
            _logger.LogInformation("Analysis Id: '{AnalysisId}' uploaded successfully to file server. File: '{FileName}'", analysisId, file.FileName);
            return analysisId;
        }

        private async Task<bool> FileResultAsync(string analysisId)
        {
            var apiKey = Environment.GetEnvironmentVariable("VIRUSTOTAL_API_KEY");
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-apikey", apiKey);

            const int maxRetries = 10;
            for (int i = 0; i < maxRetries; i++)
            {
                var response = await httpClient.GetAsync($"https://www.virustotal.com/api/v3/analyses/{analysisId}");
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                var status = doc.RootElement
                                .GetProperty("data")
                                .GetProperty("attributes")
                                .GetProperty("status")
                                .GetString();

                if (status == "completed")
                {
                    var stats = doc.RootElement
                        .GetProperty("data")
                        .GetProperty("attributes")
                        .GetProperty("stats");

                    int malicious = stats.GetProperty("malicious").GetInt32();
                    int suspicious = stats.GetProperty("suspicious").GetInt32();

                    return malicious == 0 && suspicious == 0;
                }

                await Task.Delay(2000); // wait 2s between polls
            }

            // If result doesn't come in time, treat as unsafe
            return false;
        }
    }
}