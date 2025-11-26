using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] Transform imagesParent;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] List<Sprite> imagesList = new List<Sprite>();
    [SerializeField] List<Sprite> spawnedCardsList = new List<Sprite>();

    void Start()
    {
        int level = GameManager.Instance?.LevelToLoad ?? 0;
        LevelSetter(level);
    }

    void LevelSetter(int level)
    {
        int numberOfCards = 0;
        int column = 2;
        switch (level)
        {
            case 0:
                numberOfCards = 4;
                column = 2;
                break;
            case 1:
                numberOfCards = 6;
                column = 3;
                break;
            case 2:
                numberOfCards = 16;
                column = 4;
                break;
            case 3:
                numberOfCards = 30;
                column = 6;
                break;
            default:
                numberOfCards = 4;
                column = 2;
                break;
        }
        SetGridLayout(column);
        SpawnImages(numberOfCards, imagesParent);
        AssignImagesToCards();
    }

    void SpawnImages(int numberOfCards, Transform parent)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject card = Instantiate(cardPrefab, parent);
            spawnedCardsList.Add(card.GetComponent<Image>().sprite);
        }

    }

    void SetGridLayout(int column)
    {
        gridLayoutGroup.constraintCount = column;
    }

    void AssignImagesToCards()
    {
        List<Sprite> cardSprites = new List<Sprite>();
        int pairsNeeded = imagesParent.childCount / 2;

        for (int i = 0; i < pairsNeeded; i++)
        {
            Sprite selectedSprite = imagesList[i % imagesList.Count];
            cardSprites.Add(selectedSprite);
            cardSprites.Add(selectedSprite);
        }

        for (int i = 0; i < cardSprites.Count; i++)
        {
            Sprite temp = cardSprites[i];
            int randomIndex = Random.Range(i, cardSprites.Count);
            cardSprites[i] = cardSprites[randomIndex];
            cardSprites[randomIndex] = temp;
        }

        for (int i = 0; i < imagesParent.childCount; i++)
        {
            Image image = imagesParent.GetChild(i).GetComponent<Image>();
            if (image != null && i < cardSprites.Count)
            {
                image.sprite = cardSprites[i];
            }
        }
    }

}
