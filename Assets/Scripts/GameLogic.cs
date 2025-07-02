using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public Animator StartGameAnimation;
    public Animator ResetGameAnimation;
    public bool gameIsStarted = false;
    public GameObject GameOverScreen;
    public GameObject GameScreen;
    public GameObject StartGameScreen;
    public Transform Square;
    public float fadeDuration = 0.5f;
    private float startTime;
    public Text[] time;
    public AudioSource gameSound;
    public SoundScript sound;

    public CanvasGroup cg;

    // Start is called before the first frame update
    void Start()
    {
        cg = GameOverScreen.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = GameOverScreen.AddComponent<CanvasGroup>();
        }
        sound = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundScript>();

        // Hide & deactivate on start

        cg.alpha = 0f;
        BackHome();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsStarted)
        {
            float elapsed = Time.time - startTime;

            int hours = (int)(elapsed / 3600);
            int minutes = (int)((elapsed % 3600) / 60);
            int seconds = (int)(elapsed % 60);

            for (int i = 0; i < time.Length; ++i) {
                time[i].text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
            }
        }

    }

    public void BackHome()
    {
        gameSound.Play();

        if (!(sound.isSoundOn))
        {
            gameSound.Pause();
        }

        GameOverScreen.SetActive(false);
        GameScreen.SetActive(false);
        StartGameScreen.SetActive(true);

    }

    public void StartGame()
    {
        gameSound.Stop();

        if (!gameIsStarted)
        {
            StartCoroutine(StartGameWithDelay());
        }

    }

    private IEnumerator StartGameWithDelay()
    {
        // Play animation

        StartGameAnimation.Play("Start Game", 0, 0.0f);
        gameIsStarted = true;

        yield return new WaitForSeconds(0.5f); // Now this delays the reset

        // After delay — reset visuals and freeze

        StartGameScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        GameScreen.SetActive(true);
        startTime = Time.time;
    }

    public void gameOver()
    {
        gameIsStarted = false;
        StartGameScreen.SetActive(false);
        GameOverScreen.SetActive(true);
        GameScreen.SetActive(false);
        StartCoroutine(FadeIn());
        Square.position = Vector3.zero;
    }
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f; // ensure fully visible
    }

    public void resetGame()
    {
        if (!gameIsStarted)
        {
            // Start coroutine that handles everything
            StartCoroutine(ResetWithDelay());
        }
    }

    private IEnumerator ResetWithDelay()
    {
        // Play animation

        ResetGameAnimation.Play("Reset Game", 0, 0.0f);
        gameIsStarted = true;

        yield return new WaitForSeconds(0.5f); // Now this delays the reset

        // After delay — reset visuals and freeze

        StartGameScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        GameScreen.SetActive(true);
        startTime = Time.time;
    }
}
