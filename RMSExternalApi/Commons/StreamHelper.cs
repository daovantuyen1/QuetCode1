using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Commons
{
    public class StreamHelper
    {
        public static byte[] StreamToBytes(Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}