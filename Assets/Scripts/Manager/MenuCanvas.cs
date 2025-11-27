using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ButtonData
{
    public Button button;
    public TMPro.TextMeshProUGUI levelScoreText;
}

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private ButtonData[] levelButtons;
    private const string NotCompletedText = "";

    void Start() => InitializeLevelButtons();

    private void InitializeLevelButtons()
    {
        var playerData = SaveSystem.Instance.GetPlayerData();
        var currentLevel = playerData.CurrentLevel;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            ButtonData buttonData = levelButtons[i];
            bool isUnlocked = i <= currentLevel;

            buttonData.button.interactable = isUnlocked;
            buttonData.levelScoreText.text = isUnlocked && playerData.Levels[i].IsCompleted
                ? playerData.Levels[i].Score.ToString()
                : NotCompletedText;
        }
    }

    public void OnLevelButtonPressed(int level)
    {
        var playerData = SaveSystem.Instance.GetPlayerData();
        if (level < 0 || level >= playerData.Levels.Length) return;
        
        GameManager.Instance.SetLevelToLoad(level);
        SceneManager.LoadScene("Play");
    }
}
