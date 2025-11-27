using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] GameObject nextLevelButton;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void OnNextLevelButtonPressed()
    {
        if (gameManager.LevelToLoad >= SaveSystem.Instance.GetPlayerData().CurrentLevel)
        {
            nextLevelButton.SetActive(false);
        }
        gameManager.SetLevelToLoad(LevelManager.Instance.CurrentLevel + 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }

    public void OnMenuButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
