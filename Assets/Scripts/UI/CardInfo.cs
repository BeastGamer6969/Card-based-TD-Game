using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.ShaderKeywordFilter;
using TMPro;
using Unity.Mathematics;

public class CardInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Card UI")]
    public CardUiInfo cardUiInfo;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private GameObject TurretPicture;
    [SerializeField] private GameObject Tags;
    [SerializeField] private TextMeshProUGUI Description;

    [Header("Card Postioning")]
    private CardManger cardmanger;
    [SerializeField] Vector2 PositionDirection;
    [SerializeField] float PositionScale;

    private Vector2 OrignalPosition;
    private Quaternion OrignalQuaternion;
    private RectTransform rt;

    private int Cardindex = -1;

    void Start()
    {
        cardmanger = GetComponentInParent<CardManger>();
        rt = GetComponent<RectTransform>();

        GetComponent<Image>().sprite = cardUiInfo.Rarity.Rarity_Image;
        Title.text = cardUiInfo.Title;
        TurretPicture.GetComponent<Image>().sprite = cardUiInfo.Turret.Turret_Image;
        Description.text = cardUiInfo.description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Cardindex == -1 || cardmanger.handCards[Cardindex].CardTransform != GetComponent<RectTransform>()) findCardIndex();
        cardmanger.Turret = cardUiInfo.Turret.Turret;
        Vector2 WorldPos = transform.TransformDirection(PositionDirection.normalized);
        cardmanger.MoveCard(Cardindex, OrignalQuaternion, OrignalPosition + WorldPos*PositionScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Cardindex == -1 || cardmanger.handCards[Cardindex].CardTransform != GetComponent<RectTransform>()) findCardIndex();
        cardmanger.MoveCard(Cardindex, OrignalQuaternion, OrignalPosition);

    }

    public void findCardIndex()
    {
        for (int i = 0; i < cardmanger.handCards.Count; i++)
            {
                if(cardmanger.handCards[i].CardTransform == GetComponent<RectTransform>())
                {
                    Cardindex = i;
                    return;
                }
            }
    }

    public void restPositon(Vector2 restpostion, Quaternion restquaternion)
    {
        OrignalPosition = restpostion;
        OrignalQuaternion = restquaternion;
    }
}

[System.Serializable]
public class CardUiInfo
{
  public string Title;
  public TurretInfo Turret;
  public TagsInfo[] Tags;
  [TextArea(3, 6)]
  public string description;
  public RarityInfo Rarity;
}