using UnityEngine;

public class AnchorList : MonoBehaviour
{
    public static Transform[] Anchors;
  void Awake()
  {
    Anchors = new Transform[transform.childCount];
    for (int CurrentChild = 0; CurrentChild < transform.childCount; CurrentChild++)
    {
      Anchors[CurrentChild] = transform.GetChild(CurrentChild).transform;
    }
  }
}
