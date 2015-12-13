using UnityEngine;

public class Model
{
	public bool isShort;
	public int competitorCount;
	public Vehicle player;
	public float cameraZ;
	public float cameraZStart;
	public int vehicleCount = 10;
	public Vehicle[] vehicles;
	public Vehicle[] ranks;
	public Race race = new Race();
	private int laneCount;
	private int laneCopies = 3;
	private float[] laneOriginals = new float[]{-1.0f, 0.0f, 1.0f};
	private Deck lanes = new Deck();

	private void StartIsShort(bool isShort)
	{
		if (isShort)
		{
			race.finishZ = 20.0f;
			vehicleCount = 10;
			player.drive.rates[0] = 2.5f;
		}
		else
		{
			race.finishZ = 260.0f;
			vehicleCount = 50;
		}
	}

	public void Start()
	{
		player = new Vehicle();
		player.drive.Start();
		StartIsShort(isShort);
		lanes.Setup(laneOriginals, laneCopies); 
		vehicles = new Vehicle[vehicleCount];
		ranks = new Vehicle[vehicleCount];
		competitorCount = vehicleCount - 1;
		for (int index = 0; index < competitorCount; index++) 
		{
			Vehicle vehicle = new Vehicle();
			vehicle.drive.Start();
			race.SetupCompetitor(vehicle, index, competitorCount);
			vehicle.steering.Start(lanes.NextCard());
			vehicles[index] = vehicle;
			ranks[index] = vehicle;
		}
		player.index = competitorCount;
		player.stopZ = race.CalculateStop(player.index);
		vehicles[competitorCount] = player;
	}

	public void Update(float deltaSeconds)
	{
		for (int index = 0; index < vehicleCount; index++) 
		{
			Vehicle vehicle = vehicles[index];
			if (race.finishZ <= vehicle.z)
			{
				vehicle.ComeToStop();
			}
			vehicle.Update(deltaSeconds);
		}
		if (player.IsUpdateRank(ranks)) {
			player.stopZ = race.CalculateStop(player.index);
		}
		player.UpdateCollision(ranks);
		cameraZ = player.z + cameraZStart;
	}
}
