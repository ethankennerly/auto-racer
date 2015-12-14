using UnityEngine;

public class MainView : MonoBehaviour
{
	public bool isShort = false;
	public AudioClip collisionSound;

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
		controller.Update(Time.deltaTime);
	}
}
