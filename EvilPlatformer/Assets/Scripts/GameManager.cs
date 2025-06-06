using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    [Header("Timer Settings")]
    [Tooltip("Starting time in seconds")]
    public float startTime = 60f;

    [Tooltip("Reference to the TextMeshProUGUI component displaying the timer")]
    public TextMeshProUGUI timerText;

    private float _timeRemaining;
    private bool _isGameOver = false;
    private Color _originalColor;

    public Destroyable playerDestroy;

    bool hasWon;

    public TextMeshProUGUI bestTime;

    public GameObject player;

    private void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        // Initialize timer
        _timeRemaining = startTime;

        if (timerText == null)
        {
            Debug.LogError("[GameManager] No TextMeshProUGUI assigned for timerText.");
            enabled = false;
            return;
        }

        // Cache the original color so we can revert if needed
        _originalColor = timerText.color;
        UpdateTimerUI();

        bestTime.SetText(PlayerPrefs.GetFloat("BestTime", 0f).ToString("F2"));

        
    }

    void Update()
    {
        if (_isGameOver || hasWon)
            return;

        // Count down
        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining < 0f)
            _timeRemaining = 0f;

        UpdateTimerUI();

        // When timer reaches zero, trigger GameOver
        if (_timeRemaining <= 0f)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.GetComponent<Destroyable>().Destroy();
        }
    }

    private void UpdateTimerUI()
    {
        // Show whole seconds (round up so "10.x" still shows as "11" until it hits 10.0f exactly)
        //int displaySeconds = Mathf.CeilToInt(_timeRemaining);
        //timerText.text = displaySeconds.ToString("F2");
        timerText.text = _timeRemaining.ToString("F2");
        // Turn red if 10 seconds or less remain
        if (_timeRemaining <= 10f)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = _originalColor;
        }

    }

    /// <summary>
    /// Call this to begin end‐of‐game sequence.
    /// </summary>
    public void GameOver()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        StartCoroutine(HandleGameOver());

        playerDestroy.Destroy();
    }

    private IEnumerator HandleGameOver()
    {
        // You could play a "time up" sound or show a UI popup here
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinGame()
    {
        SoundManager.Instance.PlaySound("collect");
        hasWon = true;
        timerText.color = Color.white;
        timerText.SetText("Winner");
        StartCoroutine(HandleGameOver());

        float latestBestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        if (_timeRemaining > latestBestTime)
        {
            timerText.SetText("New Best Time! " + _timeRemaining.ToString("F2"));
            PlayerPrefs.SetFloat("BestTime", _timeRemaining);
        }

        SoundManager.Instance.PlayWinJingle();
    }
}
