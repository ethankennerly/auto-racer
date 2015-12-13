using System.Collections;

public class Controller
{
	public Model model = new Model();
	public View view = new View();
	
	public void Start()
	{
		model.Start();
		view.model = model;
		view.Start();
	}

	public void Update(float deltaSeconds)
	{
		model.Update(deltaSeconds);
		view.Update(deltaSeconds);
	}
}
