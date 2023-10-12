using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof (ComboMatrix))]
public class ComboMatrixController : MonoBehaviour {
	public static ComboMatrixController Instance;
	private ComboMatrix comboMatrix;
	public GameObject[] comboBallPrefabs;
	
	public enum ComboState{IDLE, CHECKING, CANCELLING, DROPPING};
	public ComboState currentState = ComboState.IDLE;		// need to be protected
	
	public enum ComboCancellingRule{Adj2, Adj3, AdjGT3};
	public ComboCancellingRule cancelRule = ComboCancellingRule.Adj2;
	
	public int comboCount;
	public int comboCount_Page;
	public float cancelDelay = 1.0f;
	public float cancelInterval = 0.5f;

	public float dropSpeed = 1.0f;
	public float swapSpeed = 5.0f;

	private bool isDragging = false;
	public bool IsDragging {get {return isDragging;} }

	private Queue<Combo> comboListOfCurrentPage;

	void Awake()
	{
		Instance = this;
	}
	
	void Start()
	{
		comboMatrix = this.gameObject.GetComponent<ComboMatrix>();
		comboListOfCurrentPage = new Queue<Combo>();
		GameObject[] compareList = new GameObject[comboBallPrefabs.Length];
		GameObject compareListParent = new GameObject();
		compareListParent.transform.parent = this.transform;
		for(int i = 0; i < comboBallPrefabs.Length; i++)
		{
			compareList[i] = Instantiate(comboBallPrefabs[i]) as GameObject;
			compareList[i].SetActive(false);
			compareList[i].transform.parent = compareListParent.transform;
		}
		for(int i = 0; i < comboMatrix.numOfRow; i++)
		{
			for(int j = 0; j < comboMatrix.numOfCol; j++)
			{
				if(comboBallPrefabs.Length > 0)
				{
					// Random Generate First Page
					// Avoid combo on initialization
					int colorIdx = 0;
					bool noCombo = false;
					while(!noCombo)
					{
						int horizontalSameColor = 0;
						int verticalSameColor = 0;
						colorIdx = UnityEngine.Random.Range(0, comboBallPrefabs.Length);
						for(int k = j - 1; k >= 0; k--)
						{
							ComboBallController horizontalPreviousBall = comboMatrix.GetComboBallController(i, k);
							if(horizontalPreviousBall.GetColor().Equals(compareList[colorIdx].GetComponent<ComboBallController>().GetColor()))
							{
								horizontalSameColor++;
							}
							else
							{
								break;
							}
						}
						for(int l = i - 1; l >= 0; l--)
						{
							ComboBallController verticalPreviousBall = comboMatrix.GetComboBallController(l, j);
							if(verticalPreviousBall.GetColor().Equals(compareList[colorIdx].GetComponent<ComboBallController>().GetColor()))
							{
								verticalSameColor++;
							}
							else
							{
								break;
							}
						}
						switch(cancelRule)
						{
						case ComboCancellingRule.Adj2:
							if(verticalSameColor < 1 && horizontalSameColor < 1)
							{
								noCombo = true;
							}
							break;
						case ComboCancellingRule.Adj3:
							if(verticalSameColor < 2 && horizontalSameColor < 2)
							{
								noCombo = true;
							}
							break;
						case ComboCancellingRule.AdjGT3:
							if(verticalSameColor < 3 && horizontalSameColor < 3)
							{
								noCombo = true;
							}
							break;
						}
					}
					GameObject comboBallObj = Instantiate(comboBallPrefabs[colorIdx]) as GameObject;
					comboBallObj.transform.parent = this.transform;
					// Assign the generated combo balls into the matrix
					// Update the position of the balls through ball controller
					ComboBallController comboBallCtrl = comboBallObj.GetComponent<ComboBallController>();
					comboMatrix.SetComboBall(i, j, comboBallCtrl);
					comboBallCtrl.InitPosition(j * comboMatrix.gridHeight, i * comboMatrix.gridWidth, 0.0f);
				}
			}
		}
		Destroy(compareListParent);
		comboCount = 0;
		comboCount_Page = 0;
		currentState = ComboState.IDLE;
		ComboCounter.Instance.ResetComboCounter();
	}
	
	
	/// <summary>
	/// Current Game Rules:
	/// Drag one Combo Ball and swap with any balls adj to it,
	/// can make more than 1 swap within the time limit
	/// then scan the whole matrix, if any adjacent balls have the same color, mark both as canceled, for each group, comboCount++
	/// destroy all balls marked as canceled
	/// for each run, comboCount_Page++
	/// generate new balls to fill in the gaps after balls destruction
	/// check the new matrix and loop until no adjacent balls have same color
	/// 
	/// Possible cancel rules:
	/// 1. 2 or more Adjacent balls with same color
	/// 2. 3 balls with same color in a row or a column
	/// </summary>
	void Update()
	{
		switch(currentState)
		{
		case ComboState.IDLE:
			break;
		case ComboState.CHECKING:
			CheckAvaliableCombo();
			break;
		case ComboState.CANCELLING:
			break;
		case ComboState.DROPPING:
			if(CheckDropingDone())
			{
				currentState = ComboState.CHECKING;
			}
			break;
		}
	}

