using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [Header("Instance")] 
    [HideInInspector] public Transform[] NodesToFollow;
    
    [Header("GD")]
    public float Speed;

    private Vector3 _direction;
    private int _targetNode;

    public Vector3 direction { set { _direction = value; } }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            UpdateMove(NodesToFollow);
        }
    }

    public void ResetToTransform(Transform resetTransform)
    {
        transform.position = resetTransform.position;
        transform.rotation = resetTransform.rotation;
    }

    public void InitiateNodesToFollow(Transform[] nodes)
    {
        NodesToFollow = nodes;
        _targetNode = 0;
    }

    private void UpdateMove(Transform[] path)
    {
        Vector3 target = path[_targetNode].transform.position;
        Vector3 direction = target - transform.position;
        float moveStep = Speed * Time.deltaTime;
        float distance = Vector3.Distance(target, transform.position);

        while (moveStep > distance)
        {
            _targetNode++;
            
            if (_targetNode >= path.Length)
                return;

            target = path[_targetNode].transform.position;
            moveStep = Speed * Time.deltaTime;
            distance = Vector3.Distance(target, transform.position);
            direction = target - transform.position;
        }
        
        direction.Normalize();
        transform.position += moveStep * direction;
    }
}
