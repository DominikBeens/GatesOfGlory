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

    private Camera mainCam;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Animator screenFadeAnim;
    public Animator gameOverAnimator;

    [Header("Pause")]
    public GameObject pausePanel;
    private bool canPause = true;

    [Header("Wave")]
    public TextMeshProUGUI waveText;
    public Image waveHealthFill;
    public TextMeshProUGUI waveHealthText;

    [Header("Other")]
    public GameObject gameInfoPanel;
    public GameObject waveTimerPanel;
    public PlayableDirector startGameTLDirector;
    public GameObject castleWeapons;
    public GameObject castleRooms;
    public GameObject startCinematicProps;
    public GameObject gameOverCinematicProps;
    public GameObject placeObjectUI;

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

        mainCam = Camera.main;

        if (GameManager.instance.showStartGameAnimation)
        {
            StartCoroutine(StartGame());
        }
        else
        {
            gameOverAnimator.enabled = false;
            canPause = true;

            gameInfoPanel.SetActive(true);
            waveTimerPanel.SetActive(true);
            startGameTLDirector.enabled = false;

            GameManager.instance.gameState = GameManager.GameState.Playing;

            WaveManager.instance.NextWave();
        }
    }

    private void Update()
    {
        if (notEnoughGoldDisplayTimer > 0)
        {
            notEnoughGoldDisplayTimer -= Time.deltaTime;
            notEnoughGoldIcon.SetActive(true);
            notEnoughGoldIcon.transform.position = Input.mousePosition;

            if (CursorManager.instance.CursorVisibilityStandard)
            {
                Cursor.visible = false;
            }
        }
        else
        {
            notEnoughGoldDisplayTimer = 0;
            notEnoughGoldIcon.SetActive(false);
            Cursor.visible = CursorManager.instance.CursorVisibilityStandard;
        }
    }

    private IEnumerator StartGame()
    {
        GameManager.instance.gameState = GameManager.GameState.Cinematic;

        Cursor.visible = false;
        canPause = false;

        CameraManager mainCamManager = mainCam.GetComponent<CameraManager>();
        mainCamManager.enabled = false;

        castleWeapons.SetActive(false);
        castleRooms.SetActive(false);
        startCinematicProps.SetActive(true);

        gameInfoPanel.SetActive(false);
        waveTimerPanel.SetActive(false);

        gameOverAnimator.enabled = true;

        yield return new WaitForSeconds(11);

        gameOverAnimator.enabled = false;
        mainCamManager.enabled = true;
        canPause = true;

        gameInfoPanel.SetActive(true);
        waveTimerPanel.SetActive(true);
        startGameTLDirector.enabled = false;

        castleWeapons.SetActive(true);
        castleRooms.SetActive(true);
        startCinematicProps.SetActive(false);

        Cursor.visible = true;

        GameManager.instance.gameState = GameManager.GameState.Playing;

        yield return new WaitForSeconds(1);

        WaveManager.instance.NextWave();
        GameManager.instance.showStartGameAnimation = false;
    }

    public IEnumerator GameOver()
    {
        CastleUpgradeManager.instance.CloseAllUI(null);
        canPause = false;
        GameManager.instance.gameState = GameManager.GameState.Cinematic;

        CameraManager mainCamManager = mainCam.GetComponent<CameraManager>();
        mainCamManager.enabled = false;

        screenFadeAnim.SetTrigger("Fade");

        yield return new WaitForSeconds(1.2f);

        gameInfoPanel.SetActive(false);
        waveTimerPanel.SetActive(false);
        castleWeapons.SetActive(false);
        castleRooms.SetActive(false);
        gameOverCinematicProps.SetActive(true);
        mainCam.fieldOfView = 60;
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
        SceneManager.LoadScene(0);
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
