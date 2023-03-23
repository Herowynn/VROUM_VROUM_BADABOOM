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
    public int rndBoostLvl = -1;
    public List<Material> BoostMaterials = new List<Material>();
    public MeshRenderer BonusMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (Type == BonusType.Attack) { }
        if (Type == BonusType.Boost)
        {
            rndBoostLvl = Random.Range(0, 3);
            BonusMeshRenderer.material = BoostMaterials[rndBoostLvl];
        }
        
    }


}
