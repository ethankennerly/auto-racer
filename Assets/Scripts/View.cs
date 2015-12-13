using UnityEngine;

public class View
{
	public Model model;
	public delegate GameObject InstantiatePrefabDelegate(GameObject prefab, Vector3 position);
	public InstantiatePrefabDelegate InstantiatePrefab;
	private Transform[] transforms;
	private Transform player;
	private Transform camera;
	private Transform finish;
	private GameObject[] competitors;
	private GameObject competitorPrefab;

	public void Start()
	{
		if (null == player) {
			player = GameObject.Find("Player").transform;
			camera = GameObject.Find("Camera").transform;
			finish = GameObject.Find("Finish").transform;
			finish.transform.position = Vector3.forward * model.race.finishZ;
			model.cameraZStart = camera.position.z;
			competitorPrefab = GameObject.Find("Competitor");
			competitorPrefab.SetActive(false);
		}
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
		SetPosition(camera, model.steering.cameraX, model.cameraZ);
	}
}
