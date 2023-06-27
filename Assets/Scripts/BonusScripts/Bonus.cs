using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public List<GameObject> BonusSkin = new List<GameObject>();

    /// <summary>
    /// Randomly instantiate the bonus and the bonus skin according to the type.
    /// </summary>
    void Start()
    {
        if (Type == BonusType.Attack) 
        {
            RndLvl = Random.Range(0, BonusSkin.Count);
            Instantiate(BonusSkin[RndLvl], transform.position, transform.rotation, transform);
        }
        if (Type == BonusType.Boost)
        {
            RndLvl = Random.Range(0, BonusSkin.Count);
            Instantiate(BonusSkin[RndLvl], transform.position, transform.rotation, transform);
        }
        
    }
    
    private void Update()
    {
        transform.Rotate(0.33f * new Vector3(0, 1, 0));

        if (!transform.GetChild(0).GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }

    /// <summary>
    /// This coroutine destroys this pickable booster object after a certain amount of time.
    /// </summary>
    /// <returns></returns>
/*    IEnumerator WaitBeforeAutoDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<DestructorComponent>())
            Destroy(gameObject);
    }
}
