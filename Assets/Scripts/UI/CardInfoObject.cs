using System;
using UnityEngine;

[CreateAssetMenu]
public class CardInfoObject : ScriptableObject
{
  public string Title;
  public TurretInfo Turret;
  public TagsInfo[] Tags;
  [TextArea(3, 6)]
  public string description;
  public RarityInfo[] Rarity;
}


[System.Serializable]
public class TurretInfo
{
  public Sprite Turret_Image;
  public GameObject Turret;
}

[System.Serializable]
public class TagsInfo
{
  public Sprite Tags_Image;
  public string Tags;
}

[System.Serializable]
public class RarityInfo
{
  public Sprite Rarity_Image;
  public int Rarity_Level;
  public int Rarity_weight;
}