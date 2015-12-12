using UnityEngine;  // Mathf

public class Vehicle
{
	public float speed = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	public Drive drive = new Drive();
	public float collisionRadius = 0.2f;
	public float collisionSpeedMultiplier = 0.05f;
	public bool isColliding;
	public bool isCollidingNow;
	private bool wasColliding;

	public void Start()
	{
	}

	public bool IsColliding(Vehicle other)
	{
		float intersect = collisionRadius + other.collisionRadius;
		isColliding = (other.speed < speed) &&
			(Mathf.Abs(z - other.z) < intersect
			|| Mathf.Abs(x - other.x) < intersect);
		return isColliding;
	}

	public bool UpdateCollision(Vehicle[] vehicles, int index)
	{
		wasColliding = isColliding;
		int ahead = index - 1;
		int behind = index + 1;
		if (0 <= ahead)
		{
			isColliding = IsColliding(vehicles[ahead]);
		}
		if (!isCollidingNow && behind < vehicles.Length)
		{
			isColliding = IsColliding(vehicles[behind]);
		}
		isCollidingNow = !wasColliding && isColliding;
		if (isCollidingNow)
		{
			speed *= collisionSpeedMultiplier;
			// Debug.Log("Vehicle.UpdateCollision: speed " + speed);
		}
		return isCollidingNow;
	}

	public void Update(float deltaSeconds)
	{
		z += drive.Update(deltaSeconds);
		speed = drive.derivatives[0];
	}
}
