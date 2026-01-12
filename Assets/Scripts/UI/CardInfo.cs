using UnityEngine;
using UnityEngine.EventSystems;

public class CardInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardHandler cardhandler;
    private int Cardindex = -1;
    void Start()
    {
        cardhandler = GetComponentInParent<CardHandler>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Cardindex == -1 || cardhandler.handCards[Cardindex].CardTransform != GetComponent<RectTransform>()) findCardIndex();

        cardhandler.handCards[Cardindex].IsSelected = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Cardindex == -1 || cardhandler.handCards[Cardindex].CardTransform != GetComponent<RectTransform>()) findCardIndex();

        cardhandler.handCards[Cardindex].IsSelected = false;
    }

    public void findCardIndex()
    {
        for (int i = 0; i < cardhandler.handCards.Count; i++)
            {
                if(cardhandler.handCards[i].CardTransform == GetComponent<RectTransform>())
                {
                    Cardindex = i;
                    return;
                }
            }
    }
}
