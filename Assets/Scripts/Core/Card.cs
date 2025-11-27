using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private float flipDuration = 0.3f;

    private Sprite frontSprite;
    private bool isFlipped = false;
    private bool isMatched = false;
    private bool isFlipping = false;

    public bool IsFlipped => isFlipped;
    public bool IsMatched => isMatched;
    public Sprite FrontSprite => frontSprite;

    void Start()
    {
        if (cardImage == null)
            cardImage = GetComponent<Image>();

        GetComponent<Button>().onClick.AddListener(OnCardClicked);
    }

    public void SetCardData(Sprite front, Sprite back)
    {
        frontSprite = front;
        backSprite = back;
        cardImage.sprite = backSprite;
    }

    void OnCardClicked()
    {
        if (isMatched || isFlipping || isFlipped)
            return;
        AudioManager.Instance?.PlayButtonClickSFX();
        StartCoroutine(FlipAnimation());
    }

    IEnumerator FlipAnimation()
    {
        isFlipping = true;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsed < flipDuration / 2)
        {
            elapsed += Time.deltaTime;
            float scaleX = Mathf.Lerp(1f, 0f, elapsed / (flipDuration / 2));
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            yield return null;
        }

        isFlipped = !isFlipped;
        cardImage.sprite = isFlipped ? frontSprite : backSprite;

        elapsed = 0f;
        while (elapsed < flipDuration / 2)
        {
            elapsed += Time.deltaTime;
            float scaleX = Mathf.Lerp(0f, 1f, elapsed / (flipDuration / 2));
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            yield return null;
        }

        transform.localScale = originalScale;
        isFlipping = false;

        LevelManager.Instance.OnCardFlipped(this);
    }

    public void FlipToBack()
    {
        if (!isFlipping && isFlipped && !isMatched)
            StartCoroutine(FlipAnimation());
    }

    public void SetMatched()
    {
        isMatched = true;
        isFlipped = true;
        gameObject.GetComponent<Image>().enabled = false;
    }
}
