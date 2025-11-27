using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private Button[] LevelButtons;

    void Start()
    {
        LevelButtonManagement();
    }

    private void LevelButtonManagement()
    {
        PlayerData playerData = SaveSystem.Instance.GetPlayerData();
        int CurrentLevel = playerData.CurrentLevel;

        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if (i <= CurrentLevel)
            {
                LevelButtons[i].interactable = true;
            }
            else
            {
                LevelButtons[i].interactable = false;
            }
        }
    }

    public void OnLevelButtonPressed(int level)
    {
        GameManager.Instance.SetLevelToLoad(level);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
}
