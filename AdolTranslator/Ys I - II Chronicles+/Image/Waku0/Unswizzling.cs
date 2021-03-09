using System;

namespace AdolTranslator.Image.Waku0
{
    class Unswizzling
    {
        // Thanks to kaplas and Twn
        public static byte[] Unswizzle(byte[] array, bool swizzle, int width = 0x100, int height = 0x100)
        {
            var output = new byte[array.Length];
            var bpp = 32 / 8;
            var pitch = width * bpp;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int originalIndex = (y * pitch) + (x * bpp);

                    int swizzledY = (8 * (y / 8)) + (x / 32);
                    int swizzledX = (((32 * (x / 4)) + (x % 4)) % 256) + (4 * (y % 8));
                    int swizzledIndex = (swizzledY * pitch) + (swizzledX * bpp);

                    if (swizzle)
                    {
                        // Source (unswizzled) is BGRA. Dest (swizzled) is RGBA.
                        output[swizzledIndex] = array[originalIndex + 2];
                        output[swizzledIndex + 1] = array[originalIndex + 1];
                        output[swizzledIndex + 2] = array[originalIndex];
                        output[swizzledIndex + 3] = array[originalIndex + 3];
                    }
                    else
                    {
                        // Source (swizzled) is RGBA. Dest (unswizzled) is BGRA.
                        output[originalIndex] = array[swizzledIndex + 2];
                        output[originalIndex + 1] = array[swizzledIndex + 1];
                        output[originalIndex + 2] = array[swizzledIndex];
                        output[originalIndex + 3] = array[swizzledIndex + 3];
                    }
                }
            }

            return output;
        }
    }
}
