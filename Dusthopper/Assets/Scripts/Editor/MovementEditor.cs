using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Movement))]
public class MovementEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Movement myScript = (Movement)target;
		if(GUILayout.Button("Switch Asteroid"))
		{
			myScript.ChangeAsteroid ();
		}
	}
}
