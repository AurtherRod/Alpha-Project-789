using UnityEngine;
using UnityEngine.SceneManagement;

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
        PlayerData playerData = SaveSystem.Instance.GetPlayerData();
        int nextLevel = (gameManager.LevelToLoad + 1) % playerData.Levels.Length;

        if (nextLevel > playerData.CurrentLevel && nextLevelButton != null)
            nextLevelButton.SetActive(false);

        gameManager.SetLevelToLoad(nextLevel);
        SceneManager.LoadScene("Play");
    }

    public void OnMenuButtonPressed() => SceneManager.LoadScene("Menu");

    public void OnMusicPanelButtonPressed() => AudioManager.Instance.AudioPanelOpen();
}
