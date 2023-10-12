using UnityEngine;
using System.Collections;

public class ComboMatrix : MonoBehaviour {
	
	public float gridWidth = 0.8f;
	public float gridHeight = 0.8f;
	public int numOfRow = 5;
	public int numOfCol = 5;
	// instead of 2D array, try to use 2D lists
	private GameObject[,] ballMatrix;
	
	void Awake()
	{
		// Has to put here in case controller initialize before this object
		ballMatrix = new GameObject[numOfRow, numOfCol];
	}
	
	public GameObject GetComboBall(int row, int col)
	{
		return ballMatrix[row, col];
	}

	public ComboBallController GetComboBallController(int row, int col)
	{
		ComboBallController result = ballMatrix[row, col].GetComponent<ComboBallController>();
		if(result == null)
		{
			Debug.LogWarning("ComboBallController Not Found");
		}
		return result;
	}
	
	public void SetComboBall(int row, int col, ComboBallController ball)
	{
		ballMatrix[row, col] = ball.gameObject;
		ball.SetCoordinate(col, row);
	}

	public void SwapComboBall(ComboBallController ballA, ComboBallController ballB)
	{
		CoordinatesInTable tmpCoor = new CoordinatesInTable();
		tmpCoor.x = ballA.Coordinate.x;
		tmpCoor.y = ballA.Coordinate.y;
		SetComboBall(ballB.Coordinate.y, ballB.Coordinate.x, ballA);
		SetComboBall(tmpCoor.y, tmpCoor.x, ballB);
	}
}
