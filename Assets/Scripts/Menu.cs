using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text endingScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button reliveButton;

    public static Menu Instance;

    private void Awake()
    {
        Instance = this;
        restartButton.onClick.AddListener(RestartGame);
    }


    public void GameOver()
    {
        scoreText.gameObject.SetActive(false);
        Time.timeScale = 0;
        endingScoreText.text = $"Your score: {scoreText.text}";
    }
    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        scoreText.gameObject.SetActive(true);
    }
}
