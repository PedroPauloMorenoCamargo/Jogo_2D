using UnityEngine;
using System.Collections.Generic;

public class Ball_Movement : MonoBehaviour
{
    public Vector2 velocity;
    public float speed = 2f;
    private float radius;

    private List<Ball_Movement> allBalls; // To keep track of all the balls

    void Start()
    {
        // Get the radius of the ball based on its scale
        radius = GetComponent<SpriteRenderer>().bounds.extents.x;

        // Give the ball a random initial velocity
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;

        // Find all balls in the scene
        allBalls = new List<Ball_Movement>(FindObjectsOfType<Ball_Movement>());
    }

    void Update()
    {
        // Move the ball
        transform.Translate(velocity * Time.deltaTime);

        // Handle screen edge collisions
        HandleWallCollisions();

        // Handle collisions with other balls
        HandleBallCollisions();
    }

    void HandleWallCollisions()
{
    Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

    if (transform.position.x < -screenBounds.x)
    {
        velocity.x = -velocity.x; // Reverse the X velocity
        // Allow the ball to continue moving smoothly by keeping it exactly at the wall
        transform.position = new Vector3(-screenBounds.x, transform.position.y, transform.position.z);
    }
    else if (transform.position.x > screenBounds.x)
    {
        velocity.x = -velocity.x; // Reverse the X velocity
        // Allow the ball to continue moving smoothly by keeping it exactly at the wall
        transform.position = new Vector3(screenBounds.x, transform.position.y, transform.position.z);
    }

    // Check for collisions with top/bottom walls (when the center of the ball reaches the wall)
    if (transform.position.y < -screenBounds.y)
    {
        velocity.y = -velocity.y; // Reverse the Y velocity
        // Allow the ball to continue moving smoothly by keeping it exactly at the wall
        transform.position = new Vector3(transform.position.x, -screenBounds.y, transform.position.z);
    }
    else if (transform.position.y > screenBounds.y)
    {
        velocity.y = -velocity.y; // Reverse the Y velocity
        // Allow the ball to continue moving smoothly by keeping it exactly at the wall
        transform.position = new Vector3(transform.position.x, screenBounds.y, transform.position.z);
    }
}


    void HandleBallCollisions()
    {
        foreach (Ball_Movement otherBall in allBalls)
        {
            // Avoid self-collision
            if (otherBall == this) continue;

            // Check if the distance between the two balls is less than the sum of their radii (i.e., they are colliding)
            float distance = Vector2.Distance(transform.position, otherBall.transform.position);
            if (distance < radius + otherBall.radius)
            {
                // Swap velocities to simulate a bounce
                Vector2 tempVelocity = velocity;
                velocity = otherBall.velocity;
                otherBall.velocity = tempVelocity;

                // Move balls slightly apart to prevent them from sticking together
                Vector2 direction = (transform.position - otherBall.transform.position).normalized;
                transform.position += (Vector3)(direction * 0.01f);
                otherBall.transform.position -= (Vector3)(direction * 0.01f);
            }
        }
    }
}
