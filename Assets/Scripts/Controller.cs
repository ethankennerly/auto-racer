using System.Collections;

public class Controller
{
	private Model model = new Model();
	private View view = new View();
	
	public void Start()
	{
		model.Start();
		view.Start();
	}

	public void Update(float deltaSeconds)
	{
		model.Update(deltaSeconds);
		view.Update(deltaSeconds);
	}
}
