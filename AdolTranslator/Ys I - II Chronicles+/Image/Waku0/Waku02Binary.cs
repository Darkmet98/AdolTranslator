using System;
using System.Drawing;
using System.IO;

namespace AdolTranslator.Image.Waku0
{
    class Waku02Binary
    {
        private Bitmap bitmap;
        public Waku02Binary(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            bitmap = new Bitmap(filePath);
        }

        public byte[] Convert()
        {
            return Unswizzling.Unswizzle(ConvertBitmap(), true);
        }

        private byte[] ConvertBitmap()
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData =
                bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            return rgbValues;
        }

    }
}
