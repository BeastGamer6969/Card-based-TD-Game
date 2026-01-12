using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class CardHandler : MonoBehaviour
{
    [Header("CardInfo")]
    [SerializeField] float slideDuration = 0.5f;
    [SerializeField] int maxHandSize = 5;
    [SerializeField] AnimationCurve spreadCurve;
    public List<CardData> handCards = new List<CardData>();

    [Header("References")]
    [SerializeField] RectTransform Card;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] RectTransform Shadow;
    [SerializeField] GameObject ShadowcardPrefab;
    [SerializeField] Vector2 ShadowCardOffset = Vector2.zero;
    [SerializeField] SplineContainer splineContainer;
    Spline spline;
    [SerializeField] RectTransform deck;

    [Header("Msic")]
    [SerializeField] InputReader inputReader;


    void Start()
    {
        inputReader.SpaceBar += DrawCard;
        spline = splineContainer.Spline;
    }

    void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;

        GameObject cardGO = Instantiate(cardPrefab, Card);
        GameObject shadowGO = Instantiate(ShadowcardPrefab, new Vector2(Shadow.position.x, Shadow.position.y) - ShadowCardOffset, Quaternion.identity, Shadow);

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

            MoveCard(CardIndex, targetRot, new Vector2(localPos.x, localPos.y));
        }
    }

    public void MoveCard(int CardIndex, Quaternion Rotation, Vector2 Position)
    {
        RectTransform[] CardTranform = new RectTransform[] 
            {handCards[CardIndex].CardTransform, 
            handCards[CardIndex].ShadowCardTransform};
        if (
            Vector2.Distance(CardTranform[0].anchoredPosition, Position) < 0.1f &&
            Quaternion.Angle(CardTranform[0].localRotation, Rotation) < 0.5f
            ) return;

        CardTranform[0].DOKill();
        CardTranform[1].DOKill();

        CardTranform[0].DOAnchorPos(Position, slideDuration);
        CardTranform[0].DOLocalRotateQuaternion(Rotation, slideDuration);

        CardTranform[1].DOAnchorPos(Position - ShadowCardOffset, slideDuration);
        CardTranform[1].DOLocalRotateQuaternion(Rotation, slideDuration);
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

[System.Serializable] public class CardData
{
    public RectTransform CardTransform;
    public RectTransform ShadowCardTransform;
    public bool IsSelected;
}
