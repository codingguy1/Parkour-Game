using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody playerRB;
    Transform rotatation;
   
    //Basic movement
    private float YMovement;
    private float XMovement;
    public float groundSpeed;
    public float sprintSpeed;
    public float airSpeed;
    private Vector3 MovementDirection;

    //Jumping
    private bool onGround;
    public float jumpHeight;
    public float gravity;


    //Changing drag
    public float groundDrag;
    public float airDrag;
    // Wall running
    public float wallDistance;
    public float wallRunGravity;
    public float minimumHeight;
    public float wallRunJump;
    bool isWallRunning;
    bool leftWall;
    bool rightWall;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;



    private void Start()
    {
        // finds the player rigidbody and transform every time it is created.
        // makes sure that the player can still move after being respawned
        playerRB = GetComponent<Rigidbody>();
        rotatation = GetComponent<Transform>();

        // makes sure that the player stays upright
        playerRB.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        // checks if a ray pointed down hits the ground
        onGround = Physics.Raycast(transform.position, Vector3.down, 4 / 2f + 0.1f);

        changeDrag();
        updateWalls();
        if (CanWallRun())
        {
            if (leftWall)
            {
                WallRunStart();
                Debug.Log("wall running on the left");
            }
            else if (rightWall)
            {
                WallRunStart();
                Debug.Log("wall running on the right");
            }
            else
            {
                WallRunStop();
            }
        }
        else
        {
            WallRunStop();
        }
        if(transform.position.y < -150f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        


    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }
    void GetInput()
    {
        // Get Axis Raw returns 1 if W or D is pressed and returns -1 if A or S is pressed
        YMovement = Input.GetAxisRaw("Horizontal");

        XMovement = Input.GetAxisRaw("Vertical");
        // multiply by transform.forward and transform.right in order to move relative to where the player is looking.
        MovementDirection = transform.forward * XMovement + transform.right * YMovement;
    }
    void PlayerMovement()
    {
        //if the player is wallrunning, decreases gravity
        if(isWallRunning)
        {
            playerRB.AddForce(0, -wallRunGravity, 0, ForceMode.Acceleration);
        }
        //adds gravity normally
        playerRB.AddForce(0, -gravity, 0, ForceMode.Acceleration);
        
        if (onGround)
        {
            if (Input.GetKey("left shift"))
            {
                playerRB.AddForce(MovementDirection.normalized * groundSpeed * sprintSpeed, ForceMode.Acceleration);

            }
            else
            { 
                //Adds force using normalized movement direction so you don't go faster when pressing 2 movement keys;
                playerRB.AddForce(MovementDirection.normalized * groundSpeed, ForceMode.Acceleration);
            }

        }
        else
        {
            playerRB.AddForce(MovementDirection.normalized * airSpeed, ForceMode.Acceleration);

        }
        // checks if player is on the ground before allowing them to jump.
        if (onGround == true && Input.GetKey("space"))
        {
            //adds upwords force to the player
            playerRB.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
        }
        //Wall running
        
    }
    void changeDrag()
    {
        // Changes drag so the player isn't able to move too fast in the air.
        if (onGround)
        {
            playerRB.drag = groundDrag;
        }
        if (onGround == false)
        {
            playerRB.drag = airDrag;
        }
    }
    void updateWalls()
    {
        //checks if you are close enough to a wall by shooting a ray to the left or right with a length of wallDistance
        //stores the rigid body of the wall that was hit in the RayCastHit variables so that the jump off the wall can be caculated later
        leftWall = Physics.Raycast(playerRB.position, -rotatation.right,out leftWallHit, wallDistance);
        rightWall = Physics.Raycast(playerRB.position, rotatation.right, out rightWallHit, wallDistance);
    }
    bool CanWallRun()
    {
        //checks if we are a high enough distance from the ground
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight);
    }
    void WallRunStart()
    {
        isWallRunning = true;
        playerRB.useGravity = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 up = new Vector3(0, 1f, 0);
            if (leftWall)
            {
                //adds a vector that is perpindicular to the wall to a vector pointing straight up
                //This creates a diagnoal vector that can be used as the direction for the jump off the wall
                Vector3 wallRunJumpDirection = up + leftWallHit.normal;
                //Resets the player's Y velocity so it doesn't interfere with the jump
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
                playerRB.AddForce(wallRunJumpDirection * wallRunJump * 100, ForceMode.Force);
            }
            else if (rightWall)
            {
                // creates a new vector3 with the 
                Vector3 wallRunJumpDirection = up + rightWallHit.normal;
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
                playerRB.AddForce(wallRunJumpDirection * wallRunJump * 100, ForceMode.Force);
            }
        }


    }
    void WallRunStop()
    {

        isWallRunning = false;
        playerRB.useGravity = true;
    }
    




}
