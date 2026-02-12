using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class CardManger : MonoBehaviour
{
    [Header("CardInfo")]
    [SerializeField] float slideDuration = 0.5f;
    [SerializeField] int maxHandSize = 5;
    [SerializeField] AnimationCurve spreadCurve;
    public List<CardData> handCards = new List<CardData>();
    public GameObject Turret;
    
    [Header("Cards")]
    [SerializeField] CardInfoObject[] cardInfoObjects;

    [Header("References")]
    [SerializeField] RectTransform Card;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] RectTransform Shadow;
    [SerializeField] GameObject ShadowcardPrefab;
    [SerializeField] Vector2 ShadowCardOffset = Vector2.zero;
    [SerializeField] GameObject DarknessAroundTheEdge;

    [Header("Spline")]
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] bool UpdateSplineasf = false;

    [Header("Deck")]
    [SerializeField] RectTransform deck;

    [Header("InfoDescription")]
    [SerializeField] float FadeTime;

    [Header("Msic")]
    [SerializeField] InputReader inputReader;
    private BuildManger buildManger;


    void Start()
    {
        DarknessAroundTheEdge.SetActive(false);

        buildManger = Camera.main.GetComponentInChildren<BuildManger>();

        inputReader.SpaceBar += DrawCard;
        inputReader.LeftClick += LeftMouseClick;
        inputReader.Tab += TabScrean;
    }

    void Update()
    {
        if (UpdateSplineasf) UpdateCardPositions();
    }

    void LeftMouseClick(bool ClickDown)
    {
        if (ClickDown)
        {
            if(Turret != null) buildManger.CardTurret = Turret;
            Turret = null;
        }
    }

    void TabScrean(bool ClickDown)
    {
        if(ClickDown) Fadedarkness(true);
        else 
        {
            Fadedarkness(false); 
            DarknessAroundTheEdge.SetActive(false);
        }
    }
    void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;
        CardUiInfo newCardUiInfo = GenrateCard();

        GameObject cardGO = Instantiate(cardPrefab, Card);
        cardGO.GetComponent<CardInfo>().cardUiInfo = newCardUiInfo;
        
        GameObject shadowGO = Instantiate(ShadowcardPrefab, new Vector2(Shadow.position.x, Shadow.position.y) - ShadowCardOffset, Quaternion.identity, Shadow);

        RectTransform Cardrt = cardGO.GetComponent<RectTransform>();
        RectTransform Shadowrt = shadowGO.GetComponent<RectTransform>();

        Cardrt.anchoredPosition = Vector2.zero;
        Cardrt.localRotation = Quaternion.identity;
        Shadowrt.anchoredPosition = Vector2.zero;
        Shadowrt.localRotation = Quaternion.identity;

        handCards.Add(new CardData{CardTransform = Cardrt, ShadowCardTransform = Shadowrt});
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
            handCards[CardIndex].CardTransform.GetComponent<CardInfo>().restPositon(new Vector2(localPos.x, localPos.y), targetRot);
        }
    }

    public void MoveCard(int CardIndex, Quaternion Rotation, Vector2 Position)
    {
        RectTransform[] CardTranform = {handCards[CardIndex].CardTransform, handCards[CardIndex].ShadowCardTransform};
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

    private CardUiInfo GenrateCard()
    {
        CardInfoObject cardInfoObject = cardInfoObjects[Random.Range(0, cardInfoObjects.Length - 1)];
        int RarityTotalRange = 0;
        for(int EachRarity = 0; EachRarity < cardInfoObject.Rarity.Length; EachRarity++)
        {
            RarityTotalRange += cardInfoObject.Rarity[EachRarity].Rarity_weight;
        }

        int ChosenRarity_weight = Random.Range(0, RarityTotalRange);
        int ChosenRarity = 0;

        for(int EachRarity = 0; EachRarity < cardInfoObject.Rarity.Length; EachRarity++)
        {
            if(ChosenRarity_weight <= cardInfoObject.Rarity[EachRarity].Rarity_weight)
            {
                ChosenRarity = EachRarity;
                continue;
            }
        }

        CardUiInfo cardUiInfo = new CardUiInfo
        {
            Title = cardInfoObject.Title,
            Turret = cardInfoObject.Turret,
            Tags = cardInfoObject.Tags,
            description = cardInfoObject.description,
            Rarity = cardInfoObject.Rarity[ChosenRarity]
        };
        return cardUiInfo;
    }

    void Fadedarkness(bool fadein)
    {
        DarknessAroundTheEdge.SetActive(true);
        Image sprite = DarknessAroundTheEdge.GetComponent<Image>();
        Color c = sprite.color;
        float time = 0;
        if (fadein)
        {
            c.a = 0f;
            sprite.color = c;
            while(time < FadeTime)
            {
                c.a += time/FadeTime;
                sprite.color = c;
                time += Time.deltaTime;
            }
        }
        else
        {
            c.a = 1f;
            sprite.color = c;
            while(time < FadeTime)
            {
                c.a -= time/FadeTime;
                sprite.color = c;
                time += Time.deltaTime;
            }
        }
    }
}

[System.Serializable] public class CardData
{
    public RectTransform CardTransform;
    public RectTransform ShadowCardTransform;
}
