using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GaleriUygulamasi.Utilities
{
    public static class UtilityManager
    {
        static string[] dosyaTipleri = { "excel", "sheet", "word", "pdf", "powerpoint", "compressed", "octet-stream", "image", "text", "audio", "video" };
        static string[] dosyaIconlar = {"fa fa-file-excel-o", "fa fa-file-excel-o","fa fa-file-word-o","fa fa-file-pdf-o","fa fa-file-powerpoint-o","fa fa-file-archive-o",
                                        "fa fa-file-code","fa fa-image","fa fa-file-text","fa fa-music","fa fa-play" };
        static string[] dosyaClasses = { "bgExcel", "bgExcel", "bgWord", "bgPdf","bgPowerpoint", "bgCompressed", "bgOctetStream", "bgImage", "bgText", "bgAudio", "bgVideo" };

        public static byte[] ByteBirlestir(byte[] arrayA, byte[] arrayB)
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }

        public static string SetIcon(string contentType)
        {
            for (int i = 0; i < dosyaTipleri.Length; i++)
            {
                if (contentType.Contains(dosyaTipleri[i]))
                {
                    return dosyaIconlar[i];
                }
            }
            return "fa fa-file-o";
        }

        public static string SetClass(string contentType)
        {
            for (int i = 0; i < dosyaTipleri.Length; i++)
            {
                if (contentType.Contains(dosyaTipleri[i]))
                {
                    return dosyaClasses[i];
                }
            }
            return "bgText";
        }

        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}