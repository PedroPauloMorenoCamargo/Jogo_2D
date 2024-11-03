using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // For URP

public class Player_Movement : MonoBehaviour
{
    public List<Rigidbody2D> joints = new List<Rigidbody2D>(); // Joints: 0 = tail, 1-2 = body, 3 = head
    public float moveForce = 5f;    // Movement speed
    public float rotateForce = 100f; // Rotation speed for the head
    public float launchForce = 10f;  // Impulse force when launching
    public AudioSource jumpAudio;
    public AudioSource fallAudio;
    public GameObject balls; // Balls (Bola_1, Bola_2, Bola_3)
    public Toggle ballToggle; // UI Toggle for ball handling

    // New sprites for each joint after collision with "End"
    public Sprite joint0EndSprite;
    public Sprite joint1EndSprite;
    public Sprite joint2EndSprite;
    public Sprite joint3EndSprite;

    // Vignette effect
    public Volume globalVolume; // Reference to Global Volume
    private Vignette vignette;

    private List<SpriteRenderer> jointRenderers = new List<SpriteRenderer>();
    private int groundedCount = 0;
    public bool isGrounded = false;
    public bool can_move = false;
    private bool ballShown = false;
    private float rest_pos = -45.0f;
    public float impactThreshold = 5f;
    private CameraFollow cameraFollow;
    private float player_last_y_position;
    private bool isEnding = false;
    
    public GameObject circle;

    public GameObject Title;

    private void Start()
    {
        player_last_y_position = joints[0].transform.position.y;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        // Initially hide balls if ballToggle is not enabled
        if (ballToggle != null && !ballToggle.isOn)
        {
            balls.SetActive(false);
        }

        // Get SpriteRenderers from joints
        for (int i = 0; i < joints.Count; i++)
        {
            SpriteRenderer sr = joints[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                jointRenderers.Add(sr);
            }
        }

        // Get the Vignette effect from the Global Volume
        if (globalVolume != null && globalVolume.profile != null)
        {
            if (!globalVolume.profile.TryGet(out vignette))
            {
                Debug.LogError("Vignette not found in Global Volume profile.");
            }

        }
    }

    private void Update()
    {
        if (isEnding)
            return; // Disable controls during end sequence

        if (Input.GetKeyDown(KeyCode.C))
        {
            Reset_Position();
        }

        if (can_move)
        {
            Move();
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (isGrounded)
                {
                    Launch();
                }
                if (ballToggle.isOn)
                {
                    HideBalls();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ballToggle.isOn)
                {
                    ShowBalls();
                }
            }

            if (ballShown && ballToggle.isOn)
            {
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
        joints[0].transform.position = new Vector2(-19.0f -2  + rest_pos, 5.0f);
        joints[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[0].velocity = Vector2.zero;
        joints[1].transform.position = new Vector2(-17.776f -2+ rest_pos, 5.0f);
        joints[1].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[1].velocity = Vector2.zero;
        joints[2].transform.position = new Vector2(-16.372f  -2+ rest_pos, 5.0f);
        joints[2].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[2].velocity = Vector2.zero;
        joints[3].transform.position = new Vector2(-15.016f -2 + rest_pos, 5.0f);
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

    public void OnChildCollisionWithGround()
    {
        if (can_move == false)
        {
            can_move = true;
            Title.tag = "Ground";
        }
        groundedCount++;
        isGrounded = true;
        if (Mathf.Abs(player_last_y_position - joints[0].transform.position.y) > 30)
        {
            fallAudio.Play();
            cameraFollow.ShakeCamera();
        }
    }

    public void OnChildCollisionExitGround()
    {
        groundedCount--;

        if (groundedCount <= 0)
        {
            isGrounded = false;
            groundedCount = 0;
        }

        player_last_y_position = joints[0].transform.position.y;
    }

    // Method to handle collision with "End" object
    public void OnChildCollisionWithEnd()
    {
        if (isEnding)
            return;

        isEnding = true;
        balls.SetActive(false); // Hide balls on end
        circle.SetActive(false); // Hide circle on end

        // Change sprites of each joint
        if (jointRenderers.Count >= 4)
        {
            jointRenderers[0].sprite = joint0EndSprite;
            jointRenderers[1].sprite = joint1EndSprite;
            jointRenderers[2].sprite = joint2EndSprite;
            jointRenderers[3].sprite = joint3EndSprite;
        }

        // Stop physics on the player
        foreach (Rigidbody2D rb in joints)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false;
        }

        // Start the vignette effect and load scene after delay
        StartCoroutine(EndSequenceCoroutine());
    }

    private IEnumerator EndSequenceCoroutine()
    {
        float duration = 10f;
        float elapsed = 0f;

        vignette.color.Override(Color.black);
        vignette.smoothness.Override(1f);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Gradually increase the vignette intensity
            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(0f, 1f, t);
            }

            yield return null;
        }

        // Ensure the vignette is fully black
        if (vignette != null)
        {
            vignette.intensity.value = 1f;
        }

        // Load Scene 0
        SceneManager.LoadScene(0);
    }
}
