﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using OpenSpace.ROM;
using System.Collections.Generic;
using System;
using UnityEditor.IMGUI.Controls;
using System.Linq;

[CustomEditor(typeof(ROMPersoBehaviour))]
public class ROMPersoBehaviourEditor : Editor {
	StateTransitionsTreeView treeViewStateTransitions;
	TreeViewState treeviewStateTransitionsState;
	MultiColumnHeaderState m_MultiColumnHeaderState;

	public override void OnInspectorGUI() {
        DrawDefaultInspector();

        ROMPersoBehaviour pb = (ROMPersoBehaviour)target;
        pb.poListIndex = EditorGUILayout.Popup("Objects List", pb.poListIndex, pb.poListNames);
        pb.stateIndex = EditorGUILayout.Popup(pb.stateIndex, pb.stateNames);
        /*if (pb.perso != null && pb.perso.p3dData != null) {
            GUILayout.Label("S0: " + pb.perso.p3dData.off_stateInitial);
            GUILayout.Label("S1: " + pb.perso.p3dData.off_stateCurrent);
            GUILayout.Label("S2: " + pb.perso.p3dData.off_state2);
        }*/

        GUILayout.BeginVertical();
        /*byte updateCheckByte = pb.perso.stdGame.updateCheckByte;
        GUILayout.Label("StdGame: " + pb.perso.stdGame.offset.ToString());
        GUILayout.Label("StdGame.UpdateByte: " + Convert.ToString(updateCheckByte, 2).PadLeft(8, '0'));
        bool consideredOnScreen = (updateCheckByte & (1 << 5)) != 0;
        bool consideredTooFarAway = (updateCheckByte & (1 << 7)) != 0;
        GUILayout.Label("Considered on screen (bit 5): " + consideredOnScreen);
        GUILayout.Label("Considered too far away (bit 7): " + consideredTooFarAway);
        GUILayout.Label("State custom bits: " + Convert.ToString(pb.state.customStateBits, 2).PadLeft(8, '0'));*/

		/* // Only enable when working on morph data, it prevents from using the buttons properly otherwise
        if (pb.a3d != null && pb.morphDataArray != null) {
            for (int i = 0; i < pb.a3d.num_channels; i++) {
                AnimMorphData currentMorphData = pb.morphDataArray[i, pb.currentFrame];

                if (currentMorphData != null) {
                    GUILayout.Label("MorphData[" + i + "," + pb.currentFrame + "]: Morph to " + currentMorphData.objectIndexTo + ", progress " + currentMorphData.morphProgress);
                }
            }
        }*/
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUI.enabled = pb.currentState > 0;
        if (GUILayout.Button("Previous state")) pb.SetState(pb.currentState - 1);
        GUI.enabled = (pb.stateNames != null && pb.currentState < pb.stateNames.Length - 1);
        if (GUILayout.Button("Next state")) pb.SetState(pb.currentState + 1);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

		if (pb.IsLoaded) {
			Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 100f);
			InitTransitionsTreeIfNeeded(rect, pb);
			if (treeViewStateTransitions.stateIndex != pb.currentState
				|| treeViewStateTransitions.persoROM != pb) {
				treeViewStateTransitions.persoROM = pb;
				treeViewStateTransitions.stateIndex = pb.currentState;
				treeViewStateTransitions.treeModel.SetData(GetData());
				treeViewStateTransitions.Reload();
			}
			treeViewStateTransitions.OnGUI(rect);
		}

		//if (GUILayout.Button("Print scripts")) pb.PrintScripts();
		if (GUILayout.Button("Print translated scripts")) pb.PrintTranslatedScripts();
        if (GUILayout.Button("Print Animation Debug Info")) pb.PrintAnimationDebugInfo();
	}

	IList<StateTransitionsTreeElement> GetData() {
		ROMPersoBehaviour pb = (ROMPersoBehaviour)target;
		List<StateTransitionsTreeElement> tr = new List<StateTransitionsTreeElement>();
		tr.Add(new StateTransitionsTreeElement("Hidden root", -1, -1));
		BasePersoBehaviour.StateTransition[] transitions = pb.GetStateTransitions(pb.currentState);
		if (transitions != null) {
			int id = 0;
			foreach (var t in transitions) {
				tr.Add(new StateTransitionsTreeElement("State transition " + t.TargetStateName, 0, id) {
					stateToGoName = t.StateToGoName,
					stateToGoIndex = t.StateToGoIndex,
					targetStateName = t.TargetStateName,
					targetStateIndex = t.TargetStateIndex,
					linkingType = t.LinkingType
				});
				id++;
			}
		}
		return tr;
	}

	void InitTransitionsTreeIfNeeded(Rect transitionsRect, ROMPersoBehaviour target) {
		if (treeViewStateTransitions == null || treeviewStateTransitionsState == null || treeViewStateTransitions.persoROM != target) {
			treeviewStateTransitionsState = new TreeViewState();

			bool firstInit = m_MultiColumnHeaderState == null;
			var headerState = StateTransitionsTreeView.CreateDefaultMultiColumnHeaderState(transitionsRect.width);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
				MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
			m_MultiColumnHeaderState = headerState;

			var multiColumnHeader = new MultiColumnHeader(headerState);
			if (firstInit)
				multiColumnHeader.ResizeToFit();

			var treeModel = new TreeModel<StateTransitionsTreeElement>(GetData());

			treeViewStateTransitions = new StateTransitionsTreeView(treeviewStateTransitionsState, multiColumnHeader, treeModel) {
				persoROM = target,
				stateIndex = target.stateIndex,
			};
		}
	}
}