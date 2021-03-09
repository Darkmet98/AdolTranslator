using System;
using System.Collections.Generic;

namespace AdolTranslator.Compression
{
    class DatDecompression
    {
        // Thanks to twn for explaining how decompression works.
        byte cVar1;
        uint bVar2;
        int addr_pixel;
        int pcVar3;
        int iVar4;
        uint uVar5;
        int pcVar6;
        ushort short_read;
        List<byte> result = new List<byte>();
        public byte[] Decompression(int size, byte[] array, int fileLength)
        {
            result.Clear();
            cVar1 = array[6];

            if (size < 5)
            {
                return array;
            }

            iVar4 = size - 5;
            pcVar6 = 5;
            do
            {
                do
                {
                    while (true)
                    {
                        while (true)
                        {
                            while (true)
                            {
                                if (iVar4 == 0)
                                {
                                    return result.ToArray();
                                }

                                if (array[pcVar6] == cVar1)
                                    break;
                                result.Add(array[pcVar6]);
                                addr_pixel++;
                                pcVar6 += 1;
                                iVar4 += -1;

                            }

                            bVar2 = array[pcVar6 + 1];

                            if (4 < bVar2) break;
                            if (bVar2 != 0)
                            {
                                return result.ToArray();
                            }

                            result.Add(cVar1);
                            addr_pixel++;
                            pcVar6 += 2;
                            iVar4 += -2;
                        }

                        uVar5 = bVar2;

                        short_read = BitConverter.ToUInt16(array, pcVar6 + 2);
                        pcVar3 = addr_pixel + (-1 - short_read);

                        pcVar6 += 4;
                        iVar4 += -4;
                        if (pcVar6 > fileLength)
                            return result.ToArray();

                        if (pcVar3 < 0)
                            break;

                        DecompressionRoutine();
                    }
                } while (bVar2 == 0);



                do
                {

                    result.Add(0);
                    addr_pixel++;
                    pcVar3 += 1;
                    uVar5 -= 1;

                    if (0 <= pcVar3)
                        DecompressionRoutine();

                } while (uVar5 != 0);
            } while (true);
        }

        private void DecompressionRoutine()
        {
            while (uVar5 != 0)
            {

                result.Add(result[pcVar3]);
                addr_pixel++;
                pcVar3 += 1;
                uVar5 -= 1;
            }
        }
    }
}
