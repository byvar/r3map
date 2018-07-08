﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OpenSpace.Animation.Component {
    public class AnimFramesKFIndex {
        public uint kf;

        public AnimFramesKFIndex() {}

        public static AnimFramesKFIndex Read(EndianBinaryReader reader) {
            AnimFramesKFIndex kfi = new AnimFramesKFIndex();
            if (Settings.s.isR2Demo) {
                kfi.kf = reader.ReadUInt16();
            } else {
                kfi.kf = reader.ReadUInt32();
            }
            return kfi;
        }
    }
}