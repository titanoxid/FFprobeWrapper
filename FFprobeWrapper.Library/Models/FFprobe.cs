using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace FFprobeWrapper.Library.Models
{
    public class FFprobe
    {
        public string ExecutionPath { get; private set; }

        public FFprobe(string path)
        {
            ExecutionPath = GetExecutionPath(path);
        }

        private string GetExecutionPath(string path)
        {
            return Path.Combine(path, "ffprobe");
        }

        public Process GetProcess()
        {
            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = ExecutionPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            return process;
        }
    }
}
