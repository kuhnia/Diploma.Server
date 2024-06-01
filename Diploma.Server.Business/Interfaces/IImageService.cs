using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Server.Business.Interfaces
{
    public interface IImageService
    {
        string GetTextFromImage(Bitmap image);
    }
}
