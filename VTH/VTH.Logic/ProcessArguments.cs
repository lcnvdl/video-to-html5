using System.Collections.Generic;

namespace VTH.Logic
{
    public class ProcessArguments
    {
        public List<string> Files { get; set; }
        public bool Audio { get; set; }

        public ProcessArguments(List<string> files, bool keepAudio)
        {
            Files = files;
            Audio = keepAudio;
        }
    }
}
