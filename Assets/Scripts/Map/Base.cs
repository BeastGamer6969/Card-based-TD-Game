using UnityEngine;

public class Base : MonoBehaviour
{
    public bool IsEnemyBase = false;
    void Start()
    {
        if (IsEnemyBase)
        {
            transform.position = AnchorList.Anchors[0].position;
        }
        else
        {
            transform.position = AnchorList.Anchors[AnchorList.Anchors.Length - 1].position;
        }
    }
}
