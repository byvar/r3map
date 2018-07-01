﻿using OpenSpace.Waypoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OpenSpace.AI {
    public class ScriptNode {
        public Pointer offset;
        public uint param;
        public byte type;
        public byte indent;

        // derived fields
        public Pointer param_ptr;
        public NodeType nodeType;

        public ScriptNode(Pointer offset) {
            this.offset = offset;
        }

        public static ScriptNode Read(EndianBinaryReader reader, Pointer offset) {
            MapLoader l = MapLoader.Loader;
            ScriptNode sn = new ScriptNode(offset);
            
            sn.param = reader.ReadUInt32();
            sn.param_ptr = Pointer.GetPointerAtOffset(offset); // if parameter is pointer
            
            if (l.mode == MapLoader.Mode.Rayman3GC) {
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                sn.type = reader.ReadByte();

                reader.ReadByte();
                reader.ReadByte();
                sn.indent = reader.ReadByte();
                reader.ReadByte();
            } else {
                reader.ReadByte();
                reader.ReadByte();
                sn.indent = reader.ReadByte();
                sn.type = reader.ReadByte();
            }
            sn.nodeType = NodeType.Unknown;
            if (Settings.s.engineMode == Settings.EngineMode.R2) {
                sn.nodeType = R2AITypes.getNodeType(sn.type);
            } else {
                sn.nodeType = R3AITypes.getNodeType(sn.type);
            }

            if (sn.param_ptr != null && sn.nodeType != NodeType.Unknown) {
                if (sn.nodeType == NodeType.WayPointRef) {
                    Pointer off_current = Pointer.Goto(ref reader, sn.param_ptr);
                    WayPoint waypoint = WayPoint.Read(reader, sn.param_ptr);
                    //l.print("Waypoint at " + waypoint.position.x + ", " + waypoint.position.y + ", " + waypoint.position.z);
                    Pointer.Goto(ref reader, off_current);
                } else if (sn.nodeType == NodeType.String) {
                    Pointer off_currentNode = Pointer.Goto(ref reader, sn.param_ptr);
                    string str = reader.ReadNullDelimitedString();
                    l.strings[sn.param_ptr] = str;
                    Pointer.Goto(ref reader, off_currentNode);
                }
            }
            return sn;
        }


        public enum NodeType {
            Unknown,
            KeyWord,
            Condition,
            Operator,
            Function,
            Procedure,
            MetaAction,
            BeginMacro,
            EndMacro,
            Field,
            DsgVarRef,
            Constant,
            Real,
            Button,
            ConstantVector,
            Vector,
            Mask,
            ModuleRef,
            DsgVarId,
            String,
            LipsSynchroRef,
            FamilyRef,
            PersoRef,
            ActionRef,
            SuperObjectRef,
            WayPointRef,
            TextRef,
            ComportRef,
            SoundEventRef,
            ObjectTableRef,
            GameMaterialRef,
            ParticleGenerator,
            VisualMaterial,
            Color,
            DataType42,
            Light,
            Caps,
            SubRoutine,
            GraphRef
        };

        internal static bool IsNodeTypeVariable(NodeType nodeType) {
            switch (nodeType) {
                case NodeType.Unknown: return false;
                case NodeType.KeyWord: return false;
                case NodeType.Condition: return false;
                case NodeType.Operator: return false;
                case NodeType.Function: return false;
                case NodeType.Procedure: return false;
                case NodeType.MetaAction: return false;
                case NodeType.BeginMacro: return false;
                case NodeType.EndMacro: return false;
                case NodeType.SubRoutine: return false;
            }

            return true;
        }
    }
}
