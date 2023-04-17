using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [Header("GD")]
    public float Speed;

    private Vector3 _direction;

    public Vector3 direction { set { _direction = value; } }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            transform.position += _direction.normalized * Speed * Time.deltaTime;
            transform.forward = _direction;
            Debug.Log(transform.position);
        }
    }

    public void ResetToTransform(Transform resetTransform)
    {
        transform.position = resetTransform.position;
        transform.rotation = resetTransform.rotation;
    }
}
