using UnityEngine;  // Random

public class Race
{
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
	public int level = 0;

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

	public void Start(Vehicle player)
	{
		vehicleCount = vehicleCounts[level];
		player.drive.derivatives[2] = topSpeeds[level];
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
}
