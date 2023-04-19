using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : GlobalController
{
    [HideInInspector] public Transform[] NodesToFollow;

    [Header("GD")]
    public float Speed;

    private int _targetNode;

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE)
        {
            UpdateMove(NodesToFollow);
        }
    }

    public void ResetToTransform(Transform resetTransform)
    {
        transform.position = resetTransform.position;
        transform.rotation = resetTransform.rotation;

        UpdateTargetNodeAfterReset();
    }

    private void UpdateTargetNodeAfterReset()
    {
        int newTargetNode = 0;

        float distance = float.MaxValue;

        for (int i = 0; i < NodesToFollow.Length; i++)
        {
            if (distance > Vector3.Distance(NodesToFollow[i].transform.position, transform.position) && Mathf.Rad2Deg * Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.forward.normalized, (NodesToFollow[i].transform.position - transform.position).normalized))) < 90f)
            {
                newTargetNode = i;
                distance = Vector3.Distance(NodesToFollow[i].transform.position, transform.position);
            }
        }

        _targetNode = newTargetNode;
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
                _targetNode = 0;

            target = path[_targetNode].transform.position;
            moveStep = Speed * Time.deltaTime;
            distance = Vector3.Distance(target, transform.position);
            direction = target - transform.position;

            //orientation
            transform.rotation = path[_targetNode].transform.rotation;
        }

        direction.Normalize();
        SphereRB.position += moveStep * direction;
    }
}
