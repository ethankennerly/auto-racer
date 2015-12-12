using UnityEngine;
using System.Collections;

public class MainView : MonoBehaviour
{
	private Controller controller = new Controller();
	
	private void Start()
	{
		controller.view.InstantiatePrefab = InstantiatePrefab;
		controller.Start();
	}

	/**
	 * MonoBehaviour enables Instantiate.
	 * "You are trying to create a MonoBehaviour using the 'new' keyword.  This is not allowed.  MonoBehaviours can only be added using AddComponent().  Alternatively, your script can inherit from ScriptableObject or no base class at all"
	 */
	private GameObject InstantiatePrefab(string prefabName, Vector3 position)
	{
		GameObject prefab = GameObject.Find(prefabName);
		prefab.SetActive(false);
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
