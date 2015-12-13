using UnityEngine;

public class Model
{
	public static string cardinal(int rank)
	{
		string place = "th";
		if (rank < 10 || 20 <= rank)
		{
			int lastDigit = rank % 10;
			if (1 == lastDigit)
			{
				place = "st";
			}
			else if (2 == lastDigit)
			{
				place = "nd";
			}
			else if (3 == lastDigit)
			{
				place = "rd";
			}
		}
		return place;
	}

	public static string SetFinishText(int index)
	{
		int rank = index + 1;
		return string.Format("{0}{1} place!", 
			rank, cardinal(rank));
	}

	public bool isShort;
	public bool isRestart;
	public bool isRestartNow;
	public Vehicle player;
	public float cameraZ;
	public float cameraZStart;
	public Vehicle[] vehicles;
	public Vehicle[] ranks;
	public Vehicle[] competitors;
	public Race race = new Race();
	private int laneCount;
	private int laneCopies = 3;
	private float[] laneOriginals = new float[]{-1.0f, 0.0f, 1.0f};
	private Deck lanes = new Deck();
	private bool isAccelerating = false;
	public string state = "Ready";
	public string finishText = " ";

	private void StartIsShort(bool isShort)
	{
		if (isShort)
		{
			race.finishZ = 20.0f;
			race.vehicleCount = 8;
			race.competitorCount = race.vehicleCount - 1;
			player.drive.rates[0] = 5.0f;
		}
		else
		{
			race.finishZ = 260.0f;
		}
	}

	public void Start()
	{
		isRestart = false;
		player = new Vehicle();
		player.drive.Start();
		race.Start(player);
		StartIsShort(isShort);
		lanes.Setup(laneOriginals, laneCopies); 
		vehicles = new Vehicle[race.vehicleCount];
		ranks = new Vehicle[race.vehicleCount];
		competitors = new Vehicle[race.competitorCount];
		for (int index = 0; index < race.competitorCount; index++) 
		{
			Vehicle vehicle = new Vehicle();
			vehicle.drive.Start();
			race.SetupCompetitor(vehicle, index, race.competitorCount);
			vehicle.steering.Start(lanes.NextCard());
			vehicles[index] = vehicle;
			ranks[index] = vehicle;
			competitors[index] = vehicle;
		}
		player.index = race.competitorCount;
		player.stopZ = race.CalculateStop(player.index);
		vehicles[race.competitorCount] = player;
		ranks[race.competitorCount] = player;
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
		else if (player.HasStopped())
		{
			if (isInput && state != "Ready")
			{
				state = "Ready";
				isRestartNow = !isRestart;
				isRestart = true;
			}
			else
			{
				state = "RestartPrompt";
			}
		}
		for (int index = 0; index < race.vehicleCount; index++) 
		{
			Vehicle vehicle = vehicles[index];
			if (race.finishZ <= vehicle.z &&
				vehicle.IsFinishingNow())
			{
				if (player == vehicle)
				{
					finishText = SetFinishText(player.index);
					if (race.Finish(player.index))
					{
						finishText += string.Format("\nNext level\n{0} of {1}", race.level + 1, race.levelCount);
					}
				}
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
