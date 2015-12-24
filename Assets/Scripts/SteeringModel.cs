using UnityEngine;  // Mathf, Random;

public class SteeringModel 
{
	public static float laneStep = 1.0f;
	public static float laneLeft = -2.0f;
	public static float laneRight = 2.0f;
	public static float laneLeftOffRoad = -1.75f;
	public static float laneRightOffRoad = 1.75f;
	public static float laneLeftOnRoad = -1.0f;
	public static float laneRightOnRoad = 1.0f;

	public float cameraX = 0.0f;
	public float cameraXMultiplier = 0.5f; // 0.75f;
	public bool isChanging = false;
	public bool isInputLeft = false;
	public bool isInputRight = false;
	public bool isOffRoad = false;
	public bool isSignaling = false;
	public bool isVerbose = false;
	private bool wasInputLeft = false;
	private bool wasInputRight = false;
	public float speedBase = 5.0f;
	public float speed = 5.0f;
	public float x = 0.0f;
	public string state = "None";
	public float[] lanes = new float[]{-1.0f, 0.0f, 1.0f};

	private float laneTarget = 0.0f;
	private float xDifference = 0.0f;
	private float tolerance = 0.001f;

	public bool isFinished = false;
	public bool isCycleLane = false;
	public float cycleDirection = 1.0f;
	private float cycleDelay = 2.0f;
	private float cycleSignal = 1.0f;
	private float cycleWaited = 0.0f;

	public void Start(float lane) 
	{
		x = lane;
		laneTarget = lane;
		isOffRoad = false;
	}

	/**
	 * If was input left or right then ignore input this frame.  Perhaps multiple updates are being called per frame, since it is a fixed update.  Test case:  2015-10-31 In left lane.  Press right.  Expect move one lane.  Sometimes move two lanes.
	 */
	public float Update(float deltaSeconds) 
	{
		if (isCycleLane)
		{
			CycleLane(deltaSeconds);
		}
		if (isInputLeft && isInputRight) 
		{
		}
		else if (isInputLeft && !wasInputLeft) 
		{
			if (isVerbose) Debug.Log("SteeringModel.update: Left");
			laneTarget -= laneStep;
			isChanging = true;
		}
		else if (isInputRight && !wasInputRight) 
		{
			if (isVerbose) Debug.Log("SteeringModel.update: Right");
			laneTarget += laneStep;
			isChanging = true;
		}
		if (isChanging) 
		{
			laneTarget = Mathf.Max(laneLeft, Mathf.Min(laneRight, laneTarget));
			if (isFinished)
			{
				laneTarget = Mathf.Max(laneLeftOnRoad, Mathf.Min(laneRightOnRoad, laneTarget));
			}
			xDifference = laneTarget - x;
			x += xDifference * deltaSeconds * speed;
			if (Mathf.Abs(xDifference) <= tolerance 
			|| (xDifference < 0 && x < laneTarget)
			|| (0 < xDifference && laneTarget < x)) 
			{
				x = laneTarget;
				isChanging = false;
				state = "None";
			}
			if (x <= laneLeftOffRoad || laneRightOffRoad <= x)
			{
				isOffRoad = true;
			}
		}
		cameraX = x * cameraXMultiplier;
		wasInputLeft = isInputLeft;
		wasInputRight = isInputRight;
		return x;
	}

	/**
	 * At higher speed, change lane up to twice as fast.
	 * Test case:  2015-12-16 Level 7 or higher:  Drtizzle expects to change lane faster.
	 */
	public void SetSpeed(float speedForward)
	{
		float speedFactor = 0.5f;
		// 1.0f;
		speed = speedFactor * speedForward;
		speed = Mathf.Max(speed, speedBase);
	}
	
	private float MayFlipCycleDirection()
	{
		if ((laneTarget <= laneLeftOnRoad && cycleDirection < 0.0f)
		|| (laneRightOnRoad <= laneTarget && 0.0f < cycleDirection))
		{
			cycleDirection = -cycleDirection;
		}
		return cycleDirection;
	}

	public void StartCycleLane()
	{
		isCycleLane = true;
		isSignaling = false;
		if (Random.value < 0.5f)
		{
			cycleDirection = -cycleDirection;
		}
		MayFlipCycleDirection();
		cycleWaited = cycleDelay - cycleSignal;
	}

	/**
	 * Flip direction before target next lane.  Test case:  2015-12-23 Level 9.  Expect car never steers off road.
	 */
	public void CycleLane(float deltaSeconds)
	{
		if (isFinished || deltaSeconds <= 0.0f)
		{
			state = "None";
		}
		else if (!isChanging)
		{
			cycleWaited += deltaSeconds;
			if (cycleDelay <= cycleWaited)
			{
				cycleWaited -= cycleDelay;
				laneTarget += MayFlipCycleDirection();
				isChanging = true;
				state = "None";
				isSignaling = false;
			}
			else if (!isSignaling && cycleDelay - cycleSignal <= cycleWaited)
			{
				isSignaling = true;
				state = MayFlipCycleDirection() < 0.0 
					? "SignalLeft" : "SignalRight";
			}
			else if (!isSignaling)
			{
				state = "None";
			}
		}
	}
}
