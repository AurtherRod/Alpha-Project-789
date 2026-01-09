using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int LevelToLoad = 0;


    private void Awake()
    {
        jainish();
        Init();
    }

    void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        durgesh();
    }

    public void SetLevelToLoad(int level)
    {
        LevelToLoad = level;
    }

    public void jainish()
    {
        Debug.Log("Hello Jainish");
    }

    public void durgesh()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Durgesh");
        }
    }
}
