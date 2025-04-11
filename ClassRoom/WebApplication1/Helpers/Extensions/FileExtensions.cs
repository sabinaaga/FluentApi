namespace WebApplication1.Helpers.Extensions
{
    public static class  FileExtensions
    {
        public static string GenerateFilePath(this IWebHostEnvironment env, string folder, string fileName)
        {
            return Path.Combine(env.WebRootPath, folder, fileName);
        }
    }
}
