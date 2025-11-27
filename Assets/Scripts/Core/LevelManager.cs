using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject LevelCompleteUI;
    [SerializeField] GameObject GamePlayUI;

    public int CurrentLevel = 0;
    public int Score = 0;
    public int Moves = 0;
    public bool IsLevelCompleted = false;
    private Queue<Card> flippedCards = new Queue<Card>();
    private int matchedPairs = 0;
    private int totalPairs;
    private bool isProcessing = false;
    public int GetHighScore() => SaveSystem.Instance.GetCurrentLevelHighScore(CurrentLevel);

    [SerializeField] float PassedTime = 0;
    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
        CurrentLevel = GameManager.Instance.LevelToLoad;
    }

    void Start()
    {
        IsLevelCompleted = false;
    }

    public void InitializeGame(int pairs)
    {
        totalPairs = pairs;
        matchedPairs = 0;
        Score = 0;
        Moves = 0;
        flippedCards.Clear();
        isProcessing = false;
    }

    void Update()
    {
        if (!IsLevelCompleted)
        {
            PassedTime += Time.deltaTime;
        }
    }

    public void OnCardFlipped(Card card)
    {
        if (card.IsFlipped)
        {
            flippedCards.Enqueue(card);

            if (flippedCards.Count >= 2 && !isProcessing)
            {
                StartCoroutine(ProcessPairs());
            }
        }
    }

    IEnumerator ProcessPairs()
    {
        isProcessing = true;

        while (flippedCards.Count >= 2)
        {
            Moves++;
            Card card1 = flippedCards.Dequeue();
            Card card2 = flippedCards.Dequeue();

            yield return new WaitForSeconds(1f);

            if (card1.FrontSprite == card2.FrontSprite)
            {
                card1.SetMatched();
                card2.SetMatched();
                matchedPairs++;
                Score += 10;

                if (matchedPairs >= totalPairs)
                {
                    AudioManager.Instance?.PlayWinGameSFX();
                    IsLevelCompleted = true;
                    GamePlayUI.SetActive(false);
                    LevelCompleteUI.SetActive(true);
                    SaveSystem.Instance.CompleteLevel(CurrentLevel, PassedTime, Score, Moves);
                }
            }
            else
            {
                card1.FlipToBack();
                card2.FlipToBack();
            }
        }

        isProcessing = false;
    }

    public (string, string) GetPassedTimeAndMove()
    {
        return (((int)PassedTime).ToString(), Moves.ToString());
    }
}
