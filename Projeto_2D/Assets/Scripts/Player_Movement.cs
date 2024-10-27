using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public List<Rigidbody2D> joints = new List<Rigidbody2D>(); // List of joints: 0 = distance joint, 1-2 = hinge joints, 3 = head
    public float moveForce = 5f;    // Movement speed
    public float rotateForce = 100f; // Rotation speed for the head
    public float launchForce = 10f;  // Impulse force when launching
    public AudioSource jumpAudio;  // Audio source for the jump sound
    public GameObject balls; // List of balls (Bola_1, Bola_2, Bola_3)

    private int groundedCount = 0;  // Counter for how many child objects are grounded
    public bool isGrounded = false;  // To check if any part is touching the ground

    public bool can_move = false;  // Toggle to enable/disable movement
    private bool ballShown = false; // Toggle to show/hide balls

    private float rest_pos = -45.0f;  // Resting position of the head

    public float impactThreshold = 5f; // Minimum impact velocity to trigger camera shake
    private CameraFollow cameraFollow; // Reference to CameraFollow script

    private float player_last_y_position;

    private void Start(){   
        player_last_y_position = joints[0].transform.position.y;
        // Get reference to the CameraFollow component on the main camera
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Reset_Position();
        }
        
        if (can_move){
            Move();
            if (Input.GetKeyUp(KeyCode.Space)){
                if(isGrounded){
                    Launch();
                }
                HideBalls();
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                ShowBalls();
            }

            if (ballShown){
                UpdateBallPositions();
            }
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            joints[0].AddForce(Vector2.left * moveForce);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            joints[0].AddForce(Vector2.right * moveForce);
        }

        // Rotation of the head (last item in the list)
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            joints[3].AddTorque(rotateForce);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            joints[3].AddTorque(-rotateForce);
        }
    }

    private void ShowBalls()
    {   
        ballShown = true;
        balls.SetActive(true);
    }

    private void HideBalls()
    {
        ballShown = false;
        balls.SetActive(false);
    }

    private void UpdateBallPositions()
    {
        Vector2 headPosition = joints[3].transform.position;
        Vector2 tailDirection = (joints[3].transform.position - joints[0].transform.position).normalized;

        Vector2 offsetPosition = headPosition + tailDirection * (1.5f);
        balls.transform.position = offsetPosition;
        balls.transform.rotation = joints[3].transform.rotation;
    }
    private void Reset_Position()
    {
        joints[0].transform.position = new Vector2(-15.0f + rest_pos, 5.0f);
        joints[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[0].velocity = Vector2.zero;
        joints[1].transform.position = new Vector2(-13.98f + rest_pos, 5.0f);
        joints[1].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[1].velocity = Vector2.zero;
        joints[2].transform.position = new Vector2(-12.81f + rest_pos, 5.0f);
        joints[2].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[2].velocity = Vector2.zero;
        joints[3].transform.position = new Vector2(-11.68f + rest_pos, 5.0f);
        joints[3].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[3].velocity = Vector2.zero;
    }

    private void Launch()
    {
        Vector2 launchDirection = joints[0].transform.right;
        joints[0].AddForce(launchDirection * launchForce, ForceMode2D.Impulse);

        if (jumpAudio != null)
        {
            jumpAudio.Play();
        }
    }

    public void OnChildCollisionWithGround(){
        if (can_move == false){
            can_move = true;
        }
        groundedCount++;
        isGrounded = true;
        if (Mathf.Abs(player_last_y_position - joints[0].transform.position.y)>30){
            cameraFollow.ShakeCamera();
        }
    }

    public void OnChildCollisionExitGround()
    {
        groundedCount--;

        if (groundedCount <= 0){
            isGrounded = false;
            groundedCount = 0;
        }

        player_last_y_position = joints[0].transform.position.y;
    }
}
