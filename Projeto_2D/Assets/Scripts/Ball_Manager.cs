using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public int numberOfBalls = 5;

    void Start()
    {
        for (int i = 0; i < numberOfBalls; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));

            // Instantiate each ball and set the parent to this BallManager GameObject
            GameObject ball = Instantiate(ballPrefab, randomPosition, Quaternion.identity);
            ball.transform.SetParent(transform); // Set the parent to the BallManager
        }
    }
}
