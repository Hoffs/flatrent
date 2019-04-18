using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Repositories;
using FlatRent.Repositories.Interfaces;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : AuthoredBaseEntityController<Entities.Image>
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger _logger;

        public ImageController(IImageRepository imageRepository, ILogger logger) : base(imageRepository)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [EntityMustExist]
        public async Task<IActionResult> GetImage([FromRoute] Guid id, int width = 0, int height = 0)
        {
            _logger.Debug("Requesting image with {Id} and {Width} {Height}", id, width, height);

            var image = await _imageRepository.GetAsync(id).ConfigureAwait(false);
            if (!image.IsUploaded) return NotFound(new FormError("Id", Errors.NotFound));

            var ms = new MemoryStream();
            var contentType = image.MimeType;
            if (width != 0 & height != 0)
            {
                var loadedImage = Image.Load(image.Bytes);
                loadedImage.Mutate(x => x.Resize(width, height));
                loadedImage.SaveAsJpeg(ms, new JpegEncoder { Quality = 70 });
                contentType = "image/jpeg";
            }
            else
            {
                await ms.WriteAsync(image.Bytes).ConfigureAwait(false);
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, contentType, image.Name);
        }

        [HttpPut("{id}")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> UploadImage([FromRoute] Guid id, IFormFile image)
        {
            _logger.Debug("Uploading image with {Id}", id);
            using (var stream = image.OpenReadStream())
            {
                if (stream.Length > 5 * MemorySize.Megabyte)
                    return BadRequest(new FormError("File", Errors.FileTooBig));

                var savedImage = await _imageRepository.GetAsync(id).ConfigureAwait(false);
                if (savedImage.Bytes?.Length > 0)
                    return BadRequest(new FormError("Id", Errors.ImageAlreadyUploaded));

                if (!image.IsImage())
                    return BadRequest(new FormError("File", Errors.InvalidImage));

                if (stream.Length > 1_750_000)
                {
    //                using (var img = new MagickImage(stream))
    //                {
    //                    using (var outMs = new MemoryStream())
    //                    {
    //                        img.Quality = 50;
    //                        img.Format = MagickFormat.WebP;
    //                        img.Write(outMs);
    //                        savedImage.MimeType = "image/webp";
    //                        outMs.Seek(0, SeekOrigin.Begin);
    //                        savedImage.Bytes = new byte[outMs.Length];
    //                        await outMs.ReadAsync(savedImage.Bytes).ConfigureAwait(false);
    //                    }
    //                }

                    var loadedImage = Image.Load(stream);
                    using (var ms = new MemoryStream())
                    {
                        loadedImage.SaveAsJpeg(ms, new JpegEncoder { Quality = 60, Subsample = JpegSubsample.Ratio420 });
                        savedImage.MimeType = "image/jpeg";
                        savedImage.Bytes = await ms.GetByteArrayAsync();
                    }
                }
                else
                {
                    savedImage.Bytes = new byte[stream.Length];
                    savedImage.MimeType = image.ContentType;
                    await stream.ReadAsync(savedImage.Bytes);
                }

                var errors = await _imageRepository.UpdateAsync(savedImage);
                if (errors != null) return BadRequest(errors);
                return Ok();
            }
        }
    }
}