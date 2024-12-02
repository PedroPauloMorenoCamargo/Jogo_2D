using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.EventSystems;


public class Player_Movement : MonoBehaviour
{
    public PauseMenu PauseMenu;
    // Joints
    public List<Rigidbody2D> joints = new List<Rigidbody2D>();

    public Joystick joystick;

    // Movimentação Horizontal
    public float moveForce = 5f;

    // Rotação
    public float rotateForce = 100f;

    // Pulo Força
    public float launchForce = 10f;
    public AudioSource jumpAudio;
    public AudioSource fallAudio;

    // Bolas de Mira
    public GameObject balls;

    // Botão de ativação das bolas
    public Toggle ballToggle;

    // Sprites para os joints no final
    public Sprite joint0EndSprite;
    public Sprite joint1EndSprite;
    public Sprite joint2EndSprite;
    public Sprite joint3EndSprite;

    // Volume global
    public Volume globalVolume;
    private Vignette vignette;

    // Checa se o player está no chão
    public bool isGrounded = false;

    // Verifica se o player pode se mover
    public bool can_move = false;

    // Força de impacto para o shake da câmera
    public float impactThreshold = 5f;

    // GameObjects
    public GameObject circle;
    public GameObject Title;

    // UI Button para o pulo
    public Button jumpButton;
    public Button restartButton;

    // Variáveis privadas
    private List<SpriteRenderer> jointRenderers = new List<SpriteRenderer>();
    private int groundedCount = 0;
    private bool ballShown = false;
    private float rest_pos = -45.0f;
    private CameraFollow cameraFollow;
    private float player_last_y_position;
    private bool isEnding = false;

    private Vector2[] jointPositions;

    private Quaternion[] jointRotations;

    public GameObject fallUI;

    public GameObject normalUI;
    public bool hasWatchedAd = false;

    public Button exitButton;

    

    private void Start()
    {
        // Configurar os eventos do botão de pulo
        if (jumpButton != null)
        {
            EventTrigger trigger = jumpButton.gameObject.AddComponent<EventTrigger>();

            // Ao pressionar o botão, exibe as bolas
            EventTrigger.Entry onPressEntry = new EventTrigger.Entry();
            onPressEntry.eventID = EventTriggerType.PointerDown;
            onPressEntry.callback.AddListener((eventData) => OnJumpButtonPressed());
            trigger.triggers.Add(onPressEntry);

            // Ao soltar o botão, realiza o pulo e oculta as bolas
            EventTrigger.Entry onReleaseEntry = new EventTrigger.Entry();
            onReleaseEntry.eventID = EventTriggerType.PointerUp;
            onReleaseEntry.callback.AddListener((eventData) => OnJumpButtonReleased());
            trigger.triggers.Add(onReleaseEntry);
        }

        exitButton.onClick.AddListener(exitButtonFunction);
        // Pega a posição Y inicial do player
        player_last_y_position = joints[0].transform.position.y;

        // Pega o componente CameraFollow
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        jointPositions = new Vector2[joints.Count];
        jointRotations = new Quaternion[joints.Count];

        if (fallUI != null)
        {
            fallUI.SetActive(false);
        }


        // Inicializa as bolas
        if (ballToggle != null && !ballToggle.isOn)
        {
            balls.SetActive(false);
        }

        // Pega os renderizadores dos joints
        for (int i = 0; i < joints.Count; i++)
        {
            SpriteRenderer sr = joints[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                jointRenderers.Add(sr);
            }
        }

        // Pesquisa o Vignette no Volume Global
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
        
        // Checa se está no final
        if (isEnding)
            return;

        // Checa se o player resetou a posição
        // Configurar o botão de restart
        if (hasWatchedAd){
            ResetToLastJumpPosition();
            hasWatchedAd = false; // Reset the boolean
            fallUI.SetActive(false);
            normalUI.SetActive(true);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(Reset_Position);
        }

        if (ballShown && ballToggle.isOn)
        {
            UpdateBallPositions();
        }
    }

