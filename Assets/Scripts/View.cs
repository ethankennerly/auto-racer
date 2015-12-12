using UnityEngine;

public class View
{
	public Model model;
	public delegate GameObject InstantiatePrefabDelegate(GameObject prefab, Vector3 position);
	public InstantiatePrefabDelegate InstantiatePrefab;
	private Transform[] transforms;
	private Transform player;
	private GameObject[] competitors;

	public void Start()
	{
		player = GameObject.Find("Player").transform;
		GameObject competitorPrefab = GameObject.Find("Competitor");
		competitorPrefab.SetActive(false);
		competitors = new GameObject[model.competitorCount];
		transforms = new Transform[model.vehicleCount];
		int competitorIndex = 0;
		for (int index = 0; index < model.vehicles.Length; index++)
		{
			Transform transform;
			Vehicle vehicle = model.vehicles[index];
			if (index == model.playerIndex)
			{
				transform = player;
			}
			else
			{
				Vector3 position = new Vector3(vehicle.x, vehicle.y, vehicle.z);
				GameObject competitor = InstantiatePrefab(competitorPrefab, position);
				competitors[competitorIndex] = competitor;
				competitorIndex++;
				transform = competitor.transform;
			}
			transforms[index] = transform;
		}
	}

	private void SetPosition(Transform transform, float x, float z)
	{
		Vector3 position = transform.position;
		position.x = x;
		position.z = z;
		transform.position = position;
	}

	private void UpdatePositions(Transform[] transforms)
	{
		for (int index = 0; index < transforms.Length; index++) {
			Transform transform = transforms[index];
			float x = model.vehicles[index].x;
			float z = model.vehicles[index].z;
			SetPosition(transform, x, z);
		}
	}

	private void UpdateInput(SteeringModel steering)
	{
		steering.isInputLeft = Input.GetKeyDown(KeyCode.LeftArrow);
		steering.isInputRight = Input.GetKeyDown(KeyCode.RightArrow);
	}

	public void Update(float deltaSeconds)
	{
		UpdateInput(model.steering);
		UpdatePositions(transforms);
	}
}
