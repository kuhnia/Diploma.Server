using Diploma.Server.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Diploma.Server.Business.Services
{
    public class ImageService : IImageService
    {
        public string GetTextFromImage(Bitmap image)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(executableLocation, "tessdata");
            if (!Directory.Exists(path))
                return null;

            using var engine = new TesseractEngine(path, "eng");
            string text = engine.Process(image).GetText();
            return text;
        }
    }
}
