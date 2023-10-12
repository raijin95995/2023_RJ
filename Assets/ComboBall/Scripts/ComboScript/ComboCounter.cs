/// <summary>
/// Combo Counter
/// Include details of combos:
/// 1. Color
/// 2. Number of balls
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboDetails
{
	public ComboBall.BallColors color;
	public int numberOfBalls = 0;

	public void Reset()
	{
		numberOfBalls = 0;
	}
}

public class ComboCounter : MonoBehaviour 
{
	public static ComboCounter Instance;
	public BallCom ballCom ;

	void Awake()
	{
		Instance = this;
		comboDetails = new ComboDetails();
		ballCom = GameObject.Find("BallCom").GetComponent<BallCom>();
	}

	private int comboCount = 0;
	private ComboDetails comboDetails;
	public TextMesh comboCountText;
	public TextMesh comboColorText;
	public TextMesh comboBallCountText;

	public void AddCombo(ComboBall.BallColors color, int numOfBalls)
	{
		comboCount++;
		ballCom.combo++;
		comboDetails.color = color;
		comboDetails.numberOfBalls = numOfBalls;
		comboCountText.text = string.Format("Combo Count: {0}", comboCount);
		comboColorText.text = string.Format("Color:\t\t{0}", comboDetails.color);
		switch(comboDetails.color)
		{
		case ComboBall.BallColors.BLUE: //前進
			comboColorText.color = Color.blue;
			ballCom.forwardCount++;
			break;
		case ComboBall.BallColors.GREEN: //後退
			comboColorText.color = Color.green;
			ballCom.backCount++;
			break;
		case ComboBall.BallColors.PINK: //近攻擊
			comboColorText.color = new Color(1.0f, 95.0f/255.0f, 246.0f/255.0f);
			ballCom.nearAtkCount++;
			break;
		case ComboBall.BallColors.PURPLE: //遠攻擊
			comboColorText.color = new Color(101.0f/255.0f, 0.0f, 126.0f/255.0f);
			ballCom.farAtkCount++;
			break;
		case ComboBall.BallColors.RED: //補血
			comboColorText.color = Color.red;
			ballCom.heathCount++;
			break;
		case ComboBall.BallColors.YELLOW: //防禦
			comboColorText.color = Color.yellow;
			ballCom.defendCount++;
			break;
		default:

			break;
		}
		comboBallCountText.text = string.Format("Ball Count:\t\t{0}", comboDetails.numberOfBalls);
	}

	public void ResetComboCounter()
	{
		comboCount = 0;
		comboDetails.Reset();

	}

}
