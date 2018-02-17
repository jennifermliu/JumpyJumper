using Random=System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
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

    //variables for generating new boxes
    private Object cylinder;
    private Object cube;
    private float dist;//dist between new block and current block
    
    void Start(){
        line = GetComponent<LineRenderer>();
        camera = GameObject.Find("Main Camera");
        cameraOffset = camera.transform.position - transform.position;
        rotationSpeed = 100f;
        minThrust = 3f;
        canJump = true;
        thrust = minThrust;
        arrowScale = 0.4f;
        thrustIncrement = 5f;
        successJump = false;
        
        cylinder = Resources.Load("Block2");
        cube = Resources.Load("Block1");
        dist = 6f;

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
        //Vector3 endPoint = forceDirection + forceDirection * arrowScale * (thrust - minThrust);
        
        Vector3 endPoint = transform.forward  + transform.forward * arrowScale * (thrust - minThrust);
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
                
                //variabel to keep tarck of direction of new block from the old block
                //0: up
                //1: down
                //2: left
                //3: right
                int current=-1;
                //check if colliding block is cylinder or cube
                Block1 c1 = collision.gameObject.GetComponent<Block1>();
                Block2 c2 = collision.gameObject.GetComponent<Block2>();
                if (c1 != null)
                {
                    current=c1.index;
                    c1.index = -1;
                    //mark prev to true to show that this is the previous block
                    c1.prev = true;
                }
                else if (c2 != null)
                {
                    current=c2.index;
                    c2.index = -1;
                    //mark prev to true to show that this is the previous block
                    c2.prev = true;
                }
                GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("block");
                foreach (GameObject oldblock in oldblocks)
                {
                    //delete all blocks that are not colliding block or previous block
                    if (oldblock != collision.gameObject)
                    {
                        if (oldblock.GetComponent<Block1>() != null && oldblock.GetComponent<Block1>().prev==false)
                        {
                            Destroy(oldblock);
                        }
                        else if (oldblock.GetComponent<Block2>() != null && oldblock.GetComponent<Block2>().prev==false)
                        {
                            Destroy(oldblock);
                        }
                    }
                }
                DisplayNewBoxes(current);
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

    private void DisplayNewBoxes(int current)
    {
        Random rnd = new Random();
        //determine shape and size of each of the three new blocks
        int[] indices = new int[3];
        indices[0] = rnd.Next(0, 6);
        indices[1] = rnd.Next(0, 6);
        indices[2] = rnd.Next(0, 6);
        //position array for 4 directions
        Vector3[] positions = new Vector3[4];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i]= transform.position;
            positions[i].y = 1;
        }
        //up
        positions[0].z -= dist;
        //down
        positions[1].z += dist;
        //left
        positions[2].x += dist;
        //right
        positions[3].x -= dist;
        if (current == 0)//up
        {
            GenerateABox(indices[0],positions[0],0);
            GenerateABox(indices[1],positions[2],2);
            GenerateABox(indices[2],positions[3],3);
        }
        else if (current == 1)//down
        {
            GenerateABox(indices[0],positions[1],1);
            GenerateABox(indices[1],positions[2],2);
            GenerateABox(indices[2],positions[3],3);
        }
        else if (current == 2)//left
        {
            GenerateABox(indices[0],positions[0],0);
            GenerateABox(indices[1],positions[1],1);
            GenerateABox(indices[2],positions[2],2);
        }
        else if (current == 3)//right
        {
            GenerateABox(indices[0],positions[0],0);
            GenerateABox(indices[1],positions[1],1);
            GenerateABox(indices[2],positions[3],3);
        }
        //if -1, don't generate new boxes
    }

    //generate a new box based on type represented by i, position represented by pos, 
    //and direction of new box relative to old box represented by dir
    private void GenerateABox(int i, Vector3 newpos, int dir)
    {
        
        if (i >= 0 && i <= 2)//cylinders
        {
            GameObject newblock = (GameObject) Instantiate(cylinder, newpos, Quaternion.identity);
            if (i == 0)//small
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                newblock.gameObject.transform.localScale -= new Vector3(1f, 0, 1f);
            }
            else if (i == 1)//medium
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.red;
                newblock.gameObject.transform.localScale -= new Vector3(0.5f, 0, 0.5f);
            }
            else//large
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
            Block2 newcylinder = newblock.GetComponent<Block2>();
            newcylinder.index = dir;
            newcylinder.prev = false;   
        }
        else if (i>=3 && i<=5)//cubes
        {
            GameObject newblock = (GameObject) Instantiate(cube, newpos, Quaternion.identity);
            if (i == 3)//small
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                newblock.gameObject.transform.localScale -= new Vector3(1f, 0, 1f);
            }
            else if (i == 4)//medium
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                newblock.gameObject.transform.localScale -= new Vector3(0.5f, 0, 0.5f);
            }
            else//large
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.black;
            }
            Block1 newcube = newblock.GetComponent<Block1>();
            newcube.index = dir;
            newcube.prev = false;
        }
    }
}