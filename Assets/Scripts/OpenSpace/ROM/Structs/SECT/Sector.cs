﻿using OpenSpace.Loader;
using System.Linq;
using UnityEngine;

namespace OpenSpace.ROM {
	public class Sector : ROMStruct {
		// size: 52 or 0x34
		public Reference<SectorSuperObjectArray1> sectors1;
		public Reference<SectorSuperObjectArray1Info> sectors1Info;
		public Reference<SectorSuperObjectArray2> sectors2;
		public Reference<SectorSuperObjectArray3> sectors3;
		public Reference<LightInfoArray> lights;
		public Reference<SectorSuperObjectArray4> sectors4;
		public Reference<SectorSuperObjectArray4Info> sectors4Info;
		public Reference<SectorSuperObjectArray5> sectors5;
		public Reference<SectorSuperObjectArray5Info> sectors5Info;
		public Reference<CompressedVector3Array> boundingVolume;
		public float float14;
		public float float18;
		public Reference<VisualMaterial> background; // 0x1C
		public byte byte1E;
		public byte byte1F;
		public ushort num_sectors1; // 0x20
		public ushort num_sectors2; // 0x22
		public ushort num_sectors3; // 0x24
		public ushort num_lights; // 0x26
		public ushort num_sectors4; // 0x28
		public ushort num_sectors5; // 0x2A, 42
		public byte byte2C;
		public byte byte2D;
		public ushort word2E;
		public byte byte30;
		public byte byte31;
		public ushort word32;

		protected override void ReadInternal(Reader reader) {
			sectors1 = new Reference<SectorSuperObjectArray1>(reader);
			sectors1Info = new Reference<SectorSuperObjectArray1Info>(reader);
			sectors2 = new Reference<SectorSuperObjectArray2>(reader);
			sectors3 = new Reference<SectorSuperObjectArray3>(reader);
			lights = new Reference<LightInfoArray>(reader);
			sectors4 = new Reference<SectorSuperObjectArray4>(reader);
			sectors4Info = new Reference<SectorSuperObjectArray4Info>(reader);
			sectors5 = new Reference<SectorSuperObjectArray5>(reader);
			sectors5Info = new Reference<SectorSuperObjectArray5Info>(reader);
			boundingVolume = new Reference<CompressedVector3Array>(reader, true, va => va.length = 2);
			float14 = reader.ReadSingle();
			float18 = reader.ReadSingle();
			background = new Reference<VisualMaterial>(reader, true);
			byte1E = reader.ReadByte();
			byte1F = reader.ReadByte();
			num_sectors1 = reader.ReadUInt16();
			num_sectors2 = reader.ReadUInt16();
			num_sectors3 = reader.ReadUInt16();
			num_lights = reader.ReadUInt16();
			num_sectors4 = reader.ReadUInt16();
			num_sectors5 = reader.ReadUInt16();
			byte2C = reader.ReadByte();
			byte2D = reader.ReadByte();
			word2E = reader.ReadUInt16();
			byte30 = reader.ReadByte();
			byte31 = reader.ReadByte();
			word32 = reader.ReadUInt16();


			sectors1.Resolve(reader, s1 => s1.length = num_sectors1);
			sectors1Info.Resolve(reader, s1 => s1.length = num_sectors1);
			sectors2.Resolve(reader, s2 => s2.length = num_sectors2);
			sectors3.Resolve(reader, s3 => s3.length = num_sectors3);
			lights.Resolve(reader, li => li.length = num_lights);
			sectors4.Resolve(reader, s4 => s4.length = num_sectors4);
			sectors4Info.Resolve(reader, s4 => s4.length = num_sectors4);
			sectors5.Resolve(reader, s5 => s5.length = num_sectors5);
			sectors5Info.Resolve(reader, s5 => s5.length = num_sectors5);

		}

		public GameObject GetGameObject() {
			GameObject gao = new GameObject("Sector @ " + Offset);
			return gao;
		}
	}
}
