using UnityEngine;
using System.Collections;

public class ComboBallPanelCam : MonoBehaviour {

	public static Camera Instance;

	void Awake()
	{
		Instance = this.GetComponent<Camera>();
		if(Instance == null)
		{
			Debug.LogError("Combo Ball Panel Camera Not Found");
		}
	}
}
