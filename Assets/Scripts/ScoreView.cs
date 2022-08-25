using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    public static ScoreView Instance;
    [SerializeField] private Text scoreText;
    void Start()
    {
        Instance = this;
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = _score.ToString();
    }
}
