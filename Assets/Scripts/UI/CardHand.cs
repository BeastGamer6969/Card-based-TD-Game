using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class CardHand : MonoBehaviour
{
    [Header("CardInfo")]
    [SerializeField] float slideDuration = 0.5f;
    [SerializeField] int maxHandSize = 5;
    [SerializeField] AnimationCurve spreadCurve;
    [SerializeField] List<CardData> handCards = new List<CardData>();

    [Header("References")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] RectTransform deck;

    [Header("Msic")]
    [SerializeField] InputReader inputReader;


    void Start()
    {
        inputReader.SpaceBar += DrawCard;
    }

    void Update()
    {
        UpdateCardPositions();
    }
    void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;
        GameObject cardGO = Instantiate(cardPrefab, deck);
        RectTransform rt = cardGO.GetComponent<RectTransform>();

        rt.anchoredPosition = Vector2.zero;
        rt.localRotation = Quaternion.identity;

        handCards.Add(new CardData{CardTransform = rt, IsSelected = false});
        UpdateCardPositions();
    }

    void UpdateCardPositions()
    {
        if (handCards.Count == 0) return;

        Spline spline = splineContainer.Spline;

        float spacing = 1f / maxHandSize;

        for (int i = 0; i < handCards.Count; i++){
            handCards[i].CardTransform.GetChild(0).gameObject.SetActive(true);

            float p = CardSpacing(i);

            // UI-local position directly from spline
            Vector3 localPos = spline.EvaluatePosition(p);

            // Rotation along spline
            Vector3 tangent = spline.EvaluateTangent(p);
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

            RectTransform card = handCards[i].CardTransform;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

            if (
                Vector2.Distance(card.anchoredPosition, localPos) < 0.1f &&
                Quaternion.Angle(card.localRotation, targetRot) < 0.5f
            )
                continue;

            card.DOKill();
            card.DOAnchorPos(localPos, slideDuration);
            card.DOLocalRotateQuaternion(targetRot, slideDuration);
        }

        handCards[0].CardTransform.GetChild(0).gameObject.SetActive(false);
    }

    public float CardSpacing(int CardIndex)
    {
        int cardCount = handCards.Count;
        float normalizedCardPosition = (cardCount == 1) ? 0.5f : (float)CardIndex / (cardCount - 1);
        float normalizedHandFill = Mathf.InverseLerp(1, maxHandSize, cardCount);
        float handSpread = spreadCurve.Evaluate(normalizedHandFill);
        float adjustedNormalizedPosition = 0.5f + (normalizedCardPosition - 0.5f) * handSpread;
        return adjustedNormalizedPosition;
    }
}
[System.Serializable]
internal class CardData
{
    public RectTransform CardTransform;
    public bool IsSelected;
}
