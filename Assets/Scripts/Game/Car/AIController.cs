using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : GlobalController
{
    [HideInInspector] public List<Transform> NodesToFollow;

    [Header("GD")]
    public float MaxSpeed;
    public float MinSpeed;
    float _speed;

    private int _targetNode;

    private Vector3 _target;
    private Vector3 _direction;
    private Vector3 _lookingDirection;
    private float _moveStep;
    private float _distance;
    private float _formerDistance;
    private int _formerTargetNode;


    public void Start()
    {
        Init();
        _speed = Random.Range(MinSpeed, MaxSpeed);
        _formerDistance = -float.MaxValue;
        _formerTargetNode = -1;
        NodesToFollow = GameManager.Instance.MapManager.CurrentMap.HarvesterNodes;
        SetTargetNode(0);
    }

    public void SetTargetNode(int index)
    {
        _targetNode = index;
        UpdateTargetPosition();
    }

    private void FixedUpdate()
    {
        UpdateGraphics();
        transform.position = SphereReference.transform.position + _carAltitudeOffset;
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE)
            UpdateMove(NodesToFollow);

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
        
        if (BoostsContainer.transform.childCount <= 0)
            yield break;
        
        BoostsContainer.transform.GetChild(0).GetComponent<Booster>().Boost(SphereRB, gameObject);
        ProfileUI.UseBoost();
    }

    IEnumerator UseAttackBonus()
    {
        yield return new WaitForSeconds(Random.Range(1f, 6f));

        if (AttacksContainer.transform.childCount <= 0)
            yield break;
        
        AttacksContainer.transform.GetChild(0).GetComponent<Offensive>().Shoot();
        ProfileUI.UseWeapon();
    }

    private void UpdateMove(List<Transform> path)
    {
        _direction = _target - SphereRB.position;
        _moveStep = _speed * Time.deltaTime;
        _distance = Vector3.Distance(_target, SphereRB.position);

        while (_moveStep > _distance)
            UpdateTargetAndRelatedInformation(path.Count);

        if (_formerDistance > 0 && _formerTargetNode >= 0)
        {
            if (_formerTargetNode == _targetNode && _formerDistance < _distance)
            {
                UpdateTargetAndRelatedInformation(path.Count);

                while (_moveStep > _distance)
                    UpdateTargetAndRelatedInformation(path.Count);
            }
        }

        if (_formerTargetNode >= 0 && _formerTargetNode != _targetNode)
            _lookingDirection = _target - SphereRB.transform.position;

        if (_distance > GetComponent<BoxCollider>().size.z)
            transform.LookAt(_target);
        else
            transform.LookAt(transform.position + _lookingDirection.normalized * 10f);

        _direction.Normalize();
        SphereRB.position += _moveStep * _direction;
        _formerTargetNode = _targetNode;
        _formerDistance = _distance;
    }

    private void UpdateTargetPosition()
    {
        _speed = Random.Range(MinSpeed, MaxSpeed);
        Vector3 xzNodePosition = new Vector3(NodesToFollow[_targetNode].position.x, 0, NodesToFollow[_targetNode].position.z);
        Vector3 positionOffset = Random.Range(-4.4f, 4.4f) * NodesToFollow[_targetNode].right.normalized + Random.Range(-2f, 2f) * NodesToFollow[_targetNode].forward.normalized + SphereReference.transform.position.y * Vector3.up;
        _target = xzNodePosition + positionOffset;
    }

    private void UpdateTargetAndRelatedInformation(int numberOfNodes)
    {
        _targetNode++;

        if (_targetNode >= numberOfNodes)
            _targetNode = 0;

        UpdateTargetPosition();
        _moveStep = _speed * Time.deltaTime;
        _distance = Vector3.Distance(_target, SphereRB.position);
        _direction = _target - SphereRB.position;
    }
}
