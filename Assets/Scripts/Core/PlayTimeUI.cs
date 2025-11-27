using UnityEngine;

public class PlayTimeUI : MonoBehaviour
{
    private LevelManager levelManager;

    [SerializeField] private TMPro.TextMeshProUGUI Move;
    [SerializeField] private TMPro.TextMeshProUGUI HighScore;
    [SerializeField] private TMPro.TextMeshProUGUI PassedTime;

    void Start()
    {
        levelManager = LevelManager.Instance;
        HighScore.text = levelManager.GetHighScore().ToString();
    }
    void Update()
    {
        if (levelManager != null)
        {
            (string passedTime, string moves) = levelManager.GetPassedTimeAndMove();
            PassedTime.text = passedTime;
            Move.text = moves;
        }
    }
}
