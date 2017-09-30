using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebelAI : MonoBehaviour {
	public float speed = 5.0f;
	private float time = 0.0f;
 public float interpolationPeriod = 0.04f;

	public void Start()
	{
	  Vector2 randomDirection = new Vector2(Random.value, Random.value);
	  transform.Rotate(randomDirection);
	}

	public void Update()
	{
		time += Time.deltaTime;

		if (time >= interpolationPeriod) {
				time = 0.0f;
				Vector2 randomDirection = new Vector2(Random.value, Random.value);
				transform.Rotate(randomDirection);
				// execute block of code here
		}
	  transform.position = transform.forward * speed;
	}

}
