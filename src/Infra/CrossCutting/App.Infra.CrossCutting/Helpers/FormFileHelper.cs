using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Infra.CrossCutting.Helpers
{
    public static class FormFileHelper
    {
        public static async Task<string> ConvertToBase64Async(IFormFile formFile, string empty = "")
        {
            if (formFile == null || formFile.Length == 0)
                return empty;

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();

                return $"data:{formFile.ContentType};base64," + Convert.ToBase64String(bytes);
            }
        }
    }
}
