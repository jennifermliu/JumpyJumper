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

		if (Input.GetKey ("left")) {
			moving.x = 0.1f;
		} else if (Input.GetKey("right")) {
			moving.x = -0.1f;
		}

		if (Input.GetKey ("space")) {
			moving.y = 0.1f;
		} 

		
		if (Input.GetKey ("down")) {
			moving.z = 0.1f;
		} else if (Input.GetKey("up")) {
			moving.z = -0.1f;
		} 

	}
}