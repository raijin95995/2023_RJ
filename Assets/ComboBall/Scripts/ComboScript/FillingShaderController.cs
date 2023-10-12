using UnityEngine;
using System.Collections;

public class FillingShaderController : MonoBehaviour {
	private float maxValue = 100.0f;
	private float currentValue = 0.0f;
	private float targetValue = 0.0f;
	private float diffFromTarget = 0.0f;
	private float speed = 0.0f;
	private float finishTime = 0.5f;
	private bool inited = false;
	public Color emptyColor = Color.white;
	public Color fullColor = Color.white;
	
	public void Init(float currentVal, float maxVal)
	{
		if(maxVal == 0.0f)
		{
			maxValue = 1.0f;
		}
		else
		{
			maxValue = maxVal;
		}
		currentValue = currentVal;
		inited = true;
		GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(emptyColor, fullColor, currentValue / maxValue));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(targetValue != currentValue)
		{
			currentValue += speed * Time.deltaTime;
			if((diffFromTarget > 0 && targetValue - currentValue <= 0)
				|| (diffFromTarget < 0 && targetValue - currentValue >= 0))
			{
				currentValue = targetValue;
			}
			GetComponent<Renderer>().material.SetFloat("_FillUpTo", Mathf.Min(1.0f, currentValue / maxValue));
			GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(emptyColor, fullColor, currentValue / maxValue));
		}
	}
	
	public void SetValue(float targetVal, bool animated = true)
	{
		if(!inited)
		{
			Debug.LogWarning("Meter not initialized", this);
		}
		targetValue = targetVal;
		if(animated)
		{
			diffFromTarget = targetValue - currentValue;
			speed = diffFromTarget / finishTime;
		}
		else
		{
			currentValue = targetValue;
			GetComponent<Renderer>().material.SetFloat("_FillUpTo", Mathf.Min(1.0f, currentValue / maxValue));
			GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(emptyColor, fullColor, currentValue / maxValue));
		}
	}
}
