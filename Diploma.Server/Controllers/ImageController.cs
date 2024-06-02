using Diploma.Entities.Models;
using Diploma.Server.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Diploma.Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IStatisticService _statisticService;
        private readonly IImageService _imageService;

        public ImageController(
            ILogger<ImageController> logger,
            IStatisticService statisticService,
            IImageService imageService)
        {
            _logger = logger;
            _statisticService = statisticService;
            _imageService = imageService;
        }

        [HttpPost("UploadImage")]
        [Consumes("image/jpeg")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await Request.Body.CopyToAsync(memoryStream);
                var bitmap = new Bitmap(memoryStream);

                string text = _imageService.GetTextFromImage(bitmap);

                var res = await _statisticService.CreateStatisticRecord(
                    new CounterSnapshot
                    {
                        Id = Guid.NewGuid(),
                        CurrentValue = double.Parse(text),
                        CreatedAt = DateTime.UtcNow,
                    });

                return res ? NoContent() : StatusCode(418);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
