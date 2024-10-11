using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody2D distanceJointObject; // The object connected with the distance joint
    public Rigidbody2D hingeJointObject; // Objects connected with the hinge joints

    public float moveForce = 5f;    // Movement speed
    public float rotateForce = 100f; // Rotation speed

    private void Update()
    {
        // Horizontal movement for the object controlled by the distance joint
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            distanceJointObject.AddForce(Vector2.left * moveForce);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            distanceJointObject.AddForce(Vector2.right * moveForce);
        }

        // Rotation of the first hinge joint based on up/down keys
        if (Input.GetKey(KeyCode.UpArrow))
        {
            hingeJointObject.AddTorque(rotateForce);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            hingeJointObject.AddTorque(-rotateForce);
        }
    }
}
