using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.Utilities.Helpers
{
    public static class FileValidator
    {
        public static bool ValidateTypeSize(this IFormFile file, int? maxMb = null, params string[] type)
        {
            bool isValidType = type.Length == 0 || type.Contains(file.ContentType);
            bool isValidSize = true;

            if (maxMb != null)
            {
                isValidSize = file.Length <= maxMb * 1024 * 1024;
            }

            return isValidType && isValidSize;
        }

        public static bool ValidateSize(this IFormFile file, int maxMb)
        {
            return file.Length <= maxMb * 1024 * 1024;
        }

       

    }
}
