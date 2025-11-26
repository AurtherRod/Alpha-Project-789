using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] Transform imagesParent;
    [SerializeField] GameObject cardPrefab;

    void Start()
    {
        int level = GameManager.Instance.LevelToLoad;
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
    }

    void SpawnImages(int numberOfCards, Transform parent)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject card = Instantiate(cardPrefab, parent);
        }
    }

    void SetGridLayout(int column)
    {
        gridLayoutGroup.constraintCount = column;
    }
}
