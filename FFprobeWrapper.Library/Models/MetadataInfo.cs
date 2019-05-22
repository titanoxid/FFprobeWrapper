using System;
using System.Collections.Generic;
using System.Text;

namespace FFprobeWrapper.Library.Models
{
    public class MetadataInfo
    {
        public string Dimension { get; set; }
        public TimeSpan Duration { get; set; }
        public double Framerate { get; set; }
        public uint Bitrate { get; set; }
        public string VideoCodec { get; set; }
        public string AudioCodec { get; set; }
    }
}