    private void ResetToLastJumpPosition()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].transform.position = jointPositions[i];
            joints[i].transform.rotation = jointRotations[i];
            joints[i].velocity = Vector2.zero;
            joints[i].angularVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Checa se está no final
        if (isEnding)
            return;

        // Checa se o player pode se mover
        if (can_move)
        {
            Move();
        }
    }

    private void Move()
    {
        float horizontalInput = joystick.Horizontal;
        joints[0].AddForce(Vector2.right * moveForce * horizontalInput * Time.fixedDeltaTime);

        float verticalInput = joystick.Vertical;
        joints[3].AddTorque(rotateForce * verticalInput * Time.fixedDeltaTime);
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

        Vector2 offsetPosition = headPosition + tailDirection * 1.5f;
        balls.transform.position = offsetPosition;
        balls.transform.rotation = joints[3].transform.rotation;
    }

    private void Reset_Position()
    {
        joints[0].transform.position = new Vector2(-19.0f - 2 + rest_pos, 5.0f);
        joints[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[0].velocity = Vector2.zero;

        joints[1].transform.position = new Vector2(-17.776f - 2 + rest_pos, 5.0f);
        joints[1].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[1].velocity = Vector2.zero;

        joints[2].transform.position = new Vector2(-16.372f - 2 + rest_pos, 5.0f);
        joints[2].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[2].velocity = Vector2.zero;

        joints[3].transform.position = new Vector2(-15.016f - 2 + rest_pos, 5.0f);
        joints[3].transform.rotation = Quaternion.Euler(0, 0, 0);
        joints[3].velocity = Vector2.zero;
    }

    private void Launch()
    {
        for (int i = 0; i < joints.Count; i++){
            jointPositions[i] = joints[i].transform.position;
            jointRotations[i] = joints[i].transform.rotation;
        }

        Vector2 launchDirection = joints[0].transform.right;
        joints[0].AddForce(launchDirection * launchForce, ForceMode2D.Impulse);

        if (jumpAudio != null)
        {
            jumpAudio.Play();
        }
    }

    public void OnJumpButtonPressed()
    {
        if (can_move && !ballShown && PauseMenu.isPaused == false){
            ShowBalls();
        }
    }

    public void OnJumpButtonReleased()
    {   
        if (ballShown && PauseMenu.isPaused == false){
            HideBalls();
        }  
        if (isGrounded && can_move && PauseMenu.isPaused == false)
        {
            Launch(); 
        }
    }

    public void OnChildCollisionWithGround()
    {
        if (Mathf.Abs(player_last_y_position - joints[0].transform.position.y) > 30)
        {
            if (fallAudio != null)
            {
                fallAudio.Play();
            }
            if (cameraFollow != null)
            {
                cameraFollow.ShakeCamera();
            } 
        }
        if(player_last_y_position - joints[0].transform.position.y > 30 && can_move){
            fallUI.SetActive(true);
            normalUI.SetActive(false);
        }
        if (can_move == false)
        {
            can_move = true;
            Title.tag = "Ground";
            player_last_y_position = joints[0].transform.position.y;
        }
        groundedCount++;
        isGrounded = true;
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

    public void OnChildCollisionWithEnd()
    {
        if (isEnding)
            return;

        isEnding = true;
        balls.SetActive(false);
        circle.SetActive(false);

        if (jointRenderers.Count >= 4)
        {
            jointRenderers[0].sprite = joint0EndSprite;
            jointRenderers[1].sprite = joint1EndSprite;
            jointRenderers[2].sprite = joint2EndSprite;
            jointRenderers[3].sprite = joint3EndSprite;
        }

        foreach (Rigidbody2D rb in joints)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false;
        }

        StartCoroutine(EndSequenceCoroutine());
    }

    private IEnumerator EndSequenceCoroutine()
    {
        float duration = 15f;
        float elapsed = 0f;

        if (vignette != null)
        {
            vignette.color.Override(Color.black);
            vignette.smoothness.Override(1f);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(0f, 1f, t);
            }

            yield return null;
        }

        if (vignette != null)
        {
            vignette.intensity.value = 1f;
        }

        SceneManager.LoadScene(0);
    }

    private void exitButtonFunction(){
        fallUI.SetActive(false);
        normalUI.SetActive(true);
        
    }
}
