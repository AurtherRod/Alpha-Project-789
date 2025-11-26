using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int LevelToLoad = 0;
    public int Score = 0;
    public int Moves = 0;

    private Queue<Card> flippedCards = new Queue<Card>();
    private int matchedPairs = 0;
    private int totalPairs;
    private bool isProcessing = false;

    private void Awake()
    {
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

    public void SetLevelToLoad(int level)
    {
        LevelToLoad = level;
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
                    Debug.Log("Level Complete! Score: " + Score + " Moves: " + Moves);
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
}
