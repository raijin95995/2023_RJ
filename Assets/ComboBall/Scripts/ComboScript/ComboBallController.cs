using UnityEngine;
using System.Collections;

public class CoordinatesInTable
{
	public int x;
	public int y;
}

[RequireComponent (typeof (ComboBall))]
public class ComboBallController : MonoBehaviour {
	protected ComboBall comboBall;
	
	protected Vector3 targetPosition;
	protected Vector3 dirVec = Vector3.zero;
	protected CoordinatesInTable coordinate;
	public CoordinatesInTable Coordinate {get { return coordinate;}}
	public float moveSpeed = 0.0f;
	public float delayTimeBeforeDrop = 1.0f;
	protected float currentDelayTimeBeforeDrop = 0.0f;
	public bool moving = false;
	public bool isInCombo = false;
	public bool isCheckedV = false;
	public bool isCheckedH = false;
	private bool isSelected = false;

	void Awake()
	{
		coordinate = new CoordinatesInTable();
		comboBall = this.gameObject.GetComponent<ComboBall>();
	}
		
	// if transition is false, move to target position instantly
	protected void SetPosition(float x, float y, float z, bool transition, float movingSpeed)
	{
		if(transition)
		{
			targetPosition = new Vector3(x, y, z);
			dirVec = (targetPosition - transform.position).normalized;
			moveSpeed = movingSpeed;
		}
		else
		{
			transform.position = new Vector3(x, y, z);
			targetPosition = transform.position;
			moving = false;
		}
	}

	public void MoveTo(float x, float y, float z, bool transition, float movingSpeed)
	{
		moving = true;
		SetPosition(x, y, z, transition, movingSpeed);
	}
	
	public void InitPosition(float x, float y, float z)
	{
		SetPosition(x, y, z, false, 0.0f);
	}

	public void SetCoordinate(int x, int y)
	{
		coordinate.x = x;
		coordinate.y = y;
		gameObject.name = string.Format("[{0}, {1}] {2}", coordinate.x, coordinate.y, GetColor().ToString());
	}

	public void RegisterDrop(float diff_y, float dropSpeed)
	{
		SetPosition(transform.position.x, transform.position.y - diff_y, transform.position.z, true, dropSpeed);
		currentDelayTimeBeforeDrop = 0.0f;
	}
	
	public void StartDrop()
	{
		moving = true;
	}
	
	public bool IsMoving {get {return moving;}}
	
	public bool IsMarkedCancel {get {return comboBall.IsMarkedCancel; }}
	
	public void MarkCancel(bool cancel)
	{
		comboBall.MarkCancel(cancel);
	}

	public void Selected(bool selected)
	{
		Color matColor = GetComponent<Renderer>().material.color;
		if(selected)
		{
			matColor.a = 0.5f;
			if(!isSelected)
			{
				transform.Translate(new Vector3(0.0f, 0.0f, -0.2f));
			}
		}
		else
		{
			matColor.a = 1.0f;
			if(isSelected)
			{
				transform.Translate(new Vector3(0.0f, 0.0f, 0.2f));
			}
		}
		isSelected = selected;
		GetComponent<Renderer>().material.color = matColor;
	}
	
	
	void Update()
	{
		if(moving && transform.position != targetPosition)
		{
			if(currentDelayTimeBeforeDrop > delayTimeBeforeDrop)
			{
				if((dirVec.x > 0 && transform.position.x >= targetPosition.x)
					|| (dirVec.x < 0 && transform.position.x <= targetPosition.x)
					|| (dirVec.y > 0 && transform.position.y >= targetPosition.y)
					|| (dirVec.y < 0 && transform.position.y <= targetPosition.y)
					|| (dirVec.z > 0 && transform.position.z >= targetPosition.z)
					|| (dirVec.z < 0 && transform.position.z <= targetPosition.z))
				{
					transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
					moving = false;
				}
				else
				{
					transform.Translate(dirVec * moveSpeed * Time.deltaTime);
				}
			}
			else
			{
				currentDelayTimeBeforeDrop += Time.deltaTime;
			}
		}
		else
		{
			moving = false;
		}
	}
	
	public ComboBall.BallColors GetColor()
	{
		return comboBall.ballColor;
	}
	
	public void DestroyBall()
	{
		Destroy(this.gameObject);
	}
}
