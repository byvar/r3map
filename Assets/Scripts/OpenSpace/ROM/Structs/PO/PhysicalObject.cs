﻿using OpenSpace.Loader;
using System.Linq;
using UnityEngine;

namespace OpenSpace.ROM {
	public class PhysicalObject : ROMStruct {
		public Reference<GeometricObject> visual;
		public Reference<CollSet> collide;

		protected override void ReadInternal(Reader reader) {
			visual = new Reference<GeometricObject>(reader, true);
			collide = new Reference<CollSet>(reader, true);
		}

		public GameObject GetGameObject() {
			GameObject gao = new GameObject("PO @ " + Offset);
			if (visual.Value != null) {
				GameObject child = visual.Value.GetGameObject(GeometricObject.Type.Visual);
				child.transform.SetParent(gao.transform);
				child.name = "[Visual] " + child.name;
			}
			if (collide.Value != null && collide.Value.mesh.Value != null) {
				GameObject child = collide.Value.mesh.Value.GetGameObject(GeometricObject.Type.Collide);
				child.transform.SetParent(gao.transform);
				child.name = "[Collide] " + child.name;
			}
			return gao;
		}
	}
}