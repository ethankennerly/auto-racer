using UnityEngine;  // Random

public class Race
{
	public float playerSpeed = 0.5f;
	public float competitorSpeed = 0.0f;
	public float competitorSpeedDerivative = 10.0f; // 5.0f;
	public float competitorStart = 2.0f;
	public float finishZ = 260.0f;
	public float cameraZ;
	public float postZ = 15.0f;
	public float carPerCycleLane = 	// 2.0f; 
					// 4.0f; 
					8.0f;

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
