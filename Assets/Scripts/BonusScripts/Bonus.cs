using System.Collections.Generic;
using UnityEngine;

public enum BonusType
{
    Attack,
    Boost
}

/// <summary>
/// Generic bonuses class.
/// </summary>
public class Bonus : MonoBehaviour
{
    public BonusType Type;
    public int RndLvl = -1;
    public List<GameObject> BonusSkins = new List<GameObject>();

    /// <summary>
    /// Randomly instantiate the bonus and the bonus skin according to the type.
    /// </summary>
    void Start()
    {
        RndLvl = Random.Range(0, BonusSkins.Count);

        GameObject bonusSkin = Instantiate(BonusSkins[RndLvl], transform.position, transform.rotation, transform);

        BoxCollider bonusSkinCollider = bonusSkin.GetComponent<BoxCollider>();

        BoxCollider bonusCollider = GetComponent<BoxCollider>();
        
        bonusCollider.center = new Vector3(bonusSkinCollider.center.x * bonusSkin.transform.localScale.x,
            bonusSkinCollider.center.y * bonusSkin.transform.localScale.y, bonusSkinCollider.center.z * bonusSkin.transform.localScale.z);
        
        bonusCollider.size = new Vector3(bonusSkinCollider.size.x * bonusSkin.transform.localScale.x,
            bonusSkinCollider.size.y * bonusSkin.transform.localScale.y, bonusSkinCollider.size.z * bonusSkin.transform.localScale.z);
        
        Destroy(bonusSkinCollider);
    }
    
    private void Update()
    {
        transform.Rotate(0.33f * new Vector3(0, 1, 0));

        if (!transform.GetChild(0).GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<DestructorComponent>())
            Destroy(gameObject);
    }
}
