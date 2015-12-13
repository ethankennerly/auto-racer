public class Race
{
	public float playerSpeed = 0.5f;
	public float competitorSpeed = 0.0f;
	public float competitorStart = 2.0f;
	public float finishZ = 260.0f;
	public float cameraZ;
	public float postZ = 15.0f;

	public void SetupCompetitor(Vehicle vehicle, int index, int competitorCount)
	{
		float total = (float) competitorCount;
		float degree = (float) index;
		float advantage = total - (float) index;
		float[] derivatives = new float[]{
			0.0f / total * advantage, 
			5.0f / total * advantage, 
			5.0f / total * advantage
		}; 
		vehicle.z = competitorStart + advantage;
		vehicle.stopZ = CalculateStop(index);
		vehicle.drive.derivatives = derivatives;
	}

	public float CalculateStop(int index)
	{
		return finishZ + postZ 
			- 0.5f * (0.5f + (float) index);
	}

	public void Update(float deltaSeconds)
	{
	}
}
