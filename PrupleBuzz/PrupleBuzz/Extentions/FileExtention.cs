using PrupleBuzz.Models;
using System.IO;

namespace PrupleBuzz.Extentions
{
    public static class FileExtention
    {

        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool IsSizeOk(this IFormFile file,int mb)
        {
            return file.Length / 1024 / 1024 < mb;
        }


        public static string CreateFile(this IFormFile FormFile, string env, string path)
        {
            string imageName = Guid.NewGuid() + FormFile.FileName;

            string Fullpath = Path.Combine(env, path, imageName);

            using (FileStream fileStream = new FileStream(Fullpath, FileMode.Create))
            {
                FormFile.CopyTo(fileStream);
            }
            return imageName;
        }
    }
}
