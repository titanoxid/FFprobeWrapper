using FFprobeWrapper.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FFprobeWrapper.Library
{
    public class FFprobeExtractor
    {
        public FFprobe FFprobe { get; private set; }
        public string Filename { get; private set; }

        public FFprobeExtractor(FFprobe fFprobe, string filename)
        {
            FFprobe = fFprobe;
            Filename = filename;
        }

        public string GetVideoFramerate()
        {
            var process = FFprobe.GetProcess();
            process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoFramerate);
            var framerateString = GetProcessOutput(process);
            var framerate = ConvertFramerateString(framerateString);
            return framerate;
        }

        public string GetVideoDimension()
        {
            var ffprobeProcess = FFprobe.GetProcess();
            ffprobeProcess.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoDimension);
            var dimension = GetProcessOutput(ffprobeProcess);
            return dimension;
        }

        public string GetVideoDuration()
        {
            var process = FFprobe.GetProcess();
            process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoDuration);
            var duration = GetProcessOutput(process);
            return duration;
        }

        public string GetVideoCodec()
        {
            var process = FFprobe.GetProcess();
            process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoCodec);
            var codec = GetProcessOutput(process);
            return codec;
        }

        public string GetAudioCodec()
        {
            var process = FFprobe.GetProcess();
            process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.AudioCodec);
            var codec = GetProcessOutput(process);
            return codec;
        }

        public string GetInfo(ExtractorInfoType type)
        {
            var process = FFprobe.GetProcess();
            switch (type)
            {
                case ExtractorInfoType.AudioCodec:
                    process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.AudioCodec);
                    break;
                case ExtractorInfoType.VideoCodec:
                    process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoCodec);
                    break;
                case ExtractorInfoType.Bitrate:
                    process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoBitrate);
                    break;
                case ExtractorInfoType.Dimension:
                    process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoDimension);
                    break;
                case ExtractorInfoType.Duration:
                    process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoDuration);
                    break;
                case ExtractorInfoType.Framerate:
                    process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoFramerate);
                    break;
            }
            var data = GetProcessOutput(process);
            return data;
        }

        public MetadataInfo GetVideoMetaDataInfo()
        {
            var process = FFprobe.GetProcess();
            process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoMetaData);
            var output = GetProcessOutput(process);
            var audioCodec = GetAudioCodec();
            var metaData = GetMetaDataFromOutput(output, audioCodec);
            return metaData;
        }

        public async Task<MetadataInfo> GetVideoMetaDataInfoAsync()
        {
            var process = FFprobe.GetProcess();
            process.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoMetaData);
            var dataTask = Task.Run(async () =>
            {
                var output = await GetProcessOutputAsync(process);
                return output;
            });
            var audioCodecTask = GetAudioCodecAsync();
            var data = await Task.WhenAll(dataTask, audioCodecTask);
            var metaData = GetMetaDataFromOutput(data[0], data[1]);
            return metaData;
        }

        private string GetProcessOutput(Process process)
        {
            process.Start();
            var errors = process.StandardError.ReadToEnd();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var info = SplitDataIntoArray(output);
            if (info.Length == 1)
            {
                output = output.Remove(output.IndexOf("\r\n"));
            }
            return output;
        }

        public async Task<string> GetVideoFramerateAsync()
        {
            var ffprobeProcess = FFprobe.GetProcess();
            ffprobeProcess.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoFramerate);
            var framerate = await Task.Run(async () =>
            {
                var output = await GetProcessOutputAsync(ffprobeProcess);
                var computedFramerate = ConvertFramerateString(output);
                return computedFramerate;
            });
            return framerate;
        }

        public async Task<string> GetVideoDurationAsync()
        {
            var ffprobeProcess = FFprobe.GetProcess();
            ffprobeProcess.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoDuration);
            var duration = await Task.Run(async () =>
            {
                var output = await GetProcessOutputAsync(ffprobeProcess);
                return output;
            });
            return duration;
        }

        public async Task<string> GetVideoDimensionAsync()
        {
            var ffprobeProcess = FFprobe.GetProcess();
            ffprobeProcess.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoDimension);
            var dimension = await Task.Run(async () =>
            {
                var output = await GetProcessOutputAsync(ffprobeProcess);
                return output;
            });
            return dimension;
        }

        public async Task<string> GetVideoCodecAsync()
        {
            var ffprobeProcess = FFprobe.GetProcess();
            ffprobeProcess.StartInfo.Arguments = GetStartArgument(FFprobeParameter.VideoCodec);
            var codec = await Task.Run(async () =>
            {
                var output = await GetProcessOutputAsync(ffprobeProcess);
                return output;
            });
            return codec;
        }

        public async Task<string> GetAudioCodecAsync()
        {
            var ffprobeProcess = FFprobe.GetProcess();
            ffprobeProcess.StartInfo.Arguments = GetStartArgument(FFprobeParameter.AudioCodec);
            var codec = await Task.Run(async () =>
            {
                var output = await GetProcessOutputAsync(ffprobeProcess);
                return output;
            });
            return codec;
        }

        private async Task<string> GetProcessOutputAsync(Process process)
        {
            process.Start();
            var errorTask = process.StandardError.ReadToEndAsync();
            var outputTask = process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();
            string[] output = await Task.WhenAll(errorTask, outputTask);
            var data = output[1];
            var info = SplitDataIntoArray(data);
            if (info.Length == 1)
            {
                data = data.Remove(data.IndexOf("\r\n"));
            }
            return data;
        }

        private MetadataInfo GetMetaDataFromOutput(string output, string audioData)
        {
            var data = SplitDataIntoArray(output);
            var dimension = $"{data[1]}x{data[2]}";
            var duration = ConvertToTimeSpan(data[4]);
            var bitrate = Convert.ToUInt32(data[5]);
            var framerate = ConvertToDouble(data[3]);

            var metaData = new MetadataInfo
            {
                AudioCodec = audioData,
                Dimension = dimension,
                Duration = duration,
                VideoCodec = data[0],
                Bitrate = bitrate,
                Framerate = framerate
            };
            return metaData;
        }

        private string GetStartArgument(string parameter)
        {
            return $"{parameter} \"{Filename}\"";
        }

        private static TimeSpan ConvertToTimeSpan(string input)
        {
            var parsedTimestamp = Double.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
            var timeSpan = TimeSpan.FromSeconds(parsedTimestamp);
            //var e = d.Subtract(TimeSpan.FromMilliseconds(d.Milliseconds));
            return timeSpan;
        }

        private static double ConvertToDouble(string framerateString)
        {
            if (Double.TryParse(framerateString, out double framerate) == false)
            {
                var leftPart = framerateString.Remove(framerateString.IndexOf("/"));
                var rightPart = framerateString.Substring(framerateString.IndexOf("/") + 1);

                Double.TryParse(leftPart, out double left);
                Double.TryParse(rightPart, out double right);

                framerate = left / right;
            }
            return framerate;
        }

        private static string ConvertFramerateString(string framerateString)
        {
            if (Double.TryParse(framerateString, out double framerate) == false)
            {
                framerate = ConvertToDouble(framerateString);
            }
            var result = framerate.ToString("F2", new System.Globalization.NumberFormatInfo { NumberDecimalSeparator = "." });
            return result;
        }

        private static string[] SplitDataIntoArray(string output)
        {
            var stringSeperator = new char[] { '\r', '\n' };
            var info = output.Split(stringSeperator, StringSplitOptions.RemoveEmptyEntries);
            return info;
        }
    }
}
