using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeightBasedMusicController : MonoBehaviour
{
    public Transform[] playerJoints = new Transform[4];
    public AudioSource groundMusic;
    public AudioSource spaceMusic;
    public Slider ambientMusicSlider;
    public float transitionHeight = -70f;
    public float fadeDuration = 1.0f;

    private bool isInSpace = false;
    public bool gameStarted = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (groundMusic != null && spaceMusic != null)
        {
            if (ambientMusicSlider != null)
            {
                if (ambientMusicSlider.value == 0)
                {
                    ambientMusicSlider.value = 1.0f;
                }
                ambientMusicSlider.onValueChanged.AddListener(UpdateAmbientVolume);
            }

            groundMusic.volume = 1.0f * ambientMusicSlider.value;
            spaceMusic.volume = 0.0f;
            groundMusic.Play();
            spaceMusic.Play();

        }
    }

    void Update()
    {
        if (!gameStarted){
            return;
        }

        //Calcula a altura média dos joints do jogador
        float meanHeight = 0f;
        foreach (Transform joint in playerJoints)
        {
            meanHeight += joint.position.y;
        }
        meanHeight /= playerJoints.Length;

        //Verifica se a altura média ultrapassou a altura de transição
        if (meanHeight >= transitionHeight && !isInSpace)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeMusic(groundMusic, spaceMusic));
            isInSpace = true;
        }
        else if (meanHeight < transitionHeight && isInSpace)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeMusic(spaceMusic, groundMusic));
            isInSpace = false;
        }
    }

    private IEnumerator FadeMusic(AudioSource fromMusic, AudioSource toMusic){
        //Fade entre as músicas
        float startVolume = fromMusic.volume;
        float endVolume = toMusic.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fromMusic.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration) * ambientMusicSlider.value;
            toMusic.volume = Mathf.Lerp(endVolume, 1, elapsedTime / fadeDuration) * ambientMusicSlider.value;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fromMusic.volume = 0;
        toMusic.volume = 1 * ambientMusicSlider.value;
    }

    void UpdateAmbientVolume(float value)
    {
        if (isInSpace)
        {
            spaceMusic.volume = value;
        }
        else
        {
            groundMusic.volume = value;
        }
    }

    void OnDestroy()
    {
        if (ambientMusicSlider != null)
        {
            ambientMusicSlider.onValueChanged.RemoveListener(UpdateAmbientVolume);
        }
    }
}
