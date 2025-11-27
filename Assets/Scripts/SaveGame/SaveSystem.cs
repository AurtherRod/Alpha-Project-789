using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int CurrentLevel;
    public LevelData[] Levels;
}

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public float CompletionTime;
    public int Score;
    public bool IsCompleted;
}

public class SaveSystem : MonoBehaviour
{
    private const string SAVE_KEY = "PlayerSaveData";
    public static SaveSystem Instance { get; private set; }

    private PlayerData data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            data = LoadPlayerData() ?? new PlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    private PlayerData LoadPlayerData()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY)) return null;
        return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(SAVE_KEY));
    }

    public static void ClearSaveData()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
    }

    public PlayerData GetPlayerData() => data;

    public void CompleteLevel(int levelIndex, float completionTime, int score, int moves)
    {
        if (levelIndex < 0 || levelIndex >= data.Levels.Length) return;
        
        data.CurrentLevel = Mathf.Min(levelIndex + 1, data.Levels.Length - 1);
        var level = data.Levels[levelIndex];
        level.IsCompleted = true;
        
        if (level.CompletionTime == 0 || completionTime < level.CompletionTime)
            level.CompletionTime = completionTime;
        
        int newScore = score / moves * 10;
        if (newScore > level.Score)
            level.Score = newScore;
        
        SavePlayerData();
    }

    public int GetCurrentLevelHighScore(int levelIndex) => 
        levelIndex >= 0 && levelIndex < data.Levels.Length ? data.Levels[levelIndex].Score : 0;


}
