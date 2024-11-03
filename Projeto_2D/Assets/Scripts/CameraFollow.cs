using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform[] targets;  
    public Vector2 offset;  
    public float smoothSpeed = 0.125f;  

    private Vector3 velocity = Vector3.zero;  

    public Camera mainCamera;  
    public int followPlayer = 0; 

    public float zoomSpeed = 2f;  
    public float shakeDuration = 0.2f;  
    public float shakeMagnitude = 0.3f;  

    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    public float minX = -10f;
    
    private bool isShaking = false;  

    void LateUpdate()
    {
        if (followPlayer == 2 && !isShaking)
        {
            FollowPlayer();
        }
        else if (followPlayer == 1)
        {
            StartCoroutine(ZoomIn());
            followPlayer = 2;
            smoothSpeed = 1f;
        }
    }

    void FollowPlayer()
    {
        // Calcula a posição média dos targets
        Vector2 meanPosition = GetMeanPosition();

        // Calcula a posição alvo da câmera
        Vector3 targetPosition = new Vector3(meanPosition.x, meanPosition.y, transform.position.z);

        // Clampa a posição alvo para os limites da câmera
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);  
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);  

        // Move a câmera suavemente para a posição alvo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition + (Vector3)offset, ref velocity, smoothSpeed);
        
        smoothSpeed = 0.125f;
    }

    Vector2 GetMeanPosition()
    {
        Vector2 sum = Vector2.zero;
        foreach (Transform target in targets)
        {
            sum.x += target.position.x;
            sum.y += target.position.y;
        }

        return sum / targets.Length;
    }

    IEnumerator ZoomIn()
    {
        // Aplica o zoom in suavemente
        while (Mathf.Abs(mainCamera.orthographicSize - 9) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 9, Time.deltaTime * zoomSpeed);
            yield return null; 
        }

        mainCamera.orthographicSize = 9;
    }

    public void ShakeCamera()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

private IEnumerator Shake()
{
    isShaking = true;
    Vector3 originalPosition = transform.position;

    float elapsed = 0f;

    while (elapsed < shakeDuration)
    {
        // Calcula o target da câmera com shake
        Vector3 playerPosition = targets[0].position; 
        float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
        float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;

        // Faz a câmera seguir o player com shake
        transform.position = new Vector3(originalPosition.x + xOffset, playerPosition.y + yOffset, originalPosition.z);

        elapsed += Time.deltaTime;

        yield return null;  
    }


    isShaking = false;
}

}
