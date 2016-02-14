using System.Collections;

public class Controller
{
	public Model model = new Model();
	public View view = new View();
	
	public void Start()
	{
		model.Start();
		view.model = model;
		ControllerUtil.SetupButtons(this, model.player.steering.buttons);
		view.Start();
	}

	public void OnMouseDown(string name)
	{
		model.player.steering.OnMouseDown(name);
	}

	public void Update(float deltaSeconds)
	{
		model.Update(deltaSeconds);
		ToyView.SetState(view.main, model.state);
		ToyView.SetText(view.finishText, model.finishText);
		for (int c = 0; c < model.race.competitorCount; c++)
		{
			if (null != view.competitors[c])
			{
				ToyView.SetState(view.competitors[c], 
					model.competitors[c].steering.state);
			}
		}
		view.Update(deltaSeconds);
	}
}
