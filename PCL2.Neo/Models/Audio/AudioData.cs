<<<<<<< HEAD
using System;
=======
﻿using System;
>>>>>>> 0af6c65e03163f6908c3345393a3181e8cb1025c
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCL2.Neo.Models.Audio
{
    public enum FileType
    {
        MP3,
        WAV,
        OGG,
        FLAC,
        M4A,
        AAC
    }

    public class AudioData
    {
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required FileType Type { get; set; }
    }
}
