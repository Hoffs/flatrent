﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlatRent.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger _logger;

        public ImageController(IImageRepository imageRepository, ILogger logger)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id, int width = 0, int height = 0)
        {
            _logger.Debug("Requesting image with {Id} and {Width} {Height}", id, width, height);
            var bytes = await _imageRepository.GetImageAsync(id).ConfigureAwait(false);
            if (bytes == null)
            {
                bytes = await System.IO.File.ReadAllBytesAsync(Path.Join("Files", "placeholder.png")).ConfigureAwait(false);
//                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var image = Image.Load(bytes);
            if (width != 0 & height != 0)
            {
                image.Mutate(x => x.Resize(width, height));
            }

            var ms = new MemoryStream();
            image.SaveAsPng(ms);
//                return new HttpResponseMessage(HttpStatusCode.OK)
//                {
//                    Content = new ByteArrayContent(ms.ToArray())
//                    {
//                        Headers =
//                        {
//                            ContentType = new MediaTypeHeaderValue("image/png")
//                        }
//                    }
//                };
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "image/png");
        }
    }
}