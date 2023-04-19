using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : GlobalController
{
    [HideInInspector] public Transform[] NodesToFollow;

    [Header("GD")]
    public float Speed;

    private int _targetNode;

    public void Start()
    {
        Init();
        NodesToFollow = GameManager.Instance.MapManager.CurrentMap.HarvesterNodes;
        SetTargetNode(0);
    }

    public void SetTargetNode(int index)
    {
        _targetNode = index + 1;
    }

    private void FixedUpdate()
    {
        UpdateGraphics();
        transform.position = SphereReference.transform.position + _carAltitudeOffset;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE)
        {
            UpdateMove(NodesToFollow);
        }
    }

    private void UpdateMove(Transform[] path)
    {
        Vector3 target = path[_targetNode].transform.position;
        Vector3 direction = target - SphereRB.position;
        float moveStep = Speed * Time.deltaTime;
        float distance = Vector3.Distance(target, SphereRB.position);

        while (moveStep > distance)
        {
            _targetNode++;

            if (_targetNode >= path.Length)
                _targetNode = 0;

            target = path[_targetNode].transform.position;
            moveStep = Speed * Time.deltaTime;
            distance = Vector3.Distance(target, SphereRB.position);
            direction = target - SphereRB.position;

            //orientation
            transform.LookAt(path[_targetNode].transform);
            //transform.rotation = path[_targetNode].transform.rotation;
        }

        direction.Normalize();
        SphereRB.position += moveStep * direction;
    }
}
