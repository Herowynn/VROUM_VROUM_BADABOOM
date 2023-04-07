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

    void Start()
    {
        if (Type == BonusType.Attack) 
        {
            rndLvl = 2;//Random.Range(0, Materials.Count);
            BonusMeshRenderer.material = Materials[rndLvl];
        }
        if (Type == BonusType.Boost)
        {
            rndLvl = Random.Range(0, Materials.Count);
            BonusMeshRenderer.material = Materials[rndLvl];
        }
        
    }
}
