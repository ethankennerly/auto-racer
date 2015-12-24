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
	public bool isPerfectMode;
	public bool isFinish;
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
	private Deck lanes = new Deck();
	private bool isAccelerating = false;
	public string state = "Ready";
	public string stateNow = null;
	public string finishText = " ";

	public void Start()
	{
		isRestart = false;
		player = new Vehicle();
		player.drive.Start();
		race.Start(player, isShort);
		lanes.Setup(player.steering.lanes, laneCopies); 
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

	private bool SetState(string nextState)
	{
		bool isNow = nextState != state;
		if (isNow)
		{
			state = nextState;
			stateNow = nextState;
		}
		return isNow;
	}

	/**
	 * Steer off-road: restart.
	 * Test case:  2015-12-16 TheMeorch expects to restart level (+TerraCottaFrog, +loxo, +rplnt).
	 */
	private void IfOffRoadRestart()
	{
		if (!isRestart && player.steering.isOffRoad)
		{
			isRestartNow = !isRestart;
			isRestart = true;
		}
	}

	public void Update(float deltaSeconds)
	{
		isRestartNow = false;
		stateNow = null;
		bool isInput = player.steering.isInputLeft || player.steering.isInputRight;
		if (!isAccelerating)
		{
			if (isInput)
			{
				isAccelerating = true;
				SetState("Start");
			}
			else
			{
				deltaSeconds = 0.0f;
			}
		}
		else if (player.HasStopped())
		{
			if (isInput && SetState("Ready"))
			{
				isRestartNow = !isRestart;
				isRestart = true;
			}
			else
			{
				SetState("RestartPrompt");
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
						finishText += string.Format("\nNext level\n{0} of {1}", Race.level + 1, race.levelCount);
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
		if (!isPerfectMode)
		{
			player.UpdateCollision(ranks);
		}
		cameraZ = player.z + cameraZStart;
		IfOffRoadRestart();
	}

	public void toggleIsPerfectMode()
	{
		isPerfectMode = !isPerfectMode;
		Debug.Log("Model.toggleIsPerfectMode: " + isPerfectMode);
	}
}
