using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public List<Rigidbody2D> joints = new List<Rigidbody2D>(); // List of joints: 0 = distance joint, 1-2 = hinge joints, 3 = head
    public float moveForce = 5f;    // Movement speed
    public float rotateForce = 100f; // Rotation speed for the head
    public float launchForce = 10f;  // Impulse force when launching

    private int groundedCount = 0;  // Counter for how many child objects are grounded
    public bool isGrounded = false;  // To check if any part is touching the ground

    private void Update()
    {
        // Horizontal movement for the object controlled by the distance joint
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            joints[0].AddForce(Vector2.left * moveForce);  // First item in the list (distance joint)
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            joints[0].AddForce(Vector2.right * moveForce);
        }

        // Rotation of the head (last item in the list)
        if (Input.GetKey(KeyCode.UpArrow))
        {
            joints[3].AddTorque(rotateForce);  // Apply torque to the head (4th item in the list)
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            joints[3].AddTorque(-rotateForce);  // Apply torque to the head
        }

        // If the object is grounded and a launch key is pressed (e.g., Space), apply impulse
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }

    // Function to launch the object along the X-axis
    private void Launch()
    {
        Vector2 launchDirection = joints[0].transform.right;  // X-axis of the distance joint object
        joints[0].AddForce(launchDirection * launchForce, ForceMode2D.Impulse);  // Launch the distance joint object
    }

    // These methods are called by the child collision scripts to notify the parent about collisions
    public void OnChildCollisionWithGround()
    {
        // Increment the grounded count when a child touches the ground
        groundedCount++;
        isGrounded = true;  // As long as at least one child is on the ground, isGrounded is true
    }

    public void OnChildCollisionExitGround()
    {
        // Decrement the grounded count when a child leaves the ground
        groundedCount--;
        
        // If no child is on the ground, set isGrounded to false
        if (groundedCount <= 0)
        {
            isGrounded = false;
            groundedCount = 0;  // Ensure the grounded count doesn't go below 0
        }
        
    }
}
