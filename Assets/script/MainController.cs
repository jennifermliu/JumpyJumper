using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

	public Vector3 moving = new Vector3(0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		//moving.x = moving.y = moving.z = 0;

		if (Input.GetKey ("left")) {
			moving.x = 0.5f;
		} else if (Input.GetKey("right")) {
			moving.x = -0.5f;
		}

		if (Input.GetKey ("space")) {
			moving.y = 0.1f;
		} 
		//else if (Input.GetKey ("down")) {
		//	moving.y = -1;
		//}
		
		if (Input.GetKey ("down")) {
			moving.z = 0.5f;
		} else if (Input.GetKey("up")) {
			moving.z = -0.5f;
		} 

	}
}

