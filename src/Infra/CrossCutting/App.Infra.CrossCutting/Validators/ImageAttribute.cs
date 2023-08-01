using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using SixLabors.ImageSharp;
using System.IO;
using System.Linq;

namespace App.Infra.CrossCutting.Validators
{
    public class ImageAttribute: ValidationAttribute
    {

        public int Width { get; set; }
        public int Height { get; set; }
        public string[] Allowed { get; set; }

        private readonly string[] MIME_TYPES = new[] {
            "image/gif",
            "image/jpeg",
            "image/png", 
            "image/svg+xml",
            "image/tiff",
            "image/x-icon",
            "image/bmp",
            "image/webp",
            "image/avif",
            "image/apng"
        };

        public ImageAttribute(string[] allowed)
        {            
            Allowed = allowed;
        }

        public ImageAttribute(int width, int height, string[] allowed)
        {
            Width = width;
            Height = height;
            Allowed = allowed;            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if(value is IFormFile)
                {
                    var file = value as IFormFile;                    
                    using(var stream = file.OpenReadStream())
                    {
                        using (var image = Image.Load(stream))
                        {
                            //Reset position stream, it's is necessary!
                            stream.Seek(0, SeekOrigin.Begin);

                            var format = Image.DetectFormat(stream);

                            if(Width != default || Height != default)
                                if (image.Width != Width && image.Height != Height)
                                    throw new ValidationException($"Image size invalid, try again with size {Width}x{Height}");

                            if (!MIME_TYPES.Any(x => x == format.DefaultMimeType))
                                throw new ValidationException("Image mime type invalid.");

                            if (!IsValidFormat(file) || !Allowed.Any(x => x.ToLower() == format.Name.ToLower()))
                                throw new ValidationException($"Image format invalid, try again with formats [{string.Join(",", Allowed)}]");

                        }
                    }                   
                }

                return ValidationResult.Success;
            }
            catch (ValidationException e)
            {
                return new ValidationResult(e.Message);
            }
            catch (Exception e)
            {
                return new ValidationResult("Try again, invalid image.");
            }
        }

        protected virtual bool IsValidFormat(IFormFile formFile)
        {
            try
            {
                var ext = Path.GetExtension(formFile.FileName);
                return Allowed.Any(x => $".{x.ToLower()}" == ext.ToLower());
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
