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

    [SerializeField] private PlayerData data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (LoadPlayerData() != null)
            {
                data = LoadPlayerData();
            }
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
        if (levelIndex < 0 || levelIndex >= data.Levels.Length || moves <= 0) return;

        var level = data.Levels[levelIndex];
        level.IsCompleted = true;

        if (level.CompletionTime == 0 || completionTime < level.CompletionTime)
            level.CompletionTime = completionTime;

        int newScore = (score * 10) / moves;
        if (newScore > level.Score)
            level.Score = newScore;

        if (levelIndex == data.CurrentLevel && levelIndex < data.Levels.Length - 1)
            data.CurrentLevel++;

        SavePlayerData();
    }

    public int GetCurrentLevelHighScore(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < data.Levels.Length)
        {
            return data.Levels[levelIndex].Score;
        }
        return 0;
    }


}
