using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Button cardButton;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private float flipDuration = 0.3f;
    [SerializeField] private bool hideOnMatch = true;

    private Sprite frontSprite;
    private bool isFlipped = false;
    private bool isMatched = false;
    private bool isFlipping = false;
    private Coroutine flipCoroutine;

    // Identifier for matching logic / pooling reuse
    public int Id { get; private set; } = -1;

    // Events to decouple card logic from managers
    public event Action<Card> Flipped;
    public event Action<Card> Matched;

    public bool IsFlipped => isFlipped;
    public bool IsMatched => isMatched;
    public Sprite FrontSprite => frontSprite;

    void Awake()
    {
        // Cache components defensively
        if (cardImage == null)
            cardImage = GetComponent<Image>();

        if (cardButton == null)
            cardButton = GetComponent<Button>();

        if (cardButton != null)
            cardButton.onClick.AddListener(OnCardClicked);
        else
            Debug.LogWarning($"{nameof(Card)} on '{gameObject.name}' has no Button component assigned.");
    }

    public void SetCardData(Sprite front, Sprite back, int id = -1)
    {
        frontSprite = front;
        backSprite = back;
        Id = id;
        if (cardImage != null)
            cardImage.sprite = backSprite;
    }

    void OnCardClicked()
    {
        if (isMatched || isFlipping || isFlipped)
            return;

        AudioManager.Instance?.PlayButtonClickSFX();
        // ensure any previous flip coroutine is stopped
        if (flipCoroutine != null)
            StopCoroutine(flipCoroutine);

        flipCoroutine = StartCoroutine(FlipAnimation());
    }

    IEnumerator FlipAnimation()
    {
        isFlipping = true;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        // first half: shrink X
        while (elapsed < flipDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (flipDuration / 2f));
            float scaleX = Mathf.Lerp(1f, 0f, t);
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            yield return null;
        }

        // swap visible face
        isFlipped = !isFlipped;
        if (cardImage != null)
            cardImage.sprite = isFlipped ? frontSprite : backSprite;

        // Notify observers at the moment the front becomes visible (useful for matching checks)
        Flipped?.Invoke(this);
        LevelManager.Instance?.OnCardFlipped(this);

        // second half: expand X
        elapsed = 0f;
        while (elapsed < flipDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (flipDuration / 2f));
            float scaleX = Mathf.Lerp(0f, 1f, t);
            transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            yield return null;
        }

        transform.localScale = originalScale;
        isFlipping = false;
        flipCoroutine = null;
    }

    public void FlipToBack()
    {
        if (!isFlipping && isFlipped && !isMatched)
        {
            if (flipCoroutine != null)
                StopCoroutine(flipCoroutine);
            flipCoroutine = StartCoroutine(FlipAnimation());
        }
    }

    public void SetMatched()
    {
        isMatched = true;
        isFlipped = true;

        // disable visuals / interaction according to inspector flags
        if (hideOnMatch)
        {
            if (cardImage != null)
                cardImage.enabled = false;
        }

        if (cardButton != null)
            cardButton.interactable = false;

        Matched?.Invoke(this);
    }

    // Reset card to a neutral state (useful for pooling / level reset)
    public void ResetCard()
    {
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
            flipCoroutine = null;
        }

        isFlipped = false;
        isMatched = false;
        isFlipping = false;
        Id = -1;

        if (cardImage != null)
        {
            cardImage.enabled = true;
            cardImage.sprite = backSprite;
        }

        if (cardButton != null)
            cardButton.interactable = true;

        transform.localScale = Vector3.one;
    }

    // Force a flip without playing SFX or invoking LevelManager (useful for setup)
    public void FlipImmediate(bool showFront)
    {
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
            flipCoroutine = null;
        }

        isFlipped = showFront;
        if (cardImage != null)
            cardImage.sprite = isFlipped ? frontSprite : backSprite;

        transform.localScale = Vector3.one;
    }
}