	private bool CheckDropingDone()
	{
		for (int i = 0; i < comboMatrix.numOfRow; i++)
		{
			for (int j = 0; j < comboMatrix.numOfCol; j++)
			{
				ComboBallController ballCtrl = comboMatrix.GetComboBall(i, j).GetComponent<ComboBallController>();
				if(ballCtrl.IsMoving)
				{
					return false;
				}
			}
		}
		return true;
	}
	
	/// <summary>
	/// Todo:
	/// 1. check combo count
	/// </summary>
	private void CheckAvaliableCombo()
	{
		comboListOfCurrentPage.Clear();
		comboListOfCurrentPage.TrimExcess();
		List<List<ComboBallController>> comboList = new List<List<ComboBallController>> ();
		/// <summary>
		/// Start from bottom left corner
		/// Check combo from left to right, bottom to top
		/// Add new combo detected to a list, merge combos if they are of the same color and adj/overlap with each other
		/// </summary>
		for(int y = 0; y < comboMatrix.numOfRow; y++)
		{
			for(int x = 0; x < comboMatrix.numOfCol; x++)
			{
				ComboBallController ballCtrl = comboMatrix.GetComboBallController(y, x);
				if(ballCtrl.isCheckedH && ballCtrl.isCheckedV)	// Skip when ball is alread checked
				{
					continue;
				}
				List<ComboBallController> currentCombo = CheckIsCombo(x, y, ballCtrl.GetColor(), true, true);
				if(currentCombo.Count > 0)
				{
					if(comboList.Count <= 0)
					{
						comboList.Add(currentCombo);
					}
					else
					{
						bool isConnected = false;
						for(int i = 0; i < comboList.Count; i++)
						{
							if(ConnectionTest(currentCombo, comboList[i]))
							{
								isConnected = true;
								comboList[i].AddRange(currentCombo);
							}
						}
						if(!isConnected)
						{
							comboList.Add(currentCombo);
						}
					}
				}
			}
		}
		for(int i = 0; i < comboList.Count; i++)
		{
			Combo combo = new Combo(comboList[i][0].GetColor(), comboList[i]);
			if(combo.NumOfBalls > 0)
			{
				Debug.Log("Combo Detected: Color: " + combo.ComboColor + " NumOfBalls: " + combo.NumOfBalls);
			}
			combo.MarkCancel(true);
			comboListOfCurrentPage.Enqueue(combo);
		}
		if(comboListOfCurrentPage.Count == 0)
		{
			ComboCounter.Instance.ResetComboCounter();
			currentState = ComboState.IDLE;
			for(int y = 0; y < comboMatrix.numOfRow; y++)
			{
				for(int x = 0; x < comboMatrix.numOfCol; x++)
				{
					ComboBallController ballCtrl = comboMatrix.GetComboBallController(y, x);
					ballCtrl.isCheckedH = false;
					ballCtrl.isCheckedV = false;
				}
			}
		}
		else
		{
			currentState = ComboState.CANCELLING;
			StartCoroutine(BallCancelHandler());
		}
	}

