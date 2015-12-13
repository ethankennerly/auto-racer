using UnityEngine;  // Mathf, Random;

public class SteeringModel 
{
	public static float laneStep = 1.0f;
	public static float laneLeft = -1.0f;
	public static float laneRight = 1.0f;

	public float cameraX = 0.0f;
	public float cameraXMultiplier = 0.5f; // 0.75f;
	public bool isChanging = false;
	public bool isInputLeft = false;
	public bool isInputRight = false;
	public bool isVerbose = false;
	private bool wasInputLeft = false;
	private bool wasInputRight = false;
	public float speed = 5.0f;
	public float x = 0.0f;
	public string state = "None";

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
		}
		cameraX = x * cameraXMultiplier;
		wasInputLeft = isInputLeft;
		wasInputRight = isInputRight;
		return x;
	}
	
	private void mayFlipCycleDirection()
	{
		if ((laneTarget <= laneLeft && cycleDirection < 0.0f)
		|| (laneRight <= laneTarget && 0.0f < cycleDirection))
		{
			cycleDirection = -cycleDirection;
		}
	}

	public void StartCycleLane()
	{
		isCycleLane = true;
		if (Random.value < 0.5f)
		{
			cycleDirection = -cycleDirection;
		}
		mayFlipCycleDirection();
		cycleWaited = cycleDelay - cycleSignal;
	}

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
				laneTarget += cycleDirection;
				isChanging = true;
				state = "None";
				mayFlipCycleDirection();
			}
			else if (cycleDelay - cycleSignal <= cycleWaited)
			{
				state = cycleDirection < 0.0 ? "SignalLeft" : "SignalRight";
			}
			else
			{
				state = "None";
			}
		}
	}
}
