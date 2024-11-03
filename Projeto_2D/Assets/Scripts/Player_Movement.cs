using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 

public class Player_Movement : MonoBehaviour{

    //Joints
    public List<Rigidbody2D> joints = new List<Rigidbody2D>();

    //Movimentação Horizontal
    public float moveForce = 5f;

    //Rotação
    public float rotateForce = 100f;

    //Pulo Força
    public float launchForce = 10f;  
    public AudioSource jumpAudio;
    public AudioSource fallAudio;

    //Bolas de Mira
    public GameObject balls; 

    //Botão de ativação das bolas
    public Toggle ballToggle; 

    // Sprites para os joints no final
    public Sprite joint0EndSprite;
    public Sprite joint1EndSprite;
    public Sprite joint2EndSprite;
    public Sprite joint3EndSprite;

    // Volume global
    public Volume globalVolume; 
    private Vignette vignette;

    //Checa se o player está no chão
    public bool isGrounded = false;

    //Verifica se o player pode se mover
    public bool can_move = false;

    // Força de impacto para o shake da câmera
    public float impactThreshold = 5f;

    //GameObjects
    public GameObject circle;

    public GameObject Title;

    // Variáveis privadas
    private List<SpriteRenderer> jointRenderers = new List<SpriteRenderer>();
    private int groundedCount = 0;
    private bool ballShown = false;
    private float rest_pos = -45.0f;
    private CameraFollow cameraFollow;
    private float player_last_y_position;
    private bool isEnding = false;

    private void Start(){
        // Pega a posição Y inicial do player
        player_last_y_position = joints[0].transform.position.y;
        // Pega o componente CameraFollow
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        //Inicializa as bolas
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

        //Pesquisa o Vignette no Volume Global
        if (globalVolume != null && globalVolume.profile != null)
        {
            if (!globalVolume.profile.TryGet(out vignette))
            {
                Debug.LogError("Vignette not found in Global Volume profile.");
            }

        }
    }

    private void Update(){
        //Checa se está no final
        if (isEnding)
            return; 

        //Checa se o player resetou a posição
        if (Input.GetKeyDown(KeyCode.C))
        {
            Reset_Position();
        }

        //Checa se o player pode se mover
        if (can_move){
            //Movimentação do player
            Move();
            //Handler do pulo e da mira
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

    private void Move(){
        // Movimentação horizontal
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            joints[0].AddForce(Vector2.left * moveForce);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            joints[0].AddForce(Vector2.right * moveForce);
        }

        // Rotação
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            joints[3].AddTorque(rotateForce);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            joints[3].AddTorque(-rotateForce);
        }
    }

    private void ShowBalls(){
        ballShown = true;
        balls.SetActive(true);
    }

    private void HideBalls(){
        ballShown = false;
        balls.SetActive(false);
    }

    private void UpdateBallPositions(){
        // Posiciona as bolas de mira conforme a posição da cabeça e a direção da cauda
        Vector2 headPosition = joints[3].transform.position;
        Vector2 tailDirection = (joints[3].transform.position - joints[0].transform.position).normalized;

        Vector2 offsetPosition = headPosition + tailDirection * (1.5f);
        balls.transform.position = offsetPosition;
        balls.transform.rotation = joints[3].transform.rotation;
    }

    private void Reset_Position(){
        // Reseta a posição dos joints
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

    private void Launch(){
        // Pulo
        Vector2 launchDirection = joints[0].transform.right;
        joints[0].AddForce(launchDirection * launchForce, ForceMode2D.Impulse);

        if (jumpAudio != null)
        {
            jumpAudio.Play();
        }
    }

    public void OnChildCollisionWithGround(){
        //Checa colisão com o chão ou com a plataforma
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

    public void OnChildCollisionExitGround(){
        //Checa se o player saiu do chão oy da plataforma
        groundedCount--;

        if (groundedCount <= 0)
        {
            isGrounded = false;
            groundedCount = 0;
        }

        player_last_y_position = joints[0].transform.position.y;
    }

    public void OnChildCollisionWithEnd(){
        // Checa se o player chegou ao final
        if (isEnding)
            return;

        isEnding = true;
        balls.SetActive(false);
        circle.SetActive(false); 

        // Troca os sprites dos joints
        if (jointRenderers.Count >= 4)
        {
            jointRenderers[0].sprite = joint0EndSprite;
            jointRenderers[1].sprite = joint1EndSprite;
            jointRenderers[2].sprite = joint2EndSprite;
            jointRenderers[3].sprite = joint3EndSprite;
        }

        // Desativa os joints
        foreach (Rigidbody2D rb in joints)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false;
        }

        // Começa a sequência de fim
        StartCoroutine(EndSequenceCoroutine());
    }

    private IEnumerator EndSequenceCoroutine(){
        // Corrotina de fim
        float duration = 15f;
        float elapsed = 0f;

        vignette.color.Override(Color.black);
        vignette.smoothness.Override(1f);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Interpola o Vignette
            if (vignette != null){
                vignette.intensity.value = Mathf.Lerp(0f, 1f, t);
            }

            yield return null;
        }

        // Checa se o Vignette não é nulo
        if (vignette != null)
        {
            vignette.intensity.value = 1f;
        }

        // Loada o menu principal
        SceneManager.LoadScene(0);
    }
}
