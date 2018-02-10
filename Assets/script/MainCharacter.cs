﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{

    public float speed = 0.3f;
    public Vector3 maxVelocity = new Vector3(1, 1,1);
    public bool standing;
    public float jetSpeed = 0.5f;
    public float airSpeedMultiplier = .1f;
    public float jump=0.5f ;
    public LineRenderer line;
    public float forceX = 0f;
    public float forceY = 0f;
    public float forceZ = 0f;
    
    private MainController controller;

    void Start(){
        controller = GetComponent<MainController> ();
        GetComponent<Rigidbody>().freezeRotation = true;
        line = GetComponent<LineRenderer>();
        SetupLine();
        line.material.color=Color.red;
    }

    // Update is called once per frame
    void Update () {
        
        forceX = 0f;
        forceY = 0f;
        forceZ = 0f;

        var absVelX = Mathf.Abs (GetComponent<Rigidbody>().velocity.x);
        var absVelY = Mathf.Abs (GetComponent<Rigidbody>().velocity.y);
        var absVelZ = Mathf.Abs (GetComponent<Rigidbody>().velocity.z);

        if (absVelY < .2f) {
            standing = true;
        } else {
            standing = false;
        }
         
        if (!controller.moving.x.Equals(0)) {
            if(absVelX < maxVelocity.x){
                forceX = standing ? speed * controller.moving.x : (speed * controller.moving.x * airSpeedMultiplier);
                //transform.localScale = new Vector3(forceX > 0 ? 1 : -1, 1, 1);
            }
        }
        
        if (!controller.moving.z.Equals(0)) {
            if(absVelZ < maxVelocity.z){
                forceZ = standing ? speed * controller.moving.z : (speed * controller.moving.z * airSpeedMultiplier);
                //transform.localScale = new Vector3(forceZ > 0 ? 1 : 1, 1, -1);
            }
        }

        if (controller.moving.y > 0) {
            if(absVelY < maxVelocity.y)
                forceY = jetSpeed * controller.moving.y;
        }

        //GetComponent<Rigidbody>().AddForce (new Vector3 (forceX, forceY, forceZ));
        //GetComponent<Rigidbody>().velocity += (new Vector3 (forceX, forceY, forceZ));
        if (Input.GetKey ("space")) {
            
            //Vector3 normalized = new Vector3(forceX,0,forceZ);
            //normalized = Vector3.Normalize(normalized);
            //GetComponent<Rigidbody>().velocity += (new Vector3 (normalized.x, forceY, normalized.z));
            GetComponent<Rigidbody>().velocity += (new Vector3 (forceX, forceY, forceZ));
        }
        
        
        Vector3 startingPoint = new Vector3(transform.position.x, 0 , transform.position.z);
        line.SetPosition(0, startingPoint);
        Vector3 endPoint= new Vector3(forceX, 0, forceZ);
        endPoint.Normalize();
        line.SetPosition(1, startingPoint+endPoint*3);
        
    }
    
    void SetupLine()
    {
        //line.sortingLayerName = "OnTop";
        //line.sortingOrder = 5;
        line.SetVertexCount(2);
        Vector3 startingPoint = new Vector3(transform.position.x, 0 , transform.position.z);
        line.SetPosition(0, startingPoint);
        Vector3 endPoint= new Vector3(forceX, 0, forceZ);
        line.SetPosition(1, startingPoint+endPoint);
        line.SetWidth(1f, 1f);
        line.useWorldSpace = true;
  
    }
    
}
