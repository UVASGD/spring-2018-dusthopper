using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;
	public Color myColor;
	public bool showFPS = false;

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		if (!showFPS)
			return;
		
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 5 / 100;
		style.normal.textColor = new Color (1f, 1f, 0f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps) Time.time: {2:0.0000}   GameState.time: {3:0.0000}", msec, fps,Time.time,GameState.time);
		GUI.Label(rect, text, style);

		//print(string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps));
	}
}