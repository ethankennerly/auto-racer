using UnityEngine;
using System.Collections;

public class Model
{
	public int competitorCount;
	public int playerIndex;
	public Vehicle player;
	public int vehicleCount = 1;
	public Vehicle[] vehicles;

	public void Start()
	{
		vehicles = new Vehicle[vehicleCount];
		competitorCount = vehicleCount - 1;
		for (int index = 0; index < competitorCount; index++) 
		{
			vehicles[index] = new Vehicle();
		}
		playerIndex = competitorCount;
		player = new Vehicle();
		vehicles[competitorCount] = player;
	}

	public void Update(float deltaSeconds)
	{
		for (int index = 0; index < vehicleCount; index++) 
		{
			vehicles[index].Update(deltaSeconds);
		}
	}
}
