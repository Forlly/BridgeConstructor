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
    [SerializeField] private Button pauseButton;

    public static Menu Instance;

    private void Awake()
    {
        Instance = this;
        restartButton.onClick.AddListener(RestartGame);
        reliveButton.onClick.AddListener(ReliveCharacter);
    }


    public void GameOver()
    {
        pauseButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        endingScoreText.text = $"Your score: {scoreText.text}";
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }
    
    public void ReliveCharacter()
    {
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);

        CharacterController.Instance.Relive();
    }
}
