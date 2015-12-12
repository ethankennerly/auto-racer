using UnityEngine;

public class Model
{
	public int competitorCount;
	public int playerIndex;
	public Vehicle player;
	public int vehicleCount = 10;
	public Vehicle[] vehicles;
	public Race race = new Race();
	public SteeringModel steering = new SteeringModel();
	private int laneCount;
	private int laneCopies = 3;
	private float[] laneOriginals = new float[]{-1.0f, 0.0f, 1.0f};
	private Deck lanes = new Deck();

	public void Start()
	{
		lanes.Setup(laneOriginals, laneCopies); 
		vehicles = new Vehicle[vehicleCount];
		competitorCount = vehicleCount - 1;
		for (int index = 0; index < competitorCount; index++) 
		{
			Vehicle vehicle = new Vehicle();
			vehicle.speed = race.competitorSpeed;
			vehicle.z = race.competitorStart + (float) index;
			vehicle.x = lanes.NextCard();
			vehicles[index] = vehicle;
		}
		playerIndex = competitorCount;
		player = new Vehicle();
		player.speed = race.playerSpeed;
		vehicles[competitorCount] = player;
	}

	public void Update(float deltaSeconds)
	{
		player.x = steering.Update(deltaSeconds);
		for (int index = 0; index < vehicleCount; index++) 
		{
			vehicles[index].Update(deltaSeconds);
		}
		player.UpdateCollision(vehicles, playerIndex);
	}
}
