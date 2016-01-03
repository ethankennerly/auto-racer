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
	public float roadZ = 350.0f;
	public float cameraZ;
	public float postZ = 15.0f;
	public float carPerCycleLane = 	// 2.0f; 
					// 4.0f; 
					8.0f;
	public int randomPerCycleLane = -1;
	public int levelCount;

	/**
	 * Shorten race at level 2 and 3 with fewer vehicles.
	 *	Test case:  2015-12-20 Level 1.  TerraCottaFrog expects challenge.  Felt bored.
	 */
	private float[] finishZs = {
		60.0f, 80.0f, 170.0f, 220.0f, 220.0f, 240.0f, 
		240.0f, 260.0f, 260.0f, 260.0f, 270.0f, 280.0f
	};

	private int[] vehicleCounts = {
		12, 14, 20, 40, 50, 60, 
		70, 80, 90, 100, 110, 120
	};

	private float[] topSpeeds = {
		60.0f, 70.0f, 80.0f, 90.0f, 100.0f, 110.0f, 
		120.0f, 130.0f, 140.0f, 150.0f, 160.0f, 180.0f
	};

	/**
	 * 
	 * Reduce frequencies of cars that switch lanes about 10% and test in slow motion.
	 *	Test case:  2015-12-20 Level 8.  Blobo expects to feel challenged.  Felt overwhelmed (+zenmumbler, +Muel).
	 * Add 2 more levels that pass more cars about 10% faster.
	 * 	Test case:  2015-12-20 nosfer expects faster.
	 */
	private int[] carPerCycleLanes = {
		-1, 20, 10, 10, 9, 9, 
		8, 8, 7, 7, 6, 6
	};

	private int[] randomPerCycleLanes = {
		-1, -1, 80, 70, 60, 50, 
		40, 38, 35, 33, 30, 25
	};

	private void StartIsShort(Vehicle player, bool isShort)
	{
		if (isShort)
		{
			finishZ = 20.0f;
			vehicleCount = level + 2;
			player.drive.rates[0] = 5.0f;
		}
	}

	public void Start(Vehicle player, bool isShort)
	{
		levelCount = topSpeeds.Length;
		vehicleCount = vehicleCounts[level];
		player.drive.derivatives[2] = topSpeeds[level];
		carPerCycleLane = carPerCycleLanes[level];
		randomPerCycleLane = randomPerCycleLanes[level];
		finishZ = finishZs[level];
		StartIsShort(player, isShort);
		competitorCount = vehicleCount - 1;
	}

	public void SetupCompetitor(Vehicle vehicle, int index, int competitorCount)
	{
		float total = (float) competitorCount;
		float denominator = Mathf.Max(20.0f, total);
		float advantage = total - (float) index;
		float[] derivatives = new float[]{
			competitorSpeed / denominator * advantage, 
			competitorSpeedDerivative / denominator * advantage, 
			competitorSpeedDerivative / denominator * advantage
		}; 
		vehicle.z = competitorStart + advantage;
		vehicle.stopZ = CalculateStop(index);
		vehicle.drive.derivatives = derivatives;
		vehicle.index = index;
		
		if (1 <= carPerCycleLane && index % carPerCycleLane == 0)
		{
			vehicle.steering.StartCycleLane();
		}
	}

	/**
	 * Increase closest rank to start signaling to change lane to 6.  Was 2.
	 *	Test case:  2015-12-21 Level 10.  Muel expects moment before changing lane and passing.
	 */
	public void CycleLaneAhead(Vehicle[] vehicles, Vehicle player)
	{
		if (randomPerCycleLane <= 0) {
			return;
		}
		int index = player.index;
		int minRanksAhead = 6;
		// 2;
		int ahead = index 
			- (int) (carPerCycleLane 
			- (Random.value * randomPerCycleLane))
			- minRanksAhead;
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
			level = Mathf.Min(topSpeeds.Length - 1, level + 1);
		}
		return isLevelUp;
	}

	public int CheatLevelUp(int amount)
	{
		level = (level + amount) % levelCount;
		if (level < 0)
		{
			level += levelCount;
		}
		Debug.Log("CheatLevelUp: level index " + level);
		return level;
	}
}
