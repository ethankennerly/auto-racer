using UnityEngine;
using System.Collections;

public class MainView : MonoBehaviour
{
	private Controller controller = new Controller();
	
	private void Start()
	{
		controller.Start();
	}

	private void Update()
	{
		controller.Update(Time.deltaTime);
	}
}
