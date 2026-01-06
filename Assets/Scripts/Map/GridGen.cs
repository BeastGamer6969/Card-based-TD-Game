using UnityEngine;
[ExecuteInEditMode]
public class GridGen : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] int Rows;
    [SerializeField] int Columns;
    [SerializeField] float Spacing;
    [SerializeField] bool Enabled = false;
    Vector3 Startpos;
    void Start()
    {
        for(int i = transform.childCount-1; i >= 0; i--)
        {
            if(Enabled) DestroyImmediate(transform.GetChild(i).gameObject);
        }
        
        Startpos = new Vector3(-((Rows -1)*Spacing/2), 0, -((Columns -1)*Spacing/2));
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Vector3 pos = Startpos + new Vector3(i * Spacing, 0,j * Spacing);
                Instantiate(cubePrefab, transform.TransformPoint(pos), Quaternion.identity, transform);
            }
        }
        
        enabled = false;
    }
}
