using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{

    public LineRenderer line;
    public float thrust;
    public float minThrust;
    public float rotationSpeed;

    private bool hasJumped;
    public float thrustIncrement;
    private Vector3 forceDirection;
    private Vector3 forceMagnitude;
    public float arrowScale;

    void Start(){
        line = GetComponent<LineRenderer>();
        SetupLine();
        line.material.color=Color.red;
        rotationSpeed = 200f;
        minThrust = 3f;
        hasJumped = false;
        thrust = minThrust;
        arrowScale = 0.4f;
        thrustIncrement = 5f;

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKey ("left")) {
            transform.Rotate(0, -Time.deltaTime*rotationSpeed, 0);
        } else if (Input.GetKey("right")) {
            transform.Rotate(0, Time.deltaTime*rotationSpeed, 0);
        }

        forceDirection = transform.forward + transform.up;

        if (Input.GetKey ("space")){
            hasJumped = true;
            thrust += Time.deltaTime * thrustIncrement;
        }

        forceMagnitude = forceDirection * thrust;

        if (Input.GetKeyUp ("space")) {
            GetComponent<Rigidbody>().AddForce(forceMagnitude, ForceMode.Impulse);
            thrust = minThrust;
        }


        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        
        Vector3 startingPoint = transform.position+offset;
        line.SetPosition(0, startingPoint);
        Vector3 endPoint = forceDirection + forceDirection * arrowScale * (thrust - minThrust);
        line.SetPosition(1, startingPoint+endPoint);
        
    }
    
    void SetupLine()
    {
        //line.sortingLayerName = "OnTop";
        //line.sortingOrder = 5;
        //line.SetVertexCount(2);
        //Vector3 startingPoint = transform.position;
        //line.SetPosition(0, startingPoint);
        //Vector3 endPoint= new Vector3(forceX, forceY, forceZ);
        //line.SetPosition(1, startingPoint+endPoint);
        //line.SetWidth(0.1f, 0.5f);
        //line.useWorldSpace = true;
  
    }
    
    
}