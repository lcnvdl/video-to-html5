using System;
using System.Collections.Generic;
using System.IO;

namespace VTH.Logic
{
    public class Ffmpeg: ConsoleProgram
    {
        protected override string ProgramX86
        {
            get { return Path.Combine(Environment.CurrentDirectory, "Data", "Ffmpeg", "x86", "bin", "ffmpeg.exe"); }
        }

        protected override string ProgramX64
        {
            get { return Path.Combine(Environment.CurrentDirectory, "Data", "Ffmpeg", "x64", "bin", "ffmpeg.exe"); }
        }

        public string InPath { get; set; }
        public bool Audio { get; set; }

        public Ffmpeg(string file, bool audio)
        {
            InPath = file;
            Audio = audio;
        }

        public IEnumerable<int> Process()
        {
            FileInfo fi = new FileInfo(InPath);
            string inFile = fi.Name;
            string outFile = Path.GetFileNameWithoutExtension(fi.Name);
            string inExt = fi.Extension.Replace(".", "").ToLower();

            yield return 0;

            //  To webm
            
            if (inExt != "webm")
            {
                string command = "-i \"{0}\" {3} \"{1}.{2}\"";
                this.Execute(string.Format(command, inFile, outFile, "webm", Audio ? "-acodec libvorbis" : "-an"), fi.DirectoryName);
            }
            yield return 33;

            //  To ogv
            if (inExt != "ogv")
            {
                string command = "-i \"{0}\" -vcodec libtheora {3} \"{1}.{2}\"";
                this.Execute(string.Format(command, inFile, outFile, "ogv", Audio ? "-acodec libvorbis" : "-an"), fi.DirectoryName);
            }
            yield return 66;

            //  To mp4
            if (inExt != "mp4")
            {
                string command = "-i \"{0}\" -vcodec mpeg4 {3} \"{1}.{2}\"";
                this.Execute(string.Format(command, inFile, outFile, "mp4", Audio ? "-acodec libfaac" : "-an"), fi.DirectoryName);
            }
            yield return 100;

        }
    }
}
