using UnityEngine;
using System.Collections;

public class ComboBall : MonoBehaviour {
	public enum BallColors{RED, GREEN, BLUE, PINK, YELLOW, PURPLE};
	public BallColors ballColor;
	private bool markedCancel;
	public Material cancelMat;
	private Material normalMat;
	// Use this for initialization
	void Start () 
	{
		markedCancel = false;
		normalMat = GetComponent<Renderer>().material;
	}
	
	public bool IsMarkedCancel{get{return markedCancel;}}
	
	public void MarkCancel(bool cancel)
	{
		if(markedCancel == cancel)
		{
			return;
		}
		markedCancel = cancel;
		if(cancel)
		{
			GetComponent<Renderer>().material = cancelMat;
		}
		else
		{
			GetComponent<Renderer>().material = normalMat;
		}
	}
}
