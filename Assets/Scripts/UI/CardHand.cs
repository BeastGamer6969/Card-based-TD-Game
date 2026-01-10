using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
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
    [SerializeField] RectTransform Card;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] RectTransform Shadow;
     [SerializeField] GameObject ShadowcardPrefab;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] RectTransform deck;

    [Header("Msic")]
    [SerializeField] InputReader inputReader;


    void Start()
    {
        inputReader.SpaceBar += DrawCard;
    }

    void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;

        GameObject cardGO = Instantiate(cardPrefab, Card);
        GameObject shadowGO = Instantiate(ShadowcardPrefab, Shadow);

        RectTransform Cardrt = cardGO.GetComponent<RectTransform>();
        RectTransform Shadowrt = shadowGO.GetComponent<RectTransform>();

        Cardrt.anchoredPosition = Vector2.zero;
        Cardrt.localRotation = Quaternion.identity;
        Shadowrt.anchoredPosition = Vector2.zero;
        Shadowrt.localRotation = Quaternion.identity;

        handCards.Add(new CardData{CardTransform = Cardrt, IsSelected = false, ShadowCardTransform = Shadowrt});
        UpdateCardPositions();
    }

    void UpdateCardPositions()
    {
        if (handCards.Count == 0) return;

        Spline spline = splineContainer.Spline;

        for (int CardIndex = 0; CardIndex < handCards.Count; CardIndex++){
            float Position = CardSpacing(CardIndex);

            Vector3 localPos = spline.EvaluatePosition(Position);

            Vector3 tangent = spline.EvaluateTangent(Position);
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

            Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

            MoveCard(handCards[CardIndex].CardTransform, targetRot, localPos, Vector3.zero);
            MoveCard(handCards[CardIndex].ShadowCardTransform, targetRot, localPos, new Vector3(0, 5, 0));
        }
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

    public void MoveCard(RectTransform card, quaternion Rotation, Vector3 Position, Vector3 Offset)
    {
        if (
            Vector2.Distance(card.anchoredPosition, Position - Offset) < 0.1f &&
            Quaternion.Angle(card.localRotation, Rotation) < 0.5f
            ) return;

        card.DOKill();
        card.DOAnchorPos(Position - Offset, slideDuration);
        card.DOLocalRotateQuaternion(Rotation, slideDuration);
    }
}

[System.Serializable] internal class CardData
{
    public RectTransform CardTransform;
    public RectTransform ShadowCardTransform;
    public bool IsSelected;
}
