﻿using UnityEngine;
using UnityEditor;

namespace OpenSpace.Visual.ISI {
    public class Radiosity : OpenSpaceStruct {
		public uint num_lod;
		public Pointer off_lod;

		public RadiosityLOD[] lod;

		protected override void ReadInternal(Reader reader) {
			num_lod = reader.ReadUInt32();
			off_lod = Pointer.Read(reader);

			lod = Load.ReadArray<RadiosityLOD>(num_lod, reader, off_lod);
		}
	}
}