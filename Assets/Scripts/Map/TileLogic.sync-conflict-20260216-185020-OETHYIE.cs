using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Selection")]
    public Color HighlightedColor;
    private Color BaseColor;
    private Renderer Rend;
    private BuildManger buildManger;


    void Start()
    {
        Rend = GetComponent<Renderer>();
        BaseColor = Rend.material.color;
        buildManger = Camera.main.transform.GetChild(0).GetComponent<BuildManger>();
    }

  public void OnPointerEnter(PointerEventData eventData)
    {
        Rend.material.color = HighlightedColor;
        if(transform.childCount > 0) buildManger.tiledata.SetData(transform.gameObject, transform.GetChild(0).gameObject);
        else buildManger.tiledata.SetData(transform.gameObject,null);
    }

     public void OnPointerExit(PointerEventData eventData)
    {
        Rend.material.color = BaseColor;
        buildManger.tiledata.SetData(null, null);
    }
}
