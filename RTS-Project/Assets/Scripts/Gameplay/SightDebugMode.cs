using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Sight))]
public class SightDebugMode : Editor
{
	void OnSceneGUI()
	{
		Sight fow = (Sight)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.detectionCol.radius * 1.2f);
		Vector3 viewAngleA = fow.DirFromAngle(-fow.fovAngle / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle(fow.fovAngle / 2, false);

		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.detectionCol.radius * 1.2f);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.detectionCol.radius * 1.2f);

		Handles.color = Color.red;

		if (fow.objectInSight)
		{
			Handles.DrawLine(fow.transform.position, fow.gameObject.transform.position);
		}

	}

}
