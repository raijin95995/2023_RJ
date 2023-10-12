using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	private float maxTime;
	private float currentTime;
	public bool isCounting = false;
	public bool isTimeUp = false;
	public FillingShaderController filling;

	public enum CountingStyle
	{
		UP,
		DOWN
	};
	private CountingStyle style;

	public void SetupTimer(float maxTime, CountingStyle style)
	{
		this.maxTime = maxTime;
		this.style = style;
		ResetTimer();
	}

	public void ResetTimer()
	{
		isCounting = false;
		isTimeUp = false;
		if(style == CountingStyle.DOWN)
		{
			currentTime = maxTime;
		}
		else if(style == CountingStyle.UP)
		{
			currentTime = 0.0f;
		}
		filling.Init(currentTime, maxTime);
	}

	public void StartTiming()
	{
		isCounting = true;
	}

	public void PauseTiming()
	{
		isCounting = false;
	}

	// Update is called once per frame
	void Update () {
		if(isCounting && !isTimeUp)
		{
			if(style == CountingStyle.DOWN)
			{
				currentTime -= Time.deltaTime;
				if(currentTime <= 0.0f)
				{
					isTimeUp = true;
				}
			}
			else if(style == CountingStyle.UP)
			{
				currentTime += Time.deltaTime;
				if(currentTime >= maxTime)
				{
					isTimeUp = true;
				}
			}
			filling.SetValue(currentTime);
		}
	}
}
