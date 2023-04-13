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
    public int RndLvl = -1;
    public List<Material> Materials = new List<Material>();
    public MeshRenderer BonusMeshRenderer;

    void Start()
    {
        if (Type == BonusType.Attack) 
        {
            RndLvl = 0;//Random.Range(0, Materials.Count);
            BonusMeshRenderer.material = Materials[RndLvl];
        }
        if (Type == BonusType.Boost)
        {
            RndLvl = Random.Range(0, Materials.Count);
            BonusMeshRenderer.material = Materials[RndLvl];
        }
        
    }
}
