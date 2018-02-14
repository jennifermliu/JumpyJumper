using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharacter : MonoBehaviour
{

    public LineRenderer line;
    public float thrust;
    public float minThrust;
    public float rotationSpeed;

    private bool canJump;
    public float thrustIncrement;
    private Vector3 forceDirection;
    private Vector3 forceMagnitude;
    public float arrowScale;

    private bool successJump;
    private GameObject camera;
    private Vector3 cameraOffset;

    void Start(){
        line = GetComponent<LineRenderer>();
        camera = GameObject.Find("Main Camera");
        cameraOffset = camera.transform.position - transform.position;
        rotationSpeed = 200f;
        minThrust = 3f;
        canJump = true;
        thrust = minThrust;
        arrowScale = 0.4f;
        thrustIncrement = 5f;
        successJump = false;

    }

    // Update is called once per frame
    void Update () {


        //Rotates character by rotationSpeed degrees per second with arrow keys
        if (Input.GetKey ("left")) {
            transform.Rotate(0, -Time.deltaTime*rotationSpeed, 0);
        } else if (Input.GetKey("right")) {
            transform.Rotate(0, Time.deltaTime*rotationSpeed, 0);
        }

        //Calculates direction of jump using forward vector, and adding 'up' vector to launch upwards
        forceDirection = transform.forward + transform.up;

        //Increases power of jump once space is held
        if (Input.GetKey ("space")){
            thrust += Time.deltaTime * thrustIncrement;
        }

        //Calculates force of jump using direction force and power of jump
        forceMagnitude = forceDirection * thrust;


        //Causes character to jump once space is released, resets canJump to prevent doubleJumps
        if (Input.GetKeyUp ("space") && canJump) {
            GetComponent<Rigidbody>().AddForce(forceMagnitude, ForceMode.Impulse);
            thrust = minThrust;
            canJump = false;
        }

        //Calculates arrow's offset from player
        Vector3 offset = new Vector3(0f, 0.5f, 0f);

        //Generates arrow endpoints based on player direction and force magnitude
        Vector3 startingPoint = transform.position+offset;
        line.SetPosition(0, startingPoint);
        Vector3 endPoint = forceDirection + forceDirection * arrowScale * (thrust - minThrust);
        line.SetPosition(1, startingPoint+endPoint);

        //Moves camera and resets successJump if player has made successful jump
        if (successJump){
            camera.transform.position = transform.position + cameraOffset;
            successJump = false;
        }
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        //Registers as successful jump if player touches new block

        if (collision.gameObject.tag == "block")
        {

            //Should register as success only when jumping on top of block, not sides - a bit buggy right now though

            if (ReturnDirection(collision.gameObject, this.gameObject) == HitDirection.Top){
                successJump = true;
                canJump = true;
            }
        }

        //Reloads the scene if player touches floor

        if (collision.gameObject.tag == "floor")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
    private HitDirection ReturnDirection(GameObject Object, GameObject ObjectHit)
    {

        HitDirection hitDirection = HitDirection.None;
        RaycastHit MyRayHit;
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction);

        if (Physics.Raycast(MyRay, out MyRayHit))
        {

            if (MyRayHit.collider != null)
            {

                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal.y > 0f){
                    hitDirection = HitDirection.Top;
                }
            }
        }
        return hitDirection;
    }


    
    
}