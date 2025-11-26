using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private Slider LevelSlider;
    [SerializeField] private TMPro.TextMeshProUGUI LevelText;

    void Start()
    {
        OnLevelSliderChanged();
    }

    public void OnLevelSliderChanged()
    {
        int level = (int)Mathf.Clamp(LevelSlider.value, 0, 4);
        GameManager.Instance.SetLevelToLoad(level);
        switch (level)
        {
            case 0:
                LevelText.text = "2x2";
                break;
            case 1:
                LevelText.text = "2x3";
                break;
            case 2:
                LevelText.text = "4x4";
                break;
            case 3:
                LevelText.text = "5x6";
                break;
            default:
                LevelText.text = "2x2";
                break;
        }
    }

    public void OnPlayButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }


}
