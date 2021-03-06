using UnityEngine;  // Mathf

public class Vehicle
{
	public float speed = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;
	public float finishZ = 0.0f;
	public float stopZ = 0.0f;
	public int index;

	public Drive drive = new Drive();
	public SteeringModel steering = new SteeringModel();
	public float collisionRadius = 0.2f;
	public float collisionSpeedMultiplier = 0.05f;
						// 0.0f;
	public bool isColliding;
	public bool isRankUpNow;
	public int collisionCount = 0;
	public bool isCollidingNow;
	private bool wasColliding;
	private Vehicle passee;
	private float now = 0.0f;
	private float rankUpInterval = 0.0f;
	private float rankUpPreviously = 0.0f;

	public bool IsUpdateRank(Vehicle[] vehicles)
	{
		Vehicle other;
		int place;
		int next = index;
		SteeringModel passee = null;
		if (1 <= index)
		{
			place = index - 1;
			other = vehicles[place];
			if (other.z < z)
			{
				next = place;
				vehicles[next] = this;
				vehicles[index] = other;
				passee = other.steering;
			}
		}
		if (next == index && index < vehicles.Length - 1)
		{
			place = index + 1;
			other = vehicles[place];
			if (z < other.z)
			{
				next = place;
				vehicles[next] = this;
				vehicles[index] = other;
			}
		}
		steering.passes(passee);
		bool isRankChangingNow = index != next;
		isRankUpNow = next < index;
		if (isRankUpNow)
		{
			rankUpInterval = now - rankUpPreviously;
			rankUpPreviously = now;
			Debug.Log("Vehicle.IsUpdateRank: " + rankUpInterval);
		}
		index = next;
		return isRankChangingNow;
	}

	public float SpeedFactor()
	{
		float speedBase = 2.0f;
		// 5.0f;
		float power = 0.25f;
		// 0.5f;
		return Mathf.Max(0.5f, Mathf.Pow(speed / speedBase, power));
	}

	public bool IsColliding(Vehicle other)
	{
		float intersect = collisionRadius + other.collisionRadius;
		isColliding = (other.speed < speed) &&
			(Mathf.Abs(z - other.z) < intersect
			&& Mathf.Abs(x - other.x) < intersect);
		return isColliding;
	}

	/**
	 * If competitor is moving faster, do not collide.
	 * Test case:  2015-12-20 Level 7 or higher.  Crash.  Blobo expects not to crash again into next car.
	 */
	public bool UpdateCollision(Vehicle[] vehicles)
	{
		wasColliding = isColliding;
		isColliding = false;
		int ahead = index - 1;
		int behind = index + 1;
		int collisionIndex = -1;
		Vehicle other;
		if (!isColliding && 0 <= ahead)
		{
			other = vehicles[ahead];
			if (other.speed < speed)
			{
				isColliding = IsColliding(other);
				if (isColliding) {
					collisionIndex = ahead;
				}
			}
		}
		if (!isColliding && behind < vehicles.Length)
		{
			other = vehicles[behind];
			if (other.speed < speed)
			{
				isColliding = IsColliding(other);
				if (isColliding) {
					collisionIndex = behind;
				}
			}
		}
		isCollidingNow = !wasColliding && isColliding;
		if (isCollidingNow)
		{
			speed *= collisionSpeedMultiplier;
			drive.derivatives[0] = speed;
			collisionCount++;
			Debug.Log("Vehicle.UpdateCollision: Collided.  New speed " + speed + ". Collision index " + collisionIndex);
		}
		return isCollidingNow;
	}

	public bool IsFinishingNow()
	{
		bool isNow = !steering.isFinished;
		if (isNow)
		{
			// Debug.Log("IsFinishingNow");
			steering.isFinished = true;
			drive.rates = drive.ratesFinish;
			steering.isCycleLane = false;
		}
		return isNow;
	}

	public bool HasStopped()
	{
		return steering.isFinished && speed < 0.25f;
	}

	public void Update(float deltaSeconds)
	{
		now += deltaSeconds;
		x = steering.Update(deltaSeconds);
		if (steering.isFinished)
		{
			drive.derivatives[2] = stopZ - z;
		}
		z += drive.Update(deltaSeconds);
		speed = drive.derivatives[0];
		steering.SetSpeed(speed);
	}
}
