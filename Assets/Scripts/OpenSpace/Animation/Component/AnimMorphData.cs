﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OpenSpace.Animation.Component {
    public class AnimMorphData {

        public AnimMorphData() {}

        public static AnimMorphData Read(EndianBinaryReader reader) {
            AnimMorphData m = new AnimMorphData();
            if (Settings.s.engineMode == Settings.EngineMode.R2) {
                reader.ReadBytes(0x8); // Haven't deciphered this yet
            } else {
                reader.ReadBytes(0x26); // Haven't deciphered this yet
            }
            return m;
        }
    }
}
