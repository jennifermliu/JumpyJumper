using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{

    public float speed = 10f;
    public Vector2 maxVelocity = new Vector2(3, 5);
    public bool standing;
    public float jetSpeed = 15f;
    public float airSpeedMultiplier = .3f;
    public float jump=2f ;


    private MainController controller;

    void Start(){
        controller = GetComponent<MainController> ();
        GetComponent<Rigidbody>().freezeRotation = true;

    }

    // Update is called once per frame
    void Update () {
        
        /*
        var forceX = 0f;
        var forceY = 0f;

        var absVelX = Mathf.Abs (GetComponent<Rigidbody>().velocity.x);
        var absVelY = Mathf.Abs (GetComponent<Rigidbody>().velocity.y);
        var absVelZ = Mathf.Abs (GetComponent<Rigidbody>().velocity.z);

        if (absVelY < .2f) {
            standing = true;
        } else {
            standing = false;
        }
        
        
        if (controller.moving.x != 0) {
            if(absVelX < maxVelocity.x){

                forceX = standing ? speed * controller.moving.x : (speed * controller.moving.x * airSpeedMultiplier);

                transform.localScale = new Vector3(forceX > 0 ? 1 : -1, 1, 1);
            }
        }

        if (controller.moving.y > 0) {
            if(absVelY < maxVelocity.y)
                forceY = jetSpeed * controller.moving.y;
        }

        GetComponent<Rigidbody>().AddForce (new Vector3 (forceX, forceY, 0));
        if (Input.GetKey ("a")) {

            //if (transform.localPosition.y < 1   ) 
                GetComponent<Rigidbody>().AddForce (new Vector3 (0, jump, 5 ));
        }
        
        */
        if (Input.GetKey ("right")) {
            transform.Rotate(Vector3.right*Time.deltaTime);
        } else if (Input.GetKey("left")) {
            transform.Rotate(Vector3.left*Time.deltaTime);
        }

        if (Input.GetKey ("up")) {
            transform.Rotate(Vector3.up*Time.deltaTime);
        } else if (Input.GetKey ("down")) {
            transform.Rotate(Vector3.down*Time.deltaTime);
        }

        if (Input.GetKey("space"))
        {
            
            GetComponent<Rigidbody>().AddForce(new Vector3(0,));
        }


    }
}
