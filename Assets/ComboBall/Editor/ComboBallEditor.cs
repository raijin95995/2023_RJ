#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ComboBall))]
[CanEditMultipleObjects]
public class ComboBallEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if(GUILayout.Button("Apply Color Change", GUILayout.Width(200)))
		{
			for(int i = 0; i < targets.Length; i++)
			{
				ComboBall ball = targets[i] as ComboBall;
				string newName;
				AssetDatabase.Refresh();
				Material matNormal, matDark;
				switch(ball.ballColor)
				{
				case ComboBall.BallColors.BLUE:
					matNormal = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/BLUE.mat", typeof(Material)) as Material;
					matDark = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/BLUE_D.mat", typeof(Material)) as Material;
					newName = "BLUE";
					break;
				case ComboBall.BallColors.GREEN:
					matNormal = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/GREEN.mat", typeof(Material)) as Material;
					matDark = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/GREEN_D.mat", typeof(Material)) as Material;
					newName = "GREEN";
					break;
				case ComboBall.BallColors.PINK:
					matNormal = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/PINK.mat", typeof(Material)) as Material;
					matDark = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/PINK_D.mat", typeof(Material)) as Material;
					newName = "PINK";
					break;
				case ComboBall.BallColors.PURPLE:
					matNormal = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/PURPLE.mat", typeof(Material)) as Material;
					matDark = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/PURPLE_D.mat", typeof(Material)) as Material;
					newName = "PURPLE";
					break;
				case ComboBall.BallColors.RED:
					matNormal = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/RED.mat", typeof(Material)) as Material;
					matDark = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/RED_D.mat", typeof(Material)) as Material;
					newName = "RED";
					break;
				case ComboBall.BallColors.YELLOW:
					matNormal = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/YELLOW.mat", typeof(Material)) as Material;
					matDark = AssetDatabase.LoadAssetAtPath("Assets/ComboBall/Materials/ComboBalls/YELLOW_D.mat", typeof(Material)) as Material;
					newName = "YELLOW";
					break;
				default: 
					Debug.LogWarning("Color Not Registered in ComboBallEditor, please check");
					matNormal = ball.GetComponent<Renderer>().material;
					matDark = ball.cancelMat;
					newName = "NONE";
					break;
				}

				ball.GetComponent<Renderer>().material = matNormal;
				ball.cancelMat = matDark;
				string[] names = ball.name.Split(']');
				ball.name = names[0] + "] " + newName;
			}
			Resources.UnloadUnusedAssets();
		}
	}
}

#endif