	private List<ComboBallController> CheckIsCombo(int posX, int posY, ComboBall.BallColors color, bool checkHorizontal, bool checkVertical)
	{
		List<ComboBallController> currentCombo = new List<ComboBallController>();
		List<ComboBallController> horizontalCombo = new List<ComboBallController>();
		List<ComboBallController> verticalCombo = new List<ComboBallController>();
		if(checkHorizontal)
		{
			// Add current ball into temp combo list
			horizontalCombo.Add(comboMatrix.GetComboBallController(posY, posX));
			// Check balls at right
			for(int x = posX + 1; x < comboMatrix.numOfCol; x++)
			{
				ComboBallController ballCtrl = comboMatrix.GetComboBallController(posY, x);
				if(!ballCtrl.isCheckedH && color.Equals(ballCtrl.GetColor()))
				{
					ballCtrl.isCheckedH = true;
					horizontalCombo.Add(ballCtrl);
				}
				else
				{
					break;
				}
			}
		}
		if(checkVertical)
		{
			verticalCombo.Add(comboMatrix.GetComboBallController(posY, posX));
			for(int y = posY + 1; y < comboMatrix.numOfRow; y++)
			{
				ComboBallController ballCtrl = comboMatrix.GetComboBallController(y, posX);
				if(!ballCtrl.isCheckedV && color.Equals(ballCtrl.GetColor()))
				{
					ballCtrl.isCheckedV = true;
					verticalCombo.Add(ballCtrl);
				}
				else
				{
					break;
				}
			}
		}
		switch(cancelRule)
		{
		case ComboCancellingRule.Adj2:
			if(horizontalCombo.Count >= 2)
			{
				currentCombo.AddRange(horizontalCombo);
			}
			if(verticalCombo.Count >= 2)
			{
				//Union
				currentCombo.AddRange(verticalCombo);
			}
			break;
		case ComboCancellingRule.Adj3:
			if(horizontalCombo.Count >= 3)
			{
				currentCombo.AddRange(horizontalCombo);
			}
			if(verticalCombo.Count >= 3)
			{
				//Union
				currentCombo.AddRange(verticalCombo);
			}
			break;
		case ComboCancellingRule.AdjGT3:
			if(horizontalCombo.Count > 3)
			{
				currentCombo.AddRange(horizontalCombo);
			}
			if(verticalCombo.Count > 3)
			{
				//Union
				currentCombo.AddRange(verticalCombo);
			}
			break;
		}
		return currentCombo;
	}

	private IEnumerator BallCancelHandler()
	{
		yield return new WaitForSeconds(cancelDelay);
		for (int j = 0; j < comboMatrix.numOfCol; j++)
		{
			int numOfCanceledBallInCol = 0;
			List<GameObject> generatedComboBalls = new List<GameObject>();
			for (int i = 0; i < comboMatrix.numOfRow; i++)
			{
				ComboBallController ballCtrl = comboMatrix.GetComboBallController(i, j);
				if(ballCtrl.IsMarkedCancel)
				{
					numOfCanceledBallInCol++;
					if(comboBallPrefabs.Length > 0)
					{
						// Random Generate First Page
						int colorIdx = UnityEngine.Random.Range(0, comboBallPrefabs.Length);
						GameObject comboBallObj = Instantiate(comboBallPrefabs[colorIdx]) as GameObject;
						comboBallObj.transform.parent = this.transform;
						// Update the position of the balls through ball controller
						ComboBallController comboBallCtrl = comboBallObj.GetComponent<ComboBallController>();
						comboBallCtrl.InitPosition(j * comboMatrix.gridWidth, (comboMatrix.numOfRow + numOfCanceledBallInCol - 1) * comboMatrix.gridHeight, 0.0f);
						generatedComboBalls.Add(comboBallObj);
					}
				}
				else
				{
					// Rest ball states
					ballCtrl.isCheckedH = false;
					ballCtrl.isCheckedV = false;
					ballCtrl.isInCombo = false;
					if(numOfCanceledBallInCol > 0)
					{
						comboMatrix.SetComboBall(i - numOfCanceledBallInCol, j, ballCtrl);
						ballCtrl.RegisterDrop(numOfCanceledBallInCol * comboMatrix.gridHeight, dropSpeed);
					}
				}
			}
			for (int i = 0; i < numOfCanceledBallInCol; i++)
			{
				ComboBallController comboBallCtrl = generatedComboBalls[i].GetComponent<ComboBallController>(); 
				comboMatrix.SetComboBall(comboMatrix.numOfRow - numOfCanceledBallInCol + i, j, comboBallCtrl);
				comboBallCtrl.RegisterDrop(numOfCanceledBallInCol * comboMatrix.gridHeight, dropSpeed);
			}
		}

		while(comboListOfCurrentPage.Count > 1)
		{
			Combo currCombo = comboListOfCurrentPage.Dequeue();
			ComboCounter.Instance.AddCombo(currCombo.ComboColor, currCombo.NumOfBalls);
			currCombo.CancelCombo();
			yield return new WaitForSeconds(cancelInterval);
		}
		if(comboListOfCurrentPage.Count == 1)
		{
			Combo currCombo = comboListOfCurrentPage.Dequeue();
			ComboCounter.Instance.AddCombo(currCombo.ComboColor, currCombo.NumOfBalls);
			currCombo.CancelCombo();
		}


		for ( int i = 0; i < comboMatrix.numOfRow; i++ )
		{
			for ( int j = 0; j < comboMatrix.numOfCol; j++)
			{
				comboMatrix.GetComboBallController(i, j).StartDrop();
			}
		}
		currentState = ComboState.DROPPING;
	}

	
	public ComboState getCurrentState()
	{
		return currentState;
	}

	public void ComboBallSelect(ComboBallController selectedBall)
	{
		selectedBall.Selected(true);
	}

