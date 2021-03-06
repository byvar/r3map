﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OpenSpace.AI {
    public class AIModel : OpenSpaceStruct {
        public Pointer off_behaviors_normal;
        public Pointer off_behaviors_reflex;
        public Pointer off_dsgVar;
        public Pointer off_macros;
        public uint flags;

        public Behavior[] behaviors_normal = null;
        public Behavior[] behaviors_reflex = null;
        public DsgVar dsgVar;
        public Macro[] macros = null;

        public string name;

        protected override void ReadInternal(Reader reader) {
            MapLoader l = MapLoader.Loader;
            l.aiModels.Add(this);
            //l.print("AIModel @ " + Offset);
            off_behaviors_normal = Pointer.Read(reader);
            off_behaviors_reflex = Pointer.Read(reader);
            off_dsgVar = Pointer.Read(reader);
            if (Settings.s.engineVersion >= Settings.EngineVersion.R2) {
                off_macros = Pointer.Read(reader);
                flags = reader.ReadUInt32();
            }

            Pointer.DoAt(ref reader, off_behaviors_normal, () => {
                Pointer off_entries = Pointer.Read(reader);
                uint num_entries = reader.ReadUInt32();
                behaviors_normal = new Behavior[num_entries];
                if (num_entries > 0) {
                    Pointer.DoAt(ref reader, off_entries, () => {
                        for (int i = 0; i < num_entries; i++) {
                            behaviors_normal[i] = l.FromOffsetOrRead<Behavior>(reader, Pointer.Current(reader), inline: true);
                            behaviors_normal[i].type = Behavior.BehaviorType.Intelligence;
                            behaviors_normal[i].aiModel = this;
                            behaviors_normal[i].index = i;
                        }
                    });
                }
            });
            Pointer.DoAt(ref reader, off_behaviors_reflex, () => {
                Pointer off_entries = Pointer.Read(reader);
                uint num_entries = reader.ReadUInt32();
                behaviors_reflex = new Behavior[num_entries];
                if (num_entries > 0) {
                    Pointer.DoAt(ref reader, off_entries, () => {
                        for (int i = 0; i < num_entries; i++) {
                            behaviors_reflex[i] = l.FromOffsetOrRead<Behavior>(reader, Pointer.Current(reader), inline: true);
                            behaviors_reflex[i].type = Behavior.BehaviorType.Reflex;
                            behaviors_reflex[i].aiModel = this;
                            behaviors_reflex[i].index = i;
                        }
                    });
                }
            });

            dsgVar = l.FromOffsetOrRead<DsgVar>(reader, off_dsgVar);

            Pointer.DoAt(ref reader, off_macros, () => {
                Pointer off_entries = Pointer.Read(reader);
                byte num_entries = reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                macros = new Macro[num_entries];
                if (num_entries > 0) {
                    Pointer.DoAt(ref reader, off_entries, () => {
                        for (int i = 0; i < num_entries; i++) {
                            macros[i] = l.FromOffsetOrRead<Macro>(reader, Pointer.Current(reader), inline: true);
                            macros[i].aiModel = this;
                            macros[i].index = i;
                        }
                    });
                }
            });
        }

        /*public Behavior GetBehaviorByOffset(Pointer offset)
        {
            if (offset == null) {
                return null;
            }
            // Look in both behavior lists
            if (behaviors_normal != null) {
                foreach (Behavior behavior in behaviors_normal) {

                    if (behavior.offset == offset) {
                        return behavior;
                    }
                }
            }
            if (behaviors_reflex != null) {
                foreach (Behavior behavior in behaviors_reflex) {

                    if (behavior.offset == offset) {
                        return behavior;
                    }

                }
            }
            return null;
        }

		public int GetBehaviorIndex(Behavior behavior) {
			if (behavior == null) return -1;
			// Look in both behavior lists
			if (behaviors_normal != null) {
				for (int i = 0; i < behaviors_normal.Length; i++) {
					if (behavior == behaviors_normal[i]) return i;
				}
			}
			if (behaviors_reflex != null) {
				for (int i = 0; i < behaviors_reflex.Length; i++) {
					if (behavior == behaviors_reflex[i]) return i;
				}
			}
			return -1;
		}

        public Macro GetMacroByOffset(Pointer offset)
        {
            if (offset == null) {
                return null;
            }
            // Look in both behavior lists
            if (macros != null) {
                foreach (Macro macro in macros) {

                    if (macro.offset == offset) {
                        return macro;
                    }
                }
            }
            
            return null;
        }*/
    }
}