using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BonusType
{
    Attack,
    Boost
}
public class Bonus : MonoBehaviour
{
    public BonusType Type;
    public int rndLvl = -1;
    public List<Material> Materials = new List<Material>();
    public MeshRenderer BonusMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (Type == BonusType.Attack) 
        {
            rndLvl = Random.Range(0, 4);
            BonusMeshRenderer.material = Materials[rndLvl];
        }
        if (Type == BonusType.Boost)
        {
            rndLvl = Random.Range(0, 3);
            BonusMeshRenderer.material = Materials[rndLvl];
        }
        
    }
}
