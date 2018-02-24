using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block1 : MonoBehaviour {

	// Use this for initialization

	public int index;
	public Boolean prev;
	public int reward;

    private Color startColor;
    private Color endColor;

    private Renderer renderer;

    private bool disappear = false;
    private float startTime = 0f;
    public float targetTime = 2f;
    private float progress = 0f;


	void Start () {

        renderer = gameObject.GetComponent<Renderer>();

        startColor = renderer.material.color;
        endColor = startColor;
        endColor.a = 0;

	}
	
	// Update is called once per frame
	void Update () {



        if (disappear){
            progress = Time.time - startTime;
            renderer.material.color = Color.Lerp(startColor, endColor, progress/targetTime);
        }
		
        if (progress >= targetTime){
            Destroy(gameObject);
        }


	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            startTime = Time.time;
            disappear = true;
            renderer.material.color = endColor;
        }

    }
}
