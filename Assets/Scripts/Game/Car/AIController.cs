using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : GlobalController
{
    [HideInInspector] public Transform[] NodesToFollow;

    [Header("GD")]
    public float MaxSpeed;
    public float MinSpeed;
    float _speed;

    private int _targetNode;

    Vector3 target;
    Vector3 direction;
    float moveStep;
    float distance;


    public void Start()
    {
        Init();
        _speed = Random.Range(MinSpeed, MaxSpeed);
        NodesToFollow = GameManager.Instance.MapManager.CurrentMap.HarvesterNodes;
        SetTargetNode(0);

        UpdateTarget();
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

        if (_hasABoost)
        {
            _hasABoost = false;
            StartCoroutine(UseBoost());
        }

        if(_hasAnAttackBonus)
        {
            _hasAnAttackBonus = false;
            StartCoroutine(UseAttackBonus());
            //if (Physics.Raycast(RayPoint.position, -transform.up, Mathf.Infinity, CarLayerMask))
            //{
            //    AttacksContainer.transform.GetChild(0).GetComponent<Offensive>().Shoot();
            //    ProfileUI.UseWeapon();
            //}
        }
    }

    IEnumerator UseBoost()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        BoostsContainer.transform.GetChild(0).GetComponent<Booster>().Boost(SphereRB, gameObject);
        ProfileUI.UseBoost();
    }

    IEnumerator UseAttackBonus()
    {
        yield return new WaitForSeconds(1);

        if (AttacksContainer.transform.GetComponentInChildren<Offensive>())
        {
            AttacksContainer.transform.GetChild(0).GetComponent<Offensive>().Shoot();
            ProfileUI.UseWeapon();
        }
    }

    private void UpdateMove(Transform[] path)
    {
        direction = target - SphereRB.position;
        moveStep = _speed * Time.deltaTime;
        distance = Vector3.Distance(target, SphereRB.position);

        while (moveStep > distance)
        {
            _targetNode++;

            if (_targetNode >= path.Length)
                _targetNode = 0;

            UpdateTarget();
            moveStep = _speed * Time.deltaTime;
            distance = Vector3.Distance(target, SphereRB.position);
            direction = target - SphereRB.position;
        }

        direction.Normalize();
        transform.LookAt(target);
        SphereRB.position += moveStep * direction;
    }

    void UpdateTarget()
    {
        _speed = Random.Range(MinSpeed, MaxSpeed);
        target = NodesToFollow[_targetNode].transform.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}