	public void ComboBallDrag(ComboBallController selectedBall, Vector3 currPos)
	{
		selectedBall.transform.position = new Vector3(currPos.x, currPos.y, selectedBall.transform.position.z);
		Ray ray = new Ray(selectedBall.transform.position, Vector3.forward);
		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo))
		{
			ComboBallController swapBall = hitInfo.collider.GetComponent<ComboBallController>();
			if(swapBall != null && !swapBall.IsMoving)
			{
				SwapBalls(selectedBall, swapBall);
			}
		}
	}

	public void ComboBallRelease(ComboBallController selectedBall)
	{
		selectedBall.Selected(false);
		selectedBall.MoveTo((float)selectedBall.Coordinate.x * comboMatrix.gridWidth, 
		                    (float)selectedBall.Coordinate.y * comboMatrix.gridHeight, 
		                    selectedBall.transform.position.z, false, 0.0f);
		isDragging = false;
		currentState = ComboState.CHECKING;
	}

	public void SwapBalls(ComboBallController selectedBall, ComboBallController ballGoingToBeSwapped)
	{
		if(!isDragging)
		{
			isDragging = true;
		}
		if(!GameController.Instance.timer.isCounting)
		{
			GameController.Instance.timer.gameObject.SetActive(true);
			GameController.Instance.timer.StartTiming();
		}
		comboMatrix.SwapComboBall(selectedBall, ballGoingToBeSwapped);
		ballGoingToBeSwapped.MoveTo((float)ballGoingToBeSwapped.Coordinate.x * comboMatrix.gridWidth, 
		                            (float)ballGoingToBeSwapped.Coordinate.y * comboMatrix.gridHeight, 
		                            ballGoingToBeSwapped.transform.position.z, true, swapSpeed);
	}

	public bool IsInBoundary(Vector3 pos)
	{
		bool result = true;
		if(pos.x < 0.0f - comboMatrix.gridWidth * 0.5f
		   || pos.x > comboMatrix.numOfCol * comboMatrix.gridWidth - comboMatrix.gridWidth * 0.5f
		   || pos.y < 0.0f - comboMatrix.gridHeight * 0.5f
		   || pos.y > comboMatrix.numOfRow * comboMatrix.gridHeight - comboMatrix.gridHeight * 0.5f)
		{
			result = false;
		}
		return result;
	}

	private bool ConnectionTest(List<ComboBallController> currentCombo, List<ComboBallController> newCombo)
	{
		if(currentCombo == null || newCombo == null || currentCombo.Count <= 0 || newCombo.Count <= 0)
			return false;
		if (currentCombo [0].GetColor () != newCombo [0].GetColor ())
			return false;
		for(int i = 0; i < newCombo.Count; i++)
		{
			for(int j = 0; j < currentCombo.Count; j++)
			{
				// if adj or overlap, return true
				if(newCombo[i].Coordinate.x == currentCombo[j].Coordinate.x)
				{
					if(newCombo[i].Coordinate.y >= currentCombo[j].Coordinate.y - 1 && newCombo[i].Coordinate.y <= currentCombo[j].Coordinate.y + 1)
						return true;
				}
				if(newCombo[i].Coordinate.y == currentCombo[j].Coordinate.y)
				{
					if(newCombo[i].Coordinate.x >= currentCombo[j].Coordinate.x - 1 && newCombo[i].Coordinate.x <= currentCombo[j].Coordinate.x + 1)
						return true;
				}
			}
		}
		return false;
	}

	private void RevertCheckedCombo(List<ComboBallController> iList)
	{
		if (iList == null || iList.Count <= 0)
			return;
		for(int i = 0; i < iList.Count; i++)
		{
			iList[i].isCheckedH = false;
			iList[i].isCheckedV = false;
		}
	}
}

public class Combo
{
	private ComboBall.BallColors comboColor;
	public ComboBall.BallColors ComboColor {get{return comboColor;}}
	private List<ComboBallController> comboElements;
	public int NumOfBalls {get{return comboElements.Count;}}

	public Combo(ComboBall.BallColors color, List<ComboBallController> combo)
	{
		comboColor = color;
		comboElements = new List<ComboBallController>();
		comboElements.AddRange(combo);
		comboElements = comboElements.Distinct().ToList();
		foreach(ComboBallController ctrl in comboElements)
		{
			ctrl.isInCombo = true;
		}
	}

	public void CancelCombo()
	{
		foreach(ComboBallController ctrl in comboElements)
		{
			ctrl.DestroyBall();
		}
		comboElements.Clear();
		comboElements.TrimExcess();
	}

	public void MarkCancel(bool cancel)
	{
		foreach(ComboBallController ctrl in comboElements)
		{
			ctrl.MarkCancel(cancel);
		}
	}
}

