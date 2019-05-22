using System;
using System.Collections.Generic;
using System.Text;

namespace FFprobeWrapper.Library
{
    public static class FFprobeParameter
    {
        public const string VideoDuration = "-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 -sexagesimal";
        public const string VideoFramerate = "-v error -select_streams v:0 -show_entries stream=avg_frame_rate -of default=noprint_wrappers=1:nokey=1";
        public const string VideoDimension = "-v error -select_streams v:0 -show_entries stream=width,height -of csv=s=x:p=0";
        public const string VideoBitrate = "-v error -select_streams v:0 -show_entries stream=bit_rate -of default=noprint_wrappers=1:nokey=1";
        public const string VideoCodec = "-v error -select_streams v:0 -show_entries stream=codec_name -of default=noprint_wrappers=1:nokey=1";
        public const string AudioCodec = "-v error -select_streams a:0 -show_entries stream=codec_name -of default=noprint_wrappers=1:nokey=1";
        public const string VideoMetaData = "-v error -select_streams v:0 -show_entries stream=codec_name,width,height,bit_rate,duration,avg_frame_rate -of default=noprint_wrappers=1:nokey=1";
        //public const string VideoMetaData = "-v error -select_streams v:0 -show_entries stream=codec_name,width,height,bit_rate,duration -of default=noprint_wrappers=1:nokey=1 -sexagesimal";
    }
}
