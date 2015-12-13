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
		ToyView.setState(view.main, model.state);
		ToyView.setText(view.finishText, model.finishText);
		for (int c = 0; c < model.race.competitorCount; c++)
		{
			if (null != view.competitors[c])
			{
				ToyView.setState(view.competitors[c], 
					model.competitors[c].steering.state);
			}
		}
		view.Update(deltaSeconds);
	}
}
