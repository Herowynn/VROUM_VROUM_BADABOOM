using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BonusType
{
    Attack,
    Boost
}

public class Bonus : MonoBehaviour
{
    public BonusType Type;
    public int RndLvl = -1;
    public List<GameObject> BonusSkin = new List<GameObject>();
    //public MeshRenderer BonusMeshRenderer;

    void Start()
    {
        if (Type == BonusType.Attack) 
        {
            RndLvl = Random.Range(0, BonusSkin.Count);
            //BonusMeshRenderer.material = BonusSkin[RndLvl];
            Instantiate(BonusSkin[RndLvl], transform.position, transform.rotation, transform);
        }
        if (Type == BonusType.Boost)
        {
            RndLvl = Random.Range(0, BonusSkin.Count);
            //BonusMeshRenderer.material = BonusSkin[RndLvl];
            Instantiate(BonusSkin[RndLvl], transform.position, transform.rotation, transform);
        }
        
    }
    private void Update()
    {
        transform.Rotate(0.33f*new Vector3(0, 1, 0));
    }
}
