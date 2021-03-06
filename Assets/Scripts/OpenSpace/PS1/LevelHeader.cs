﻿using OpenSpace.FileFormat.Texture;
using OpenSpace.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenSpace.PS1 {
	public class LevelHeader : OpenSpaceStruct {
		public uint num_geometricObjectsDynamic_cine;

		public Pointer off_dynamicWorld;
		public Pointer off_fatherSector;
		public Pointer off_inactiveDynamicWorld;
		public uint num_always;
		public Pointer off_always;
		public uint num_wayPoints;
		public uint num_graphs;
		public Pointer off_wayPoints;
		public Pointer off_graphs;
		public ushort num_persos;
		public ushort ushort_116;
		public Pointer off_persos;
		public Pointer off_states;
		public ushort num_states;
		public ushort ushort_122;
		public uint uint_124;
		public uint uint_128;
		public Pointer off_12C;
		public Pointer off_130;
		public uint uint_134;
		public uint uint_138; // usually 0x28
		public uint uint_13C;
		public uint uint_140;
		public uint uint_144;
		public int initialStreamID; // ID of first cutscene stream upon entering level
		public Pointer off_animPositions;
		public Pointer off_animRotations;
		public Pointer off_animScales;

		// UI Textures
		public byte num_ui_textures;
		public byte byte_159;
		public ushort ushort_15A;
		public Pointer off_ui_textures_names;
		public Pointer off_ui_textures_width;
		public Pointer off_ui_textures_height;
		public Pointer off_ui_textures_pageInfo;
		public Pointer off_ui_textures_palette;
		public Pointer off_ui_textures_xInPage;
		public Pointer off_ui_textures_yInPage;

		public Pointer off_178;
		public int int_17C;

		public Pointer off_geometricObjects_dynamic;
		public Pointer off_geometricObjects_static;
		public uint num_geometricObjects_dynamic;

		public ushort ushort_18C;
		public ushort ushort_18E;
		public short short_190;
		public ushort ushort_192;
		public uint num_ipoCollision;
		public Pointer off_ipoCollision;
		public uint num_meshCollision;
		public Pointer off_meshCollision;
		public uint off_meshCollision_;
		public Pointer off_sectors;
		public ushort num_sectors;
		public ushort ushort_1AA;
		public uint num_cameraModifiers;
		public uint uint_1B0;
		public uint uint_1B4;
		public uint uint_1B8;
		public Pointer off_cameraModifiers_volumes; // 0x28 structs of 0x38 size
		public Pointer off_cameraModifiers; // 0x28 structs of 0x68 size. starts with 0x5.
		public Pointer off_gameMaterials; // b structs of 0x10
		public uint num_gameMaterials; // b
		public uint uint_1CC;
		public ushort ushort_1D0;
		public ushort ushort_1D2;
		public Pointer off_1D4; // same offset as the first pointer in the first struct of off_1C4
		public uint num_ago_textures;
		public Pointer off_ago_textures_pageInfo;
		public Pointer off_ago_textures_palette;
		public Pointer off_ago_textures_xInPage;
		public Pointer off_ago_textures_yInPage;
		public Pointer off_ago_textures_globalX;
		public Pointer off_ago_textures_globalY;
		public uint uint_1F4;
		public uint uint_1F8;
		public uint uint_1FC;

		// Rush
		public Pointer off_rush_114;
		public ushort ushort_rush_118;
		public ushort ushort_rush_11A;
		public Pointer off_ago_textures_width;
		public Pointer off_ago_textures_height;

		// Donald Duck
		public uint num_skinnableObjects;
		public Pointer[] off_skins;
		public Pointer off_current_skin_memory;


		// Parsed
		public PointerList<State> states;
		public SuperObject fatherSector;
		public SuperObject dynamicWorld;
		public SuperObject inactiveDynamicWorld;
		public ObjectsTable geometricObjectsStatic;
		public ObjectsTable geometricObjectsDynamic;
		public Perso[] persos;
		public AlwaysList[] always;
		public Sector[] sectors;
		public SkinnableGeometricObjectList[] skins;
		public GeometricObjectCollide[] ipoCollision;
		public GeometricObjectCollide[] meshCollision;
		public GameMaterial[] gameMaterials;
		public WayPoint[] wayPoints;
		public Graph[] graphs;
		public CameraModifier[] cameraModifiers;
		public CameraModifierVolume[] cameraModifierVolumes;

		public PS1AnimationVector[] animPositions;
		public PS1AnimationQuaternion[] animRotations;
		public PS1AnimationVector[] animScales;

		protected override void ReadInternal(Reader reader) {
			R2PS1Loader l = Load as R2PS1Loader;
			if (Settings.s.game == Settings.Game.RRush) {
				reader.ReadBytes(0x40);
			} else if (Settings.s.game == Settings.Game.DD) {
				reader.ReadBytes(0x58);
			} else if (Settings.s.game == Settings.Game.VIP) {
				reader.ReadBytes(0x28);
			} else if(Settings.s.game == Settings.Game.JungleBook) {
				reader.ReadBytes(0xEC);
			} else {
				reader.ReadBytes(0xCC);
				num_geometricObjectsDynamic_cine = reader.ReadUInt32();
				reader.ReadBytes(0x20); // after this we're at 0xf0
			}
			off_dynamicWorld = Pointer.Read(reader);
			off_fatherSector = Pointer.Read(reader);
			off_inactiveDynamicWorld = Pointer.Read(reader);
			num_always = reader.ReadUInt32();
			off_always = Pointer.Read(reader); //
			num_wayPoints = reader.ReadUInt32(); // x
			num_graphs = reader.ReadUInt32();
			off_wayPoints = Pointer.Read(reader); // x structs of 0x14 waypoints
			off_graphs = Pointer.Read(reader); // graphs. structs of 0x68
			num_persos = reader.ReadUInt16();
			ushort_116 = reader.ReadUInt16();
			off_persos = Pointer.Read(reader);
			Load.print(off_dynamicWorld + " - " + off_fatherSector + " - " + off_inactiveDynamicWorld + " - " + off_always + " - " + off_wayPoints + " - " + off_graphs + " - " + off_persos);
			off_states = Pointer.Read(reader);
			num_states = reader.ReadUInt16();
			ushort_122 = reader.ReadUInt16();
			uint_124 = reader.ReadUInt32();
			uint_128 = reader.ReadUInt32();
			off_12C = Pointer.Read(reader); // this + 0x10 = main character
			off_130 = Pointer.Read(reader);
			uint_134 = reader.ReadUInt32();
			uint_138 = reader.ReadUInt32();
			uint_13C = reader.ReadUInt32(); // same as mainChar_states count
			uint_140 = reader.ReadUInt32();
			uint_144 = reader.ReadUInt32();
			initialStreamID = reader.ReadInt32(); // -1
			Load.print(Pointer.Current(reader));
			off_animPositions = Pointer.Read(reader); // 0x6 size
			off_animRotations = Pointer.Read(reader); // big array of structs of 0x8 size. 4 ushorts per struct
			off_animScales = Pointer.Read(reader);
			Load.print(off_12C + " - " + off_130 + " - " + off_animPositions + " - " + off_animRotations + " - " + off_animScales);
			//Load.print(Pointer.Current(reader));
			if (Settings.s.game == Settings.Game.DD) {
				// Also stuff for big textures, but names and amount is stored in exe
				Pointer.Read(reader);
				Pointer.Read(reader);
				Pointer.Read(reader);
				Pointer.Read(reader);
				Pointer.Read(reader);
				reader.ReadUInt32();
				reader.ReadUInt32();
				Pointer.Read(reader);

				off_geometricObjects_dynamic = Pointer.Read(reader);
				num_skinnableObjects = reader.ReadUInt32();
				int num_skins = 5;
				off_skins = new Pointer[num_skins];
				skins = new SkinnableGeometricObjectList[num_skins];
				for (int i = 0; i < num_skins; i++) {
					off_skins[i] = Pointer.Read(reader);
					skins[i] = Load.FromOffsetOrRead<SkinnableGeometricObjectList>(reader, off_skins[i], onPreRead: s => s.length = num_skinnableObjects);
				}
				off_current_skin_memory = Pointer.Read(reader);
				reader.ReadUInt32(); // total skin memory size?
				off_geometricObjects_static = Pointer.Read(reader);
				num_geometricObjects_dynamic = reader.ReadUInt32();
			} else if (Settings.s.game == Settings.Game.VIP || Settings.s.game == Settings.Game.JungleBook) {
				reader.ReadUInt32();
				reader.ReadUInt32();
				reader.ReadUInt32();
				reader.ReadUInt32();
				reader.ReadUInt32();
				off_geometricObjects_dynamic = Pointer.Read(reader);
				off_geometricObjects_static = Pointer.Read(reader);
				num_geometricObjects_dynamic = reader.ReadUInt32();
			} else {
				// Vignette stuff, big textures
				num_ui_textures = reader.ReadByte();
				byte_159 = reader.ReadByte();
				ushort_15A = reader.ReadUInt16();
				off_ui_textures_names = Pointer.Read(reader);
				off_ui_textures_width = Pointer.Read(reader); // num_vignettes * ushort
				off_ui_textures_height = Pointer.Read(reader); // num_vignettes * ushort
				off_ui_textures_pageInfo = Pointer.Read(reader); // num_vignettes * ushort
				off_ui_textures_palette = Pointer.Read(reader);
				off_ui_textures_xInPage = Pointer.Read(reader);
				off_ui_textures_yInPage = Pointer.Read(reader); // still related to the vignette stuff

				ParseUITextures(reader);

				// Something else
				off_178 = Pointer.Read(reader);
				int_17C = reader.ReadInt32(); // -1

				off_geometricObjects_dynamic = Pointer.Read(reader);
				off_geometricObjects_static = Pointer.Read(reader);
				num_geometricObjects_dynamic = reader.ReadUInt32();
			}
			if (Settings.s.game != Settings.Game.VIP) {
				ushort_18C = reader.ReadUInt16();
				ushort_18E = reader.ReadUInt16();
				short_190 = reader.ReadInt16();
				ushort_192 = reader.ReadUInt16();
				num_ipoCollision = reader.ReadUInt32(); // y
				off_ipoCollision = Pointer.Read(reader); // y structs of 0x3c
				//Load.print(off_ipoCollision + " - " + num_ipoCollision);
				num_meshCollision = reader.ReadUInt32();
				/*if (num_meshCollision > 0) {
					Load.print(num_meshCollision);
					off_meshCollision = Pointer.Read(reader);
				} else {*/
					off_meshCollision_ = reader.ReadUInt32();
				//}
			} else {
				ushort_18C = reader.ReadUInt16();
				ushort_18E = reader.ReadUInt16();
				short_190 = reader.ReadInt16();
				ushort_192 = reader.ReadUInt16();
				reader.ReadUInt32(); // y
				reader.ReadUInt32();
				reader.ReadUInt32();
				reader.ReadUInt32(); //Pointer.Read(reader);
			}
			off_sectors = Pointer.Read(reader); // num_1A8 structs of 0x54
			num_sectors = reader.ReadUInt16(); // actual sectors
											   //Load.print(off_sector_minus_one_things + " - " + bad_off_1A0 + " - " + off_sectors);

			ushort_1AA = reader.ReadUInt16();
			Load.print(Pointer.Current(reader));
			if (Settings.s.game != Settings.Game.DD) {
				num_cameraModifiers = reader.ReadUInt32();
				uint_1B0 = reader.ReadUInt32();
				uint_1B4 = reader.ReadUInt32();
				uint_1B8 = reader.ReadUInt32();
				if (Settings.s.game != Settings.Game.VIP) {
					off_cameraModifiers_volumes = Pointer.Read(reader); // uint_1B4 * 0x70
					off_cameraModifiers = Pointer.Read(reader); // uint_1ac * 0x68. lights? first with type (first byte) = 9 is camera related? position is at 0x14, 0x18, 0x1c?
				} else {
					reader.ReadUInt32();
					reader.ReadUInt32();
				}

				if (Settings.s.game == Settings.Game.RRush) {
					off_rush_114 = Pointer.Read(reader);
					ushort_rush_118 = reader.ReadUInt16();
					ushort_rush_11A = reader.ReadUInt16();
				}
			}

			off_gameMaterials = Pointer.Read(reader);
			num_gameMaterials = reader.ReadUInt32();
			uint_1CC = reader.ReadUInt32();
			ushort_1D0 = reader.ReadUInt16();
			ushort_1D2 = reader.ReadUInt16();
			off_1D4 = Pointer.Read(reader);
			num_ago_textures = reader.ReadUInt32();
			off_ago_textures_pageInfo = Pointer.Read(reader);
			off_ago_textures_palette = Pointer.Read(reader);
			off_ago_textures_xInPage = Pointer.Read(reader);
			off_ago_textures_yInPage = Pointer.Read(reader);
			off_ago_textures_globalX = Pointer.Read(reader);
			off_ago_textures_globalY = Pointer.Read(reader);
			if (Settings.s.game == Settings.Game.RRush) {
				off_ago_textures_width = Pointer.Read(reader);
				off_ago_textures_height = Pointer.Read(reader);
				//Load.print(Pointer.Current(reader));
				ParseAGOTextures(reader);
			} else if(Settings.s.game == Settings.Game.R2) {
				uint_1F4 = reader.ReadUInt32();
				uint_1F8 = reader.ReadUInt32();
				uint_1FC = reader.ReadUInt32();
				ParseAGOTextures(reader);
			}

			// Parse
			states = Load.FromOffsetOrRead<PointerList<State>>(reader, off_states, s => s.length = num_states);
			persos = Load.ReadArray<Perso>(num_persos, reader, off_persos);
			geometricObjectsDynamic = Load.FromOffsetOrRead<ObjectsTable>(reader, off_geometricObjects_dynamic, onPreRead: t => t.length = num_geometricObjects_dynamic - 2);
			geometricObjectsStatic = Load.FromOffsetOrRead<ObjectsTable>(reader, off_geometricObjects_static, onPreRead: t => {
				if (Settings.s.game == Settings.Game.R2) t.length = num_ipoCollision;
			});
			ipoCollision = Load.ReadArray<GeometricObjectCollide>(num_ipoCollision, reader, off_ipoCollision);
			meshCollision = Load.ReadArray<GeometricObjectCollide>(num_meshCollision, reader, off_meshCollision);
			gameMaterials = Load.ReadArray<GameMaterial>(num_gameMaterials, reader, off_gameMaterials);
			wayPoints = Load.ReadArray<WayPoint>(num_wayPoints, reader, off_wayPoints);
			graphs = Load.ReadArray<Graph>(num_graphs, reader, off_graphs);
			cameraModifiers = Load.ReadArray<CameraModifier>(num_cameraModifiers, reader, off_cameraModifiers);
			cameraModifierVolumes = Load.ReadArray<CameraModifierVolume>(num_cameraModifiers, reader, off_cameraModifiers_volumes);
			always = Load.ReadArray<AlwaysList>(num_always, reader, off_always);
			sectors = Load.ReadArray<Sector>(num_sectors, reader, off_sectors);

			animPositions = Load.ReadArray<PS1AnimationVector>((off_animRotations.offset - off_animPositions.offset) / 6, reader, off_animPositions);
			animRotations = Load.ReadArray<PS1AnimationQuaternion>((off_animScales.offset - off_animRotations.offset) / 8, reader, off_animRotations);
			animScales = Load.ReadArray<PS1AnimationVector>(l.maxScaleVector[0] + 1, reader, off_animScales);
		}

		public void ParseUITextures(Reader reader) {
			if (!Load.exportTextures) return;
			Load.print("Num UI Textures: " + num_ui_textures);

			UITexture[] textures = new UITexture[num_ui_textures];
			for (int i = 0; i < textures.Length; i++) {
				textures[i] = new UITexture();
			}
			Pointer.DoAt(ref reader, off_ui_textures_names, () => {
				for (int i = 0; i < textures.Length; i++) {
					Pointer p = Pointer.Read(reader);
					Pointer.DoAt(ref reader, p, () => {
						textures[i].name = reader.ReadString(0x1C);
					});
				}
			});
			Pointer.DoAt(ref reader, off_ui_textures_width, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].width = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ui_textures_height, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].height = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ui_textures_pageInfo, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].pageInfo = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ui_textures_palette, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].palette = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ui_textures_xInPage, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].xInPage = reader.ReadByte();
				}
			});
			Pointer.DoAt(ref reader, off_ui_textures_yInPage, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].yInPage = reader.ReadByte();
				}
			});


			R2PS1Loader l = Load as R2PS1Loader;
			PS1VRAM vram = l.vram;
			for (int i = 0; i < textures.Length; i++) {
				UITexture t = textures[i];
				//Load.print(t.name + " - " + t.width + " - " + t.height + " - " + t.xInPage + " - " + t.yInPage);
				t.texture = vram.GetTexture(t.width, t.height, t.pageInfo, t.palette, t.xInPage, t.yInPage);
				Util.ByteArrayToFile(l.gameDataBinFolder + "textures/ui/" + t.name + ".png", t.texture.EncodeToPNG());
			}
		}
		public void ParseAGOTextures(Reader reader) {
			if (!Load.exportTextures) return;
			Load.print("Num AGO Textures: " + num_ago_textures);

			UITexture[] textures = new UITexture[num_ago_textures];
			for (int i = 0; i < textures.Length; i++) {
				textures[i] = new UITexture();
			}
			for (int i = 0; i < textures.Length; i++) {
				textures[i].name = "Tex_" + i;
			}
			Pointer.DoAt(ref reader, off_ago_textures_pageInfo, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].pageInfo = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ago_textures_palette, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].palette = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ago_textures_xInPage, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].xInPage = reader.ReadByte();
				}
			});
			Pointer.DoAt(ref reader, off_ago_textures_yInPage, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].yInPage = reader.ReadByte();
				}
			});
			Pointer.DoAt(ref reader, off_ago_textures_globalX, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].globalX = reader.ReadUInt16();
				}
			});
			Pointer.DoAt(ref reader, off_ago_textures_globalY, () => {
				for (int i = 0; i < textures.Length; i++) {
					textures[i].globalY = reader.ReadUInt16();
				}
			});
			if (Settings.s.game == Settings.Game.RRush) {
				Pointer.DoAt(ref reader, off_ago_textures_width, () => {
					for (int i = 0; i < textures.Length; i++) {
						textures[i].width = reader.ReadUInt16();
					}
				});
				Pointer.DoAt(ref reader, off_ago_textures_height, () => {
					for (int i = 0; i < textures.Length; i++) {
						textures[i].height = reader.ReadUInt16();
					}
				});
			}


			R2PS1Loader l = Load as R2PS1Loader;
			PS1VRAM vram = l.vram;
			for (int i = 0; i < textures.Length; i++) {
				UITexture t = textures[i];
				if (t.width != 0) {
					//Load.print(t.name + " - " + t.width + " - " + t.height + " - " + t.xInPage + " - " + t.yInPage);
					t.texture = vram.GetTexture(t.width, t.height, t.pageInfo, t.palette, t.xInPage, t.yInPage);
					Util.ByteArrayToFile(l.gameDataBinFolder + "textures/ago/" + Load.lvlName + "/"
						+ $"{t.name}.png", t.texture.EncodeToPNG());
				} else {
					// Uncomment to extract AGO textures with hardcoded width & height
					/*int tp = Util.ExtractBits(t.pageInfo, 2, 7); // 0: 4-bit, 1: 8-bit, 2: 15-bit direct
					int pageW = 64;
					if (tp == 1) {
						pageW = 128;
					} else if (tp == 0) {
						pageW = 256;
					}
					int maxW = pageW - t.xInPage;
					int maxH = 256 - t.yInPage;
					for (int x = 0; x < 5; x++) {
						ushort w = (ushort)(0x10 << x);
						if (w > maxW) continue;
						for (int y = 0; y < 5; y++) {
							ushort h = (ushort)(0x10 << y);
							if (h > maxH) continue;
							try {
								t.texture = vram.GetTexture(w, h, t.pageInfo, t.palette, t.xInPage, t.yInPage);
								Util.ByteArrayToFile(l.gameDataBinFolder + "textures/ago/" + Load.lvlName + "/"
									+ $"{t.name}_{w}x{h}.png", t.texture.EncodeToPNG());
							} catch { }
						}
					}*/
				}
			}
		}

		public class UITexture {
			public string name;
			public ushort width;
			public ushort height;
			public ushort pageInfo;
			public ushort palette;
			public byte xInPage;
			public byte yInPage;

			public ushort globalX;
			public ushort globalY;

			public Texture2D texture;
		}
	}
}
