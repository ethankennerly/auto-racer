using UnityEngine;  // Random, Mathf

public class Race
{
	public static int level = 0;

	public int competitorCount;
	public int vehicleCount = 10;

	public float playerSpeed = 0.5f;
	public float competitorSpeed = 0.0f;
	public float competitorSpeedDerivative = 10.0f; // 5.0f;
	public float competitorStart = 5.0f;
	public float finishZ = 260.0f;
	public float cameraZ;
	public float postZ = 15.0f;
	public float carPerCycleLane = 	// 2.0f; 
					// 4.0f; 
					8.0f;
	public int levelCount;

	private int[] vehicleCounts = {
		50,
		60,
		70,
		80,
		90,
		100,
		110,
		120,
		130,
		140
	};

	private float[] topSpeeds = {
		80.0f,
		90.0f,
		100.0f,
		110.0f,
		120.0f,
		130.0f,
		140.0f,
		150.0f,
		160.0f,
		170.0f
	};

	private void StartIsShort(Vehicle player, bool isShort)
	{
		if (isShort)
		{
			finishZ = 20.0f;
			vehicleCount = level + 2;
			player.drive.rates[0] = 5.0f;
		}
		else
		{
			finishZ = 260.0f;
		}
	}

	public void Start(Vehicle player, bool isShort)
	{
		levelCount = topSpeeds.Length;
		vehicleCount = vehicleCounts[level];
		player.drive.derivatives[2] = topSpeeds[level];
		StartIsShort(player, isShort);
		competitorCount = vehicleCount - 1;
	}

	public void SetupCompetitor(Vehicle vehicle, int index, int competitorCount)
	{
		float total = (float) competitorCount;
		float advantage = total - (float) index;
		float[] derivatives = new float[]{
			competitorSpeed / total * advantage, 
			competitorSpeedDerivative / total * advantage, 
			competitorSpeedDerivative / total * advantage
		}; 
		vehicle.z = competitorStart + advantage;
		vehicle.stopZ = CalculateStop(index);
		vehicle.drive.derivatives = derivatives;
		vehicle.index = index;
		
		if (index % carPerCycleLane == 0)
		{
			vehicle.steering.StartCycleLane();
		}
	}

	public void CycleLaneAhead(Vehicle[] vehicles, Vehicle player)
	{
		int index = player.index;
		int ahead = index 
			- (int) (carPerCycleLane 
			- (Random.value * 50))
			- 2;
		if (0 <= ahead && ahead < vehicles.Length && index != ahead)
		{
			Vehicle vehicle = vehicles[ahead];
			if (player != vehicle)
			{
				vehicle.steering.StartCycleLane();
			}
		}
	}

	public float CalculateStop(int index)
	{
		return finishZ + postZ 
			- (0.5f + (float) index);
	}

	public bool Finish(int index)
	{
		bool isLevelUp = index <= 0;
		if (isLevelUp)
		{
			level = Mathf.Min(topSpeeds.Length, level + 1);
		}
		return isLevelUp;
	}
}
