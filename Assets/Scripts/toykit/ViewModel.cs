using System.Collections.Generic;

public class ViewModel
{
	public string main = "main";

	public Dictionary<string, object> graph = new Dictionary<string, object>(){
		{"camera", null},
		{"canvas", null},
		{"playArea", new Dictionary<string, object>(){
			{"waters", new Dictionary<string, object>() {
				{"cup_0", null},
				{"cup_1", null},
				{"cup_2", null},
				{"cup_3", null},
				{"cup_4", null},
				{"cup_5", null},
				{"cup_6", null},
				{"cup_7", null},
				{"cup_8", null}
			}}
		}}
	};

	public string[] buttons = new string[]{
		"button_0",
		"button_1",
		"button_2",
		"button_3",
		"cup_0",
		"cup_1",
		"cup_2",
		"cup_3",
		"cup_4",
		"cup_5",
		"cup_6",
		"cup_7",
		"cup_8"
	};

	public string[] sounds = new string[]{
		"drinkWater",
		"drinkWaterUndo"
	};

	public List<string> soundNews = new List<string>();

	public Dictionary<string, object> news = new Dictionary<string, object>(){
	};
}
