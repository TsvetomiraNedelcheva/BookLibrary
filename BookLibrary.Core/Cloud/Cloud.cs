using System.IO;
using System.Threading.Tasks;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BookLibrary.Core.Cloud
{
    public class Cloud
    {
        public static async Task<string> UploadAsync(Cloudinary cloudinary, IFormFile file)
        {
            var resultUrl = string.Empty;
            byte[] finalImage;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            finalImage = memoryStream.ToArray();

            using var destinationStream = new MemoryStream(finalImage);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, destinationStream),
            };
            var result = await cloudinary.UploadAsync(uploadParams);
            resultUrl = result.Uri.AbsoluteUri;

            return resultUrl;
        }
    }
}
