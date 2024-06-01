using Diploma.Server.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Diploma.Server.Business.Services
{
    public class ImageService : IImageService
    {
        public string GetTextFromImage(Bitmap image)
        {
            using var engine = new TesseractEngine($"TrainedData{Path.PathSeparator}eng.traineddata", "eng");
            string text = engine.Process(image).GetText();
            return text;
        }
    }
}
