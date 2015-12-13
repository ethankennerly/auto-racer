using UnityEngine;

public class Model
{
	public bool isShort;
	public bool isRestart;
	public bool isRestartNow;
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
	private bool isAccelerating = false;
	public string state = "Ready";

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
		isRestart = false;
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
		isRestartNow = false;
		bool isInput = player.steering.isInputLeft || player.steering.isInputRight;
		if (!isAccelerating)
		{
			if (isInput)
			{
				isAccelerating = true;
				state = "Start";
			}
			else
			{
				deltaSeconds = 0.0f;
			}
		}
		else if (isInput && player.HasStopped() && state != "Ready")
		{
			state = "Ready";
			isRestartNow = !isRestart;
			isRestart = true;
		}
		for (int index = 0; index < vehicleCount; index++) 
		{
			Vehicle vehicle = vehicles[index];
			if (race.finishZ <= vehicle.z &&
				vehicle.IsFinishingNow())
			{
				if (player != vehicle)
				{
					vehicle.index = System.Array.IndexOf(ranks, vehicle);
					if (player.index <= vehicle.index)
					{
						//? vehicle.index++;
					}
				}
				vehicle.stopZ = race.CalculateStop(vehicle.index);
				// Debug.Log("stopZ " + vehicle.stopZ);
			}
			vehicle.Update(deltaSeconds);
		}
		if (player.IsUpdateRank(ranks)) {
			race.CycleLaneAhead(vehicles, player);
		}
		player.UpdateCollision(ranks);
		cameraZ = player.z + cameraZStart;
	}
}
