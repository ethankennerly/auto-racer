using UnityEngine;
using System.Collections;

public class Vehicle
{
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	public float speed = 0.5f;

	public void Start()
	{
	}

	public void Update(float deltaSeconds)
	{
		z += speed * deltaSeconds;
	}
}
