public class Drive
{
	public float[] derivatives;
	public float[] rates;
	public float[] derivativesStart = new []{0.0f, 5.0f, 80.0f};
	public float[] ratesStart = new []{0.125f, 0.01f};
	public float[] ratesFinish = new []{10.0f, 10.0f};

	public void Start()
	{
		derivatives = (float[]) derivativesStart.Clone();
		rates = (float[]) ratesStart.Clone();
	}

	public float Update(float deltaSeconds)
	{
		for (int index = rates.Length - 1; 0 <= index; index--)
		{
			derivatives[index] += (derivatives[index + 1] 
				- derivatives[index]) 
				* deltaSeconds * rates[index];
		}
		return derivatives[0] * deltaSeconds;
	}
}
