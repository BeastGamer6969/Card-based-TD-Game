using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class MakePath : MonoBehaviour
{
  [SerializeField] GameObject PathPrefab;

  void Start()
  {
    for(int Anchor = 0; Anchor < AnchorList.Anchors.Length-1; Anchor++)
    {
      Transform Pos1 =  AnchorList.Anchors[Anchor];
      Transform Pos2 =  AnchorList.Anchors[Anchor+1];

      float Distance = Vector3.Distance(Pos1.position,Pos2.position);
      Vector3 dir = (Pos2.position - Pos1.position).normalized;

      Vector3 SpwanPos = Pos1.position + (dir * (Distance/2));

      Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

      GameObject ClonePath = Instantiate(PathPrefab, SpwanPos, rot, transform);

      ClonePath.transform.localScale = new Vector3(
                ClonePath.transform.localScale.x,
                ClonePath.transform.localScale.y,
                Distance
            );
      

    }
  }
}
