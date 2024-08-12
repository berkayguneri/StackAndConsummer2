using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSeven : MonoBehaviour
{
    public static GameManagerSeven instance;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI scoreText;
    public GameObject complete;
    public GameObject radialShine;
    public GameObject backGround;
    public GameObject restartButton;

    [HideInInspector] public int score;
    private int displayedScore = 0;
    private bool isAnimatingScore = false;
    private bool radiShine;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (radiShine == true)
        {
            radialShine.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 20f * Time.deltaTime));
        }
    }

    private void Start()
    {
        UpdateScore();
    }
    private void UpdateScore()
    {
        scoreText.text = "Score: " + displayedScore.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
    public void OpenWinPanel()
    {
        StartCoroutine(FinishLaunch());
        StartCoroutine(AnimateScore());
    }

    public void OpenLosePanel()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public IEnumerator FinishLaunch()
    {
        radiShine = true;
        winPanel.SetActive(true);
        backGround.SetActive(true);
        yield return new WaitForSecondsRealtime(.5f);
        complete.SetActive(true);
        yield return new WaitForSecondsRealtime(.4f);
        radialShine.SetActive(true);
        yield return new WaitForSecondsRealtime(.5f);
        scoreText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        restartButton.gameObject.SetActive(true);
    }

    private IEnumerator AnimateScore()
    {
        displayedScore = 0;
        int finalScore = score;
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            displayedScore = Mathf.FloorToInt(Mathf.Lerp(0, finalScore, elapsedTime / duration));
            UpdateScore();
            yield return null;
        }


        displayedScore = finalScore;
        //UpdateScore();


    }
}
