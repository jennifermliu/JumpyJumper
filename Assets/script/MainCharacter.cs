using System;
using Random=System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Menus;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class MainCharacter : MonoBehaviour
{

    public LineRenderer line;
    public float thrust;
    public float minThrust;
    private float maxThrust;
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
    private Object freezeBlock;
    private Object multiBlock;
    private float dist; //dist between new block and current block

    //Display scores
    private GameObject score;
    private int currentscore = 0;
    private int blockscore = 0;

    private Text powerUpText;
    private Text scoreText;
    private Text highestText;
    private Text centerText;

    //Register for Menu
    public static UIManager UI { get; private set; }
    public bool menushowed = false;
    
    public const int num = 4;//number of boxes generated 

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public Vector3 cameraTargetPos;
    private bool incrementThrust = true;
    public bool canMove = true; //used for the freeze block powerup

    public int blocknumber;//number of blocks jumped
    private int pastBlockNumber = 0; //placeholder for keeping track of multiblock powerup
    private int effectDuration = 3; //number of blocks powerup effects last for

    public int scoreMultiplier;//multiplier when jumped to center

    private int scoreBlockMultiplier; //multiplier from score multiplier block

    private int highestscore; //highest score

    private const float distFromCenter = 0.5f; //maximal distance from center to be considered as centered
    
    private const float length = 5f;

    public GameObject destination;
    
    void Start()
    {
        line = GetComponent<LineRenderer>();
        camera = GameObject.Find("Main Camera");
        cameraTargetPos = camera.transform.position;
        cameraOffset = camera.transform.position - transform.position;
        rotationSpeed = 100f;
        minThrust = 3f;
        maxThrust = 7f;
        canJump = true;
        thrust = minThrust;
        arrowScale = 0.4f;
        thrustIncrement = 5f;
        successJump = false;

        cylinder = Resources.Load("Block2");
        cube = Resources.Load("Block1");
        freezeBlock = Resources.Load("FreezeBlock");
        multiBlock = Resources.Load("MultiBlock");

        dist = 6f;

        score = GameObject.FindGameObjectWithTag("score");
        score.GetComponent<Text>().text = "Score : " + currentscore;

        powerUpText = GameObject.Find("PowerUpText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        highestText = GameObject.Find("HighestScore").GetComponent<Text>();
        centerText = GameObject.Find("CenterText").GetComponent<Text>();

        UI = new UIManager();
        highestscore = PlayerPrefs.GetInt("highestscore", 0);

        blocknumber = 0;
        scoreMultiplier = 1;
        scoreBlockMultiplier = 1;
        //highestscore = 0;

        //Vector3 despos = new Vector3(4.3f, 1, -229);
        Vector3 despos = new Vector3(4.3f, 1, 6);
        destination = (GameObject) Instantiate(cylinder, despos, Quaternion.identity);
        destination.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        destination.gameObject.tag = "destination";

    }

    void ResetDestination(Vector3 pos)
    {
        
    }
    
    void Awake()
    {
        //GameObject.DontDestroyOnLoad(highestText);
        //highestscore = int.Parse(highestText.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //controls character movement and jumping
            playerControls();
        }

        //displays arrow
        updateArrow();

        //displays powerUp text
        updateText();

        //menuTrigger
        showMenu();

        //DontDestroyOnLoad(highestText);     


        //Moves camera and resets successJump if player has made successful jump
        if (successJump)
        {
            cameraTargetPos = transform.position + cameraOffset;
            successJump = false;
        }

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, cameraTargetPos, ref velocity, smoothTime);
        
        
    }

    void updateText (){
        
        String text = "";

        if (scoreBlockMultiplier == 2)
        {
            text += "Score Multiplier! (x2)";
        }

        if (!canMove)
        {
            text += "Player is frozen!!!";
        }

        powerUpText.text = text;

    }

    void playerControls()
    {
        //Rotates character by rotationSpeed degrees per second with arrow keys
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -Time.deltaTime * rotationSpeed, 0);
        }
        else if (Input.GetKey("right"))
        {
            transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
        }

        //Calculates direction of jump using forward vector, and adding 'up' vector to launch upwards
        forceDirection = transform.forward + transform.up;

        //Increases power of jump once space is held
        if (Input.GetKey("space"))
        {
            if (thrust > maxThrust)
            {
                incrementThrust = false;
            }
            else if (thrust < minThrust)
            {
                incrementThrust = true;
            }

            if (!incrementThrust)
            {
                thrust -= Time.deltaTime * thrustIncrement;
            }
            else if (incrementThrust)
            {
                thrust += Time.deltaTime * thrustIncrement;
            }
        }

        //Calculates force of jump using direction force and power of jump
        forceMagnitude = forceDirection * thrust;


        //Causes character to jump once space is released, resets canJump to prevent doubleJumps
        if (Input.GetKeyUp("space") && canJump)
        {
            GetComponent<Rigidbody>().AddForce(forceMagnitude, ForceMode.Impulse);
            thrust = minThrust;
        }
    }

    void updateArrow()
    {
        //Calculates arrow's offset from player
        Vector3 offset = new Vector3(0f, 0.5f, 0f);

        //Generates arrow endpoints based on player direction and force magnitude
        Vector3 startingPoint = transform.position + offset;
        line.SetPosition(0, startingPoint);
        //Vector3 endPoint = forceDirection + forceDirection * arrowScale * (thrust - minThrust);

        Vector3 endPoint = transform.forward + transform.forward * arrowScale * (thrust - minThrust);
        line.SetPosition(1, startingPoint + endPoint);
    }

    void showMenu()
    {
        //Show menu when pressing p
        if (Input.GetKeyDown("p"))
        {
            if (!menushowed)
            {
                UI.ShowPauseMenu();
                menushowed = true;
            }
        }

        if (GameObject.FindGameObjectsWithTag("menu") == null)
        {
            menushowed = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Registers as successful jump if player touches new block
        if (collision.gameObject.tag == "block")
        {
            blocknumber++;//increment number of block jumped
            //Should register as success only when jumping on top of block, not sides - a bit buggy right now though
            if (ReturnDirection(collision.gameObject, this.gameObject) == HitDirection.Top)
            {
                successJump = true;
                canJump = true;

                //variabel to keep track of direction of new block from the old block
                //0: up
                //1: down
                //2: left
                //3: right
                int current = -1;
                //check if colliding block is cylinder or cube
                Block1 c1 = collision.gameObject.GetComponent<Block1>();
                Block2 c2 = collision.gameObject.GetComponent<Block2>();
                MultiBlock m1 = collision.gameObject.GetComponent<MultiBlock>();
                FreezeBlock f1 = collision.gameObject.GetComponent<FreezeBlock>();

                if (c1 != null)
                {
                    blockscore = c1.reward;
                    current = c1.index;
                    c1.index = -1;
                    //mark prev to true to show that this is the previous block
                    
                    if (isCenter(c1.transform.position))
                    {
                        scoreMultiplier++;
                        if (!c1.prev)
                        {
                            StartCoroutine(ShowMessage("Jump To The Center (X2)", 1f, 3));
                        }              
                    }                    
                    else scoreMultiplier = 1;//reset score multiplier if player not in center
                    c1.prev = true; 
                }
                else if (c2 != null)
                {
                    blockscore = c2.reward;
                    current = c2.index;
                    c2.index = -1;
                    //mark prev to true to show that this is the previous block
                    
                    if (isCenter(c2.transform.position))
                    {
                        scoreMultiplier++;
                        if (!c2.prev)
                        {
                            StartCoroutine(ShowMessage("Jump To The Center (X2)", 1f, 3));
                        }                      
                    }
                    else scoreMultiplier = 1;
                    c2.prev = true;
                }
                else if (m1 != null)
                {
                    scoreBlockMultiplier = 2;
                    if (isCenter(m1.transform.position))
                    {
                        scoreMultiplier++;
                        //index for centertext is 3
                        if (!m1.prev)
                        {
                            StartCoroutine(ShowMessage("Jump To The Center (X2)", 1f, 3));
                        }                       
                    }
                    else scoreMultiplier = 1;
                    m1.prev = true;
                    pastBlockNumber = blocknumber;

                }
                
                else if (f1 != null)
                {
                    if (isCenter(f1.transform.position))
                    {
                        scoreMultiplier++;
                        if (!f1.prev)
                        {
                            StartCoroutine(ShowMessage("Jump To The Center (X2)", 1f, 3));
                        }
                        
                    }
                    else scoreMultiplier = 1;
                    f1.prev = true;

                }

                if (blocknumber > pastBlockNumber + effectDuration){
                    scoreBlockMultiplier = 1;
                }
                
                blockscore *= scoreMultiplier * scoreBlockMultiplier;//multiply score of current block with multiplier

                //Display block score earned
                //Index for scoretext is 1
                StartCoroutine(ShowMessage("+ " + blockscore, 1f, 1));
                
                //Increment scores 
                currentscore += blockscore;
                score.GetComponent<Text>().text = "Score : " + currentscore;

                GameObject[] oldblocks = GameObject.FindGameObjectsWithTag("block");
                foreach (GameObject oldblock in oldblocks)
                {
                    //delete all blocks that are not colliding block
                    if (oldblock != collision.gameObject)
                    {
                        Destroy(oldblock);
                    }
                }
                DisplayNewBoxes(collision.transform.position);
            }
        }

        //Reloads the scene if player touches floor

        if (collision.gameObject.tag == "floor")
        {

            
            //Index for highestscore is 2
            //reloading the scene is done in StartCoroutine function

            //Fetch the highestscore from the previous scene
            //highestscore = PlayerPrefs.GetInt("highestscore", 0);
            if (currentscore > highestscore)
            {
                highestscore = currentscore;
                
            }
            PlayerPrefs.SetInt("highestscore", highestscore);
            StartCoroutine(ShowMessage("Highest Score: " + highestscore, 3f, 2));

            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    void OnCollisionExit(Collision collision){
        canJump = false;
    }

    public bool isCenter(Vector3 blockpos)
    {
        Vector3 pos = transform.position;
        if (pos.x >= blockpos.x - distFromCenter && pos.x <= blockpos.x + distFromCenter && pos.z >= blockpos.z - distFromCenter && pos.z <= blockpos.z + distFromCenter)
        {
            return true;
        }
        return false;
    }
    

    private enum HitDirection
    {
        None,
        Top,
        Bottom,
        Forward,
        Back,
        Left,
        Right
    }

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

                if (MyNormal.y > 0f)
                {
                    hitDirection = HitDirection.Top;
                }
            }
        }
        return hitDirection;
    }

    private void DisplayNewBoxes(Vector3 currpos)
    {
        Random rnd = new Random();//type generator
        //determine shape and size of each of the three new blocks
        int[] indices = new int[num];
        for (int i = 0; i < num; i++)
        {
            indices[i] = rnd.Next(0, 8);
        }
        //position array for 3 directions
        Vector3[] positions = new Vector3[num];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = currpos;
            positions[i].y = 1;
        }
        Random distrnd = new Random();//distance generated
        int[] dist = new int[num];
        for (int i = 0; i < num; i++)
        {
            dist[i] = distrnd.Next(1, 3);
        }
        
        //up
        positions[0].z -= dist[0] * length;
        //down
        positions[1].z += dist[1] * length;
        //left
        positions[2].x += dist[2] * length;
        //right
        positions[3].x -= dist[3] * length;
        //if -1, don't generate new boxes
        
        for(int i = 0; i<positions.Length; i++)
        {
            GenerateABox(indices[i],positions[i],i);
        }
        
    }

    //generate a new box based on type represented by i, position represented by pos, 
    //and direction of new box relative to old box represented by dir
    private void GenerateABox(int i, Vector3 newpos, int dir)
    {
        //don't add block if there's a destination block here
        if (destination.transform.position == newpos)
        {
            return;
        }
        int shape;
        Vector3 small = new Vector3(-0.1f, 0, -0.1f);
        Vector3 medium = new Vector3(-0.05f, 0, -0.05f);
        Vector3 large = new Vector3(0f, 0, 0f);
        float sizeMultiplier = getSizeMultiplier();
       
        if (i >= 0 && i <= 2) //cylinders
        {
            shape = 1;
            GameObject newblock = (GameObject) Instantiate(cylinder, newpos, Quaternion.identity);
            if (i == 0) //small
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                newblock.gameObject.transform.localScale += small;
                
            }
            else if (i == 1) //medium
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.red;
                newblock.gameObject.transform.localScale += medium;
            }
            else //large
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.green;
                newblock.gameObject.transform.localScale += large;
            }
            newblock.gameObject.transform.localScale *= sizeMultiplier;
            Block2 newcylinder = newblock.GetComponent<Block2>();
            newcylinder.index = dir;
            newcylinder.prev = false;
            blockscore = calculateScore(newpos, 8 - i, shape);
            newcylinder.reward = blockscore;
        }
        else if (i >= 3 && i <= 5) //cubes
        {
            shape = 0;
            GameObject newblock = (GameObject) Instantiate(cube, newpos, Quaternion.identity);
            if (i == 3) //small
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                newblock.gameObject.transform.localScale += small;
            }
            else if (i == 4) //medium
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                newblock.gameObject.transform.localScale += medium;
            }
            else //large
            {
                newblock.gameObject.GetComponent<Renderer>().material.color = Color.black;
                newblock.gameObject.transform.localScale += large;
            }
            newblock.gameObject.transform.localScale *= sizeMultiplier;
            Block1 newcube = newblock.GetComponent<Block1>();
            newcube.index = dir;
            newcube.prev = false;
            blockscore = calculateScore(newpos, 8 - i, shape);
            newcube.reward = blockscore;
        }
        else if (i == 6){ //freezeBlock
            shape = 1;
            GameObject newblock = (GameObject)Instantiate(freezeBlock, newpos, Quaternion.identity);
            FreezeBlock newcube = newblock.GetComponent<FreezeBlock>();
            newcube.index = dir;
            newcube.prev = false;
            blockscore = calculateScore(newpos, 8 - i, shape);
            newcube.reward = blockscore;
        }
        else if (i== 7){ //multiplierBlock
            shape = 1;
            GameObject newblock = (GameObject)Instantiate(multiBlock, newpos, Quaternion.identity);
            MultiBlock newcube = newblock.GetComponent<MultiBlock>();
            newblock.gameObject.transform.localScale *= sizeMultiplier;
            newcube.index = dir;
            newcube.prev = false;
            blockscore = calculateScore(newpos, 8 - i, shape);
            newcube.reward = blockscore;
        }
    }

    public float getSizeMultiplier()
    {
        int level = blocknumber / 5;
        if (blocknumber < 40)
        {
            float multiplier = 1.0f - 0.1f * level;
            return multiplier;
        }
        return 0.2f;
    }

    private int calculateScore(Vector3 pos, int size, int shape)
    {
        int distance =
            Mathf.FloorToInt((pos - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude);

        int score = 1 + Mathf.FloorToInt(0.2f * size) + shape + Mathf.FloorToInt(0.02f * distance);

        return score;
    }
    
    IEnumerator ShowMessage (string message, float delay, int index) {
        if (index == 1)
        {
            scoreText.text = message;
            scoreText.enabled = true;
            yield return new WaitForSeconds(delay);
            scoreText.enabled = false;
        }
        else if (index == 2)
        {
            highestText.text = message;
            highestText.enabled = true;
            yield return new WaitForSeconds(delay);
            highestText.enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        else if (index == 3)
        {
            centerText.text = message;
            centerText.enabled = true;
            yield return new WaitForSeconds(delay);
            centerText.enabled = false;

        }

    }
}