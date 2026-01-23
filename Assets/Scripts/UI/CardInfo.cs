using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.ShaderKeywordFilter;

public class CardInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardManger cardmanger;
    [SerializeField] float scalingFactor;
    [SerializeField] float sacletime;
    [SerializeField] Vector2 Positionfactor;
    [SerializeField] GameObject Turret;

    private Vector2 OrignalPosition;
    private RectTransform rt;

    [SerializeField] bool Effect;
    private int Cardindex = -1;

    
    void Start()
    {
        cardmanger = GetComponentInParent<CardManger>();
        rt = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Cardindex == -1 || cardmanger.handCards[Cardindex].CardTransform != GetComponent<RectTransform>()) findCardIndex();
        cardmanger.Turret = Turret;
        cardmanger.MoveCard(Cardindex, Quaternion.identity, OrignalPosition + Positionfactor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Cardindex == -1 || cardmanger.handCards[Cardindex].CardTransform != GetComponent<RectTransform>()) findCardIndex();
        cardmanger.MoveCard(Cardindex, Quaternion.identity, OrignalPosition);

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

    public void restPositon(Vector2 restpostion)
    {
        OrignalPosition = restpostion;
    }
}
