using UnityEngine;

public class MainView : MonoBehaviour
{
	public bool isShort = false;
	public bool isMusicBox = false;
	public float steeringSpeed;
	public AudioClip collisionSound;
	public AudioClip musicSound;
	public AudioClip passSound;
	public AudioClip steerLeftSound;
	public AudioClip steerRightSound;

	// Musical Notes
	public AudioClip[] melodies = new AudioClip[3];
	public AudioClip[] basses = new AudioClip[3];

	private Controller controller = new Controller();
	
	private void Start()
	{
		controller.model.isShort = isShort;
		controller.view.InstantiatePrefab = InstantiatePrefab;
		controller.view.sounds = this;
		controller.Start();
	}

	/**
	 * MonoBehaviour enables Instantiate.
	 * "You are trying to create a MonoBehaviour using the 'new' keyword.  This is not allowed.  MonoBehaviours can only be added using AddComponent().  Alternatively, your script can inherit from ScriptableObject or no base class at all"
	 */
	private GameObject InstantiatePrefab(GameObject prefab, Vector3 position)
	{
		GameObject instance = (GameObject) Instantiate(
			prefab, position, Quaternion.identity);
		instance.SetActive(true);
		return instance;
	}

	private void Update()
	{
		controller.view.SetMusicBox(isMusicBox);
		controller.Update(Time.deltaTime);
		steeringSpeed = controller.model.player.steering.speed;
	}
}
