using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Services
{
    public class RecipeMediaService
    {
        private readonly string _mediaRoot;

        public RecipeMediaService()
        {
            _mediaRoot = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "CookPlan",
                "RecipeImages");

            Directory.CreateDirectory(_mediaRoot);
        }

        public string? SaveRecipePhoto(string? sourcePath)
        {
            return SaveImage(sourcePath, "recipes");
        }

        public string? SaveStepPhoto(string? sourcePath)
        {
            return SaveImage(sourcePath, "steps");
        }

        private string? SaveImage(string? sourcePath, string folder)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                return null;

            if (!File.Exists(sourcePath))
                return sourcePath;

            var targetFolder = Path.Combine(_mediaRoot, folder);
            Directory.CreateDirectory(targetFolder);

            var extension = Path.GetExtension(sourcePath);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var targetPath = Path.Combine(targetFolder, fileName);

            File.Copy(sourcePath, targetPath, overwrite: true);

            return targetPath;
        }
    }
}
