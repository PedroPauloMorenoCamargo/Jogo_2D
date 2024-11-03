using UnityEngine;

public class BackgroundColorLerp : MonoBehaviour
{
    public Color color1 = Color.red;   
    public Color color2 = Color.blue; 
    public float duration = 5.0f;      

    private Camera cam;
    private float lerpTime;

    void Start()
    {
        cam = Camera.main;  
        lerpTime = 0f;      
    }

    void Update()
    {
        //Incrementa o tempo de interpolação
        lerpTime += Time.deltaTime;

        // Calcula o valor de t entre 0 e 1
        float t = Mathf.PingPong(lerpTime / duration, 1f);

        // Interpola a cor de fundo da câmera
        cam.backgroundColor = Color.Lerp(color1, color2, t);
    }
}
