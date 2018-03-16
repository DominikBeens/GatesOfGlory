using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour 
{

    public static UIManager instance;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Animator screenFadeAnim;
    public Animator gameOverAnimator;
    public Transform gameOverCamSpawn;

    [Header("Pause")]
    public GameObject pausePanel;
    private bool canPause = true;

    [Header("Wave")]
    public TextMeshProUGUI waveText;
    public Image waveHealthFill;
    public TextMeshProUGUI waveHealthText;

    [Header("Other")]
    public GameObject UICam;
    public GameObject gameInfoPanel;
    public GameObject waveTimerPanel;
    public PlayableDirector startGameTLDirector;

    [Header("Not Enough Gold Icon")]
    public GameObject notEnoughGoldIcon;
    public float notEnoughGoldIconDisplayTime;
    private float notEnoughGoldDisplayTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (notEnoughGoldDisplayTimer > 0)
        {
            notEnoughGoldDisplayTimer -= Time.deltaTime;
            notEnoughGoldIcon.SetActive(true);
            notEnoughGoldIcon.transform.position = Input.mousePosition;
            Cursor.visible = false;
        }
        else
        {
            notEnoughGoldDisplayTimer = 0;
            notEnoughGoldIcon.SetActive(false);
            Cursor.visible = true;
        }
    }

    private IEnumerator StartGame()
    {
        canPause = false;

        CameraManager mainCamManager = Camera.main.GetComponent<CameraManager>();
        mainCamManager.enabled = false;
        UICam.SetActive(false);

        gameInfoPanel.SetActive(false);
        waveTimerPanel.SetActive(false);

        gameOverAnimator.enabled = true;

        yield return new WaitForSeconds(12);

        gameOverAnimator.enabled = false;
        mainCamManager.enabled = true;
        canPause = true;

        gameInfoPanel.SetActive(true);
        waveTimerPanel.SetActive(true);
        UICam.SetActive(true);

        GameManager.instance.gameState = GameManager.GameState.Playing;

        yield return new WaitForSeconds(2);

        startGameTLDirector.enabled = false;
        WaveManager.instance.NextWave();
    }

    public IEnumerator GameOver()
    {
        CastleUpgradeManager.instance.CloseAllUI();
        canPause = false;
        GameManager.instance.gameState = GameManager.GameState.Cinematic;

        CameraManager mainCamManager = Camera.main.GetComponent<CameraManager>();
        mainCamManager.enabled = false;

        screenFadeAnim.SetTrigger("Fade");

        yield return new WaitForSeconds(1.2f);

        //float speed = 0;
        //while (Vector3.Distance(mainCamManager.transform.position, gameOverCamSpawn.position) > 0.1f)
        //{
        //    if (speed < 20)
        //    {
        //        speed += 0.5f;
        //    }
        //    mainCamManager.transform.position = Vector3.MoveTowards(mainCamManager.transform.position, gameOverCamSpawn.position, Time.deltaTime * speed);
        //    mainCamManager.transform.rotation = Quaternion.RotateTowards(mainCamManager.transform.rotation, gameOverCamSpawn.rotation, Time.deltaTime * 30);

        //    if (Camera.main.fieldOfView < mainCamManager.maxZoomOut)
        //    {
        //        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, mainCamManager.maxZoomOut, Time.deltaTime * 3);
        //    }
        //    yield return null;
        //}

        gameInfoPanel.SetActive(false);
        waveTimerPanel.SetActive(false);
        UICam.SetActive(false);
        Camera.main.fieldOfView = 60;
        gameOverAnimator.enabled = true;
        gameOverAnimator.SetTrigger("End");

        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(gameOverAnimator.GetCurrentAnimatorStateInfo(0).length);

        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void PauseButton()
    {
        if (!canPause)
        {
            return;
        }

        if (GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            GameManager.instance.gameState = GameManager.GameState.Paused;

            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            GameManager.instance.gameState = GameManager.GameState.Playing;

            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void DisplayNotEnoughGoldIcon()
    {
        notEnoughGoldDisplayTimer = notEnoughGoldIconDisplayTime;
    }
}
