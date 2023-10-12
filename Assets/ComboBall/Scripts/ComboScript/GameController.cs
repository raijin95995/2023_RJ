using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ComboMatrixController))]

public class GameController : MonoBehaviour {
	public static GameController Instance;
	public enum GameState{IDLE, COMBOING};
	private GameState currentState;
	private ComboBallController selectedBall;
	public float dragTimeLimit = 10.0f;
	public Timer timer;

	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () 
	{
		currentState = GameState.IDLE;
	}

	private void MainGameLoop()
	{
		switch(currentState)
		{
		case GameState.IDLE:
			InputHandler();
			break;
		case GameState.COMBOING:
			if(ComboMatrixController.Instance.getCurrentState() == ComboMatrixController.ComboState.IDLE)
			{
				currentState = GameState.IDLE;
			}
			break;
		}
	}
	/// <summary>
	/// TODO:
	/// 1. Drag and Drop
	/// 2. Ball swapping
	/// 3. Enter game as checking state, not cancelling state
	/// </summary>
	private void InputHandler()
	{
#if UNITY_ANDROID
		if(Input.touchCount > 0 && selectedBall == null)
		{
			Ray ray = ComboBallPanelCam.Instance.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo))
			{
				selectedBall = hitInfo.collider.GetComponent<ComboBallController>();
				if(selectedBall != null)
				{
					ComboMatrixController.Instance.ComboBallSelect(selectedBall);
					timer.SetupTimer(dragTimeLimit, Timer.CountingStyle.DOWN);
				}
			}
		}
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && selectedBall != null)
		{
			Vector3 touchPos = Input.GetTouch(0).position;
			touchPos.z = -ComboBallPanelCam.Instance.transform.position.z;
			Vector3 screenPos = ComboBallPanelCam.Instance.ScreenToWorldPoint(touchPos);
			if(ComboMatrixController.Instance.IsInBoundary(screenPos) && !timer.isTimeUp)
			{
				ComboMatrixController.Instance.ComboBallDrag(selectedBall, screenPos);
			}
			else
			{
				StartCombo();
			}
		}
		if(Input.touchCount == 0 && selectedBall != null)
		{
			StartCombo();
		}
#elif UNITY_IPHONE

#else
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = ComboBallPanelCam.Instance.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo))
			{
				selectedBall = hitInfo.collider.GetComponent<ComboBallController>();
				if(selectedBall != null)
				{
					ComboMatrixController.Instance.ComboBallSelect(selectedBall);
					timer.SetupTimer(dragTimeLimit, Timer.CountingStyle.DOWN);
				}
			}
		}
		if(Input.GetMouseButton(0) && selectedBall != null)
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = -ComboBallPanelCam.Instance.transform.position.z;
			Vector3 screenPos = ComboBallPanelCam.Instance.ScreenToWorldPoint(mousePos);
			if(ComboMatrixController.Instance.IsInBoundary(screenPos) && !timer.isTimeUp)
			{
				ComboMatrixController.Instance.ComboBallDrag(selectedBall, screenPos);
			}
			else
			{
				StartCombo();
			}
		}
		if(Input.GetMouseButtonUp(0) && selectedBall != null)
		{
			StartCombo();
		}

#endif
	}

	private void StartCombo()
	{
		ComboMatrixController.Instance.ComboBallRelease(selectedBall);
		selectedBall = null;
		currentState = GameState.COMBOING;
		timer.gameObject.SetActive(false);
		timer.ResetTimer();
	}

	// Update is called once per frame
	void Update () 
	{
		MainGameLoop ();
	}
}
