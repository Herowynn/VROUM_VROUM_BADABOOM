using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : GlobalController
{
    #region Public Fields

    [HideInInspector] public List<Transform> NodesToFollow;
    [HideInInspector] public string Feedback;
    public Material RedMaterial;

    [Header("GD")]
    public float ClassicSpeed;
    public float BrutalDiffSpeed;
    public float MinimumTimeBeforeActivatingBonus;
    public float MaximumTimeBeforeActivatingBonus;
    public float BrutalDiffMaximumTimeBeforeActivatingBonus;
    public float TargetPositionWidthRandomOffset;
    public float TargetPositionForwardRandomOffset;
    public float DistanceToNodeLimitForIncrementation;
    public float MaximumDistanceToCarForTargetting;
    public float SpeedSlowFactorDuringTurning;

    #endregion

    #region Private Fields

    private float _speed;
    private float _previousDistance;
    private int _targetNodeIndex;
    private Transform _targetNodeTransform;
    private Vector3 _targetPosition;
    private GameObject _targetCar;
    private Vector3 _targetCarPosition;
    private Vector3 _aiWantedDirection;

    #endregion

    public GameObject TargetCar { set {  _targetCar = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }

    #region Unity Methods

    public void Start()
    {
        Init();
        _speed = (GameManager.Instance.MultipleInputManager.AiDifficulty == AIDifficulty.Brutal) ? BrutalDiffSpeed : ClassicSpeed; 
        NodesToFollow = GameManager.Instance.MapManager.CurrentMap.HarvesterNodes;
        _targetNodeIndex = 1;
        _targetCar = null;
        _targetCarPosition = Vector3.zero;
        _previousDistance = -float.MaxValue;
        UpdateDirectionAndTargetNode();
    }

    private void FixedUpdate()
    {
        if (PlayerState == PlayerState.ALIVE)
            UpdateGraphics();
    }

    private void Update()
    {
        Feedback = $"Index of the target node : {_targetNodeIndex} - Position of the target node :" +
            $" {_targetNodeTransform.position} - Target position : {_targetPosition}";

        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE && _isGrounded)
        {
            Rotate();
            Move();

            if (_hasABoost)
            {
                _hasABoost = false;
                StartCoroutine(UseBoost());
            }

            // When the car targetted by this one drove too far from this one, when stop targetting it and come back to a
            // normal behaviour.
            if (_targetCar != null && Vector3.Distance(SphereReference.transform.position, _targetCarPosition) > MaximumDistanceToCarForTargetting)
            {
                _targetCarPosition = Vector3.zero;
                _targetCar = null;
            }

            // If this car picked an attack bonus, it search for a target in front of him and not too far.
            // If there is one, it targets it and the targetCarPosition is updated every frame.
            // Otherwise, we use the basic behaviour (coroutine that will shoot in the random next seconds).
            if (_hasAnAttackBonus)
            {
                // If the difficulty is set to normal, there is no targetting behaviour.
                if (GameManager.Instance.MultipleInputManager.AiDifficulty == AIDifficulty.Normal)
                {
                    _hasAnAttackBonus = false;
                    StartCoroutine(UseAttackBonus());
                }
                else
                {
                    if (_targetCar == null)
                        _targetCar = GetTargetCar();

                    if (_targetCar != null)
                        _targetCarPosition = _targetCar.GetComponent<GlobalController>().SphereReference.transform.position;
                    else
                    {
                        _hasAnAttackBonus = false;
                        StartCoroutine(UseAttackBonus());
                    }
                }
            }
        }
        else if (GameManager.Instance.GameState != GameState.RACING)
        {
            SphereReference.transform.position = _lastRebornPosition;
            SphereRB.velocity = Vector3.zero;
        }

        transform.position = SphereReference.transform.position + _carAltitudeOffset;
    }

    #endregion

    #region Movements Calculation Methods

    private void Rotate()
    {
        // If there is a car target, the looking direction is the direction towards the target car.
        // Otherwise, the looking direction is the direction towards the next node direction (with the correct altitude).
        Vector3 lookingDirection = _targetCar == null ? _aiWantedDirection : _targetCarPosition - SphereReference.transform.position;

        Vector3 cross = Vector3.Cross(transform.forward, lookingDirection);
        float carSignRotation = Mathf.Sign(cross.y);

        if (Mathf.Abs(Vector3.Angle(transform.forward, lookingDirection)) > 10f)
            transform.Rotate(transform.up, carSignRotation * TurnStrength * Time.deltaTime);
        else
        {
            transform.forward = lookingDirection;

            // If there is a target and the car is aligned with it, the attack bonus is used.
            if (_targetCar != null)
            {
                _hasAnAttackBonus = false;
                AttacksContainer.transform.GetChild(0).GetComponent<Offensive>().Shoot();
                ProfileUI.UseWeapon();
                _targetCarPosition = Vector3.zero;
                _targetCar = null;
            }
        }
    }

    private void Move()
    {
        float moveStep = _speed * SlowFactor * Time.deltaTime;
        float distance = Vector3.Distance(_targetPosition, SphereReference.transform.position);

        // If the vehicule moves away from the next node when it arrives close to it (in a close area behind the node),
        // it means that the car follows a target or has been pushed in another direction by an attack bonus.
        // Hence, the target node index is incremented as are the corresponding values (target node position...).
        if (_previousDistance > 0 && distance > _previousDistance)
        {
            if (Mathf.Abs(Vector3.Angle(_targetNodeTransform.forward, SphereReference.transform.position - _targetNodeTransform.position))
                < 90f || Vector3.Distance(_targetPosition, SphereReference.transform.position) < DistanceToNodeLimitForIncrementation)
            {
                IncrementTargetNodeIndex();
                distance = Vector3.Distance(_targetPosition, SphereReference.transform.position);
            }
        }

        // If the car is closer to the next node than the distance it has to drive in this frame, the target node index
        //  is incremented and all the corresponding values are updated (target node position, wanted direction...).
        if (moveStep > distance)
        {
            IncrementTargetNodeIndex();
            distance = Vector3.Distance(_targetPosition, SphereReference.transform.position);
        }

        // If the car is currently turning and not aligned enough with the wanted direction,
        // the speed of the car is slower than the normal speed (slow factor applied to the random speed choosen).
        if (_targetCar != null)
            moveStep = (Mathf.Abs(Vector3.Angle(_targetCarPosition - SphereReference.transform.position, transform.forward))
                < 10f ? moveStep : moveStep / SpeedSlowFactorDuringTurning);
        else
            moveStep = (Mathf.Abs(Vector3.Angle(_aiWantedDirection, transform.forward)) < 10f ? moveStep :
                moveStep / SpeedSlowFactorDuringTurning);

        SphereReference.transform.position += transform.forward.normalized * moveStep;
        _previousDistance = distance;
    }

    private void UpdateDirectionAndTargetNode()
    {
        _targetNodeTransform = NodesToFollow[_targetNodeIndex];
        _targetPosition = _targetNodeTransform.position + _targetNodeTransform.right.normalized * Random.Range(-TargetPositionWidthRandomOffset, TargetPositionWidthRandomOffset) +
            _targetNodeTransform.forward.normalized * Random.Range(-TargetPositionForwardRandomOffset, TargetPositionForwardRandomOffset) -
            _targetNodeTransform.up.normalized * GameManager.Instance.MapManager.CurrentMap.AltitudeDifferenceBetweenHarvesterAndCar;

        _aiWantedDirection = _targetPosition - SphereReference.transform.position;
    }

    private void IncrementTargetNodeIndex()
    {
        _targetNodeIndex++;

        if (_targetNodeIndex >= NodesToFollow.Count)
            _targetNodeIndex = 0;

        UpdateDirectionAndTargetNode();
    }

    public void UpdateTargetNodeOnReborn(Transform rebornTransform)
    {
        int targetNodeIndex = -1;
        float distanceToNode = float.MaxValue;

        for(int i = 0; i < NodesToFollow.Count; i++)
        {
            Vector3 xzNodePosition = new Vector3(NodesToFollow[i].position.x, rebornTransform.position.y, NodesToFollow[i].position.z);
            Vector3 rebornPointToNodeVector = xzNodePosition - rebornTransform.position;
            float currentDistance = rebornPointToNodeVector.magnitude;

            if(Mathf.Abs(Vector3.Angle(rebornTransform.forward, rebornPointToNodeVector)) < 90f && currentDistance < distanceToNode)
            {
                distanceToNode = currentDistance;
                targetNodeIndex = i;
            }
        }

        _targetNodeIndex = targetNodeIndex;
        UpdateDirectionAndTargetNode();
    }

    #endregion

    /// <summary>
    /// Compares the distance between every other "alive" car and this car.
    /// If the car with the smallest distance to this one is in front of it and close enough, it becomes a target to shoot.
    /// </summary>
    /// <returns></returns>
    private GameObject GetTargetCar()
    {
        float distance = float.MaxValue;
        GameObject targetPlayer = null;

        foreach (GameObject player in GameManager.Instance.PlayersManager.Players)
        {
            if (player.GetComponent<GlobalController>().PlayerState == PlayerState.ALIVE && player != gameObject)
            {
                GameObject otherSphereReference = player.GetComponent<GlobalController>().SphereReference;
                float currentDistance = Vector3.Distance(SphereReference.transform.position, otherSphereReference.transform.position);
                float forwardAngle = Mathf.Abs(Vector3.Angle(transform.forward, otherSphereReference.transform.position -
                    SphereReference.transform.position));

                if (forwardAngle < 90f && currentDistance < distance)
                {
                    distance = currentDistance;
                    targetPlayer = player;
                }
            }
        }

        return (distance < MaximumDistanceToCarForTargetting) ? targetPlayer : null;
    }

    private IEnumerator UseBoost()
    {
        yield return new WaitForSeconds(Random.Range(MinimumTimeBeforeActivatingBonus, (GameManager.Instance.MultipleInputManager.AiDifficulty
            == AIDifficulty.Brutal) ? BrutalDiffMaximumTimeBeforeActivatingBonus : MaximumTimeBeforeActivatingBonus));
        BoostsContainer.transform.GetChild(0).GetComponent<Booster>().Boost(SphereRB, gameObject);
        ProfileUI.UseBoost();
    }

    private IEnumerator UseAttackBonus()
    {
        yield return new WaitForSeconds(Random.Range(MinimumTimeBeforeActivatingBonus, (GameManager.Instance.MultipleInputManager.AiDifficulty
            == AIDifficulty.Brutal) ? BrutalDiffMaximumTimeBeforeActivatingBonus : MaximumTimeBeforeActivatingBonus));
        AttacksContainer.transform.GetChild(0).GetComponent<Offensive>().Shoot();
        ProfileUI.UseWeapon();
    }
}
