using UnityEngine;

public class View
{
	public Model model;
	public GameObject main;
	public TextMesh finishText;
	public GameObject[] competitors;
	public delegate GameObject InstantiatePrefabDelegate(GameObject prefab, Vector3 position);
	public InstantiatePrefabDelegate InstantiatePrefab;
	private Transform[] transforms;
	private Transform player;
	private Transform camera;
	private Transform finish;
	private GameObject competitorPrefab;

	public void Start()
	{
		if (null == main) {
			main = GameObject.Find("Main");
			player = GameObject.Find("Player").transform;
			camera = GameObject.Find("Camera").transform;
			finish = GameObject.Find("Finish").transform;
			finishText = GameObject.Find("FinishText").GetComponent<TextMesh>();
			finish.transform.position = Vector3.forward * model.race.finishZ;
			model.cameraZStart = camera.position.z;
			competitorPrefab = GameObject.Find("Competitor");
			competitorPrefab.SetActive(false);
		}
		competitors = new GameObject[model.race.competitorCount];
		int vehicleCount = model.race.vehicleCount;
		transforms = new Transform[vehicleCount];
		int competitorIndex = 0;
		for (int index = 0; index < vehicleCount; index++)
		{
			Transform transform;
			Vehicle vehicle = model.vehicles[index];
			if (index == model.player.index)
			{
				transform = player;
			}
			else
			{
				vehicle.y = competitorPrefab.transform.position.y;
				Vector3 position = new Vector3(vehicle.x, vehicle.y, vehicle.z);
				GameObject competitor = InstantiatePrefab(competitorPrefab, position);
				competitors[competitorIndex] = competitor;
				competitorIndex++;
				transform = competitor.transform;
			}
			transforms[index] = transform;
		}
	}

	private void UpdatePositions(Transform[] transforms)
	{
		for (int index = 0; index < transforms.Length; index++) {
			Transform transform = transforms[index];
			float x = model.vehicles[index].x;
			float z = model.vehicles[index].z;
			ToyView.SetPositionXZ(transform, x, z);
		}
	}

	private void UpdateInput(SteeringModel steering)
	{
		steering.isInputLeft = Input.GetKeyDown(KeyCode.LeftArrow);
		steering.isInputRight = Input.GetKeyDown(KeyCode.RightArrow);
	}

	public void Update(float deltaSeconds)
	{
		UpdateInput(model.player.steering);
		UpdatePositions(transforms);
		ToyView.SetPositionXZ(camera, model.player.steering.cameraX, model.cameraZ);
		if (model.isRestartNow)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
