using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;


public class BuildManger : MonoBehaviour
{
    [Header("Turret")]
    public GameObject CardTurret;
    [SerializeField] private bool RemoveCardTurret = true;
    [SerializeField] bool CanMoveTurret = false;

    [Header("TileInfo")]
    public TileData tiledata = new TileData();

    [Header("SelectionInfo")]
    public SelectionData selectionData = new SelectionData();

    [Header("MergeRules")]
    [SerializeField] MergeRules[] mergeRules;
    [SerializeField] InputReader inputReader;

    static BuildManger BM;
    void Start()
    {
        if (BM == null) BM = this;
        else if (BM != this) enabled = false;

        inputReader.LeftClick += LeftMouseClick;
        inputReader.RightClick += RightMouseClick;
    }

    void Update()
    {
        if (!CanMoveTurret || selectionData.Turret == null) return;
        selectionData.Turret.transform.SetParent(tiledata.Tile.transform);
        selectionData.Turret.transform.position = Givepositon(selectionData.Turret, tiledata.Tile);
    }

    void LeftMouseClick(bool ClickDown)
    {
        if (tiledata.Tile == null) return;
        if (ClickDown)
        {
            if(tiledata.Turret == null)
            {
                return;
            }

            if(selectionData.Turret == null)
            {
                selectionData.SetData(tiledata.Tile, tiledata.Turret);
                CanMoveTurret = true;
                tiledata.Turret = null;
            }
        }
        if (!ClickDown)
        {   
            if (CardTurret != null)
            {
                SpwanTurret();
                return;   
            }

            if(selectionData.Turret == null) return;

            if(tiledata.Turret == null) MoveTurret(tiledata.Tile);
            else if(CanUpgrade() == true) SpwanTurret(true);
            else MoveTurret(selectionData.Tile);
        }
    }

    void RightMouseClick(bool ClickDown)
    {
        if(ClickDown)
        {   
            if(tiledata.Turret != null)
            {
                DestroyTurret();
            }
        }
    }

    private void SpwanTurret(bool Upgrade = false)
    {
        if (CardTurret == null) return;
        if (Upgrade)
        {
            Destroy(selectionData.Turret);
            Destroy(tiledata.Turret);
        }
        GameObject turret = Instantiate(CardTurret, tiledata.Tile.transform, true);
        turret.transform.position = Givepositon(CardTurret, tiledata.Tile);
        if(RemoveCardTurret) CardTurret = null;
        tiledata.Turret = turret;
        selectionData.Clear();
    }

    private void DestroyTurret()
    {
        Destroy(tiledata.Turret);
        tiledata.Turret = null;
    }

    private Vector3 Givepositon(GameObject Turret, GameObject tile)
    {
        return new Vector3(tile.transform.position.x, Turret.transform.position.y, tile.transform.position.z);
    }

    private void MoveTurret(GameObject tile)
    {
        CanMoveTurret = false;
        selectionData.Turret.transform.SetParent(tile.transform);
        selectionData.Turret.transform.position = Givepositon(selectionData.Turret, tile);
        if(tile == tiledata.Tile) tiledata.Turret = selectionData.Turret;
        selectionData.Clear();
    }

    private bool CanUpgrade()
    {
        foreach(var rule in mergeRules)
        {
            if((rule.FirstTurretId == tiledata.Turret.GetComponent<Id>().TowerId && rule.SecondTurretId == selectionData.Turret.GetComponent<Id>().TowerId) ||
                (rule.FirstTurretId == selectionData.Turret.GetComponent<Id>().TowerId && rule.SecondTurretId == tiledata.Turret.GetComponent<Id>().TowerId))
            {
                CardTurret = rule.ResultTurret;
                return true;
            }
        }
        return false;
    }

    
}

[System.Serializable]
public class TileData
{
    public GameObject Tile;
    public GameObject Turret;

    public void SetData(GameObject tile, GameObject Turret)
    {
        Tile = tile;
        this.Turret = Turret;
    }
    public void Clear()
    {
        Tile = null;
        Turret = null;
    }
}

[System.Serializable]
public class SelectionData
{
    public GameObject Tile;
    public GameObject Turret;
    public void SetData(GameObject tile, GameObject Turret)
    {
        Tile = tile;
        this.Turret = Turret;
    }
    public void Clear()
    {
        Tile = null;
        Turret = null;
    }
}

[System.Serializable]
public class MergeRules
{
    public int FirstTurretId;
    public int SecondTurretId;
    public GameObject ResultTurret;
}