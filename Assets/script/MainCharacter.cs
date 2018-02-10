using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{

    public float speed = 0.5f;
    public Vector3 maxVelocity = new Vector3(1, 1,1);
    public bool standing;
    public float jetSpeed = 1f;
    public float airSpeedMultiplier = .1f;
    public float jump=1f ;


    private MainController controller;

    void Start(){
        controller = GetComponent<MainController> ();
        GetComponent<Rigidbody>().freezeRotation = true;

    }

    // Update is called once per frame
    void Update () {
        
        
        var forceX = 0f;
        var forceY = 0f;
        var forceZ = 0f;

        var absVelX = Mathf.Abs (GetComponent<Rigidbody>().velocity.x);
        var absVelY = Mathf.Abs (GetComponent<Rigidbody>().velocity.y);
        var absVelZ = Mathf.Abs (GetComponent<Rigidbody>().velocity.z);

        if (absVelY < .2f) {
            standing = true;
        } else {
            standing = false;
        }
        
        
        if (controller.moving.x.Equals(0)) {
            if(absVelX < maxVelocity.x){
                forceX = standing ? speed * controller.moving.x : (speed * controller.moving.x * airSpeedMultiplier);
                //transform.localScale = new Vector3(forceX > 0 ? 1 : -1, 1, 1);
            }
        }
        
        if (controller.moving.z.Equals(0)) {
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
            //if (transform.localPosition.y < 1   ) 
            GetComponent<Rigidbody>().velocity += (new Vector3 (forceX, forceY, forceZ));
        }

       

    }
}