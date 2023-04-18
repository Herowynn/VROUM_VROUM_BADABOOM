using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    public float speed;

    private Vector3 _direction;

    public Vector3 direction { set { _direction = value; } }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            transform.position += _direction.normalized * speed * Time.deltaTime;
            transform.forward = _direction;
        }
    }
}
