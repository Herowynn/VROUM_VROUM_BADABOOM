using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    #region Public Fields

    [Header("GD")]
    public Rigidbody SphereRB;
    public float ForwardAccel;
    public float MaximumSpeed;
    public float TurnStrength;
    public float DragOnGround;
    public float DragInTheAir;
    public float WheelTurnSpeed;
    public float AdditionalEarthGravity;
    public float SlowFactor = 1f;

    [Header("Layers")]
    public LayerMask GroundLayerMask;
    public LayerMask CarLayerMask;
    public int GroundLayerNumber;
    public float ArrayRayLength;
    public Transform RayPoint;
    public GameObject RightBackWheel;
    public GameObject RightFrontWheel;
    public GameObject LeftBackWheel;
    public GameObject LeftFrontWheel;

    [Header("Bonus Effects")]
    public int BumpForce = 10000;
    public int ProjectileForce = 100;
    public Vector3 BumpDirection;
    public Vector3 ExplosionDirection;
    public Vector3 ProjectileDirection;

    [Header("Bonus")]
    public Transform AttacksContainer;
    public Transform BoostsContainer;
    public GameObject[] AttackList;
    public GameObject[] BoostList;

    [Header("Audio")]
    public AudioSource Source;
    public AudioClip GroundHitSound;
    public AudioClip BaseSound;
    public AudioClip MovingSound;

    [Header("Info")]
    public Color Color;
    public PlayerState PlayerState;

    [Header("Instance")]
    public GameObject Visual;
    public GameObject SphereReference;
    public MeshRenderer BodyColor;
    public List<Material> CarColors;

    [Header("Score")]
    //Need to move to separate file (?)
    public int Score;

    //Need to move to separate file (?)
    [Header("UI")]
    public PointsUI PointsUI;
    public ProfileUI ProfileUI;

    #endregion

    #region Protected Fields

    protected Rigidbody _rigidbody;
    protected BoxCollider _boxCollider;
    protected Vector2 _movementInput;
    protected bool _isGrounded;
    protected bool _wasGrounded;
    protected bool _canMove;
    protected Vector3 _distArrowRayPoint;
    protected Vector3 _wantedDirection;
    protected float _yComponentWantedDirection;
    protected Vector3 _carAltitudeOffset;
    protected Vector3 _lastRebornPosition;
    protected bool _hasAnAttackBonus;
    protected bool _hasABoost;
    protected List<GameObject> _visibleCarParts = new List<GameObject>();

    protected bool _hitBySaw;
    protected bool _isBumped;
    protected bool _isExploded;
    protected bool _isTouchedByMachineGun;

    #endregion

    public List<GameObject> VisibleCarParts { get { return _visibleCarParts; } }
    public bool HitBySaw { set { _hitBySaw = value; } }
    public bool IsBumped { set { _isBumped = value; } }
    public bool IsExploded { set { _isExploded = value; } }
    public bool IsTouchedByMachineGun { set { _isTouchedByMachineGun = value; } }

    #region Unity Functions

    protected void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    // Function that should be call in the start function of children
    protected void Init()
    {
        SphereRB.transform.parent = null;

        _yComponentWantedDirection = 0f;

        _carAltitudeOffset = new Vector3(0, transform.position.y - SphereRB.transform.position.y, 0);

        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        CreatePlayerUis();

        PointsUI.ChangeVisualColoration(Color);
        PointsUI.ChangePointsCount(Score);
        BodyColor.material = CarColors[GameManager.Instance.PlayersManager.PlayerColors.IndexOf(Color)];

        for (int i = 0; i < GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions.Count; i++)
        {
            if (transform.GetSiblingIndex() == i)
                _lastRebornPosition = GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions[i].position;
        }

        _hasABoost = false;
        _hasAnAttackBonus = false;

        GameObject model = transform.GetChild(0).gameObject;
        for(int i = 0; i < model.transform.childCount; i++)
        {
            GameObject part = model.transform.GetChild(i).gameObject;
            if (part.transform.childCount == 0)
                _visibleCarParts.Add(part);
            else
            {
                for (int j = 0; j < part.transform.childCount; j++)
                    _visibleCarParts.Add(part.transform.GetChild(j).gameObject);
            }
        }
    }

    protected void UpdateGraphics()
    {
        _isGrounded = false;

        if (Physics.Raycast(RayPoint.position, -transform.up, out RaycastHit hit, ArrayRayLength, GroundLayerMask))
        {
            _isGrounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            if (hit.normal == new Vector3(0, 1, 0))
                _yComponentWantedDirection = 0;
            else
                _yComponentWantedDirection = transform.forward.y;
        }

        if (_hitBySaw && _isGrounded)
        {
            SphereRB.AddForce(transform.up * 20000, ForceMode.Impulse);
            StartCoroutine(WaitBeforeSawEnd());
        }

        if (_hitBySaw && !_isGrounded)
        {
            transform.Rotate(30 * new Vector3(0, 1, 0));
        }

        if (_isExploded && _isGrounded)
        {
            SphereRB.AddForce((ExplosionDirection + new Vector3(0, 100, 0)) * 200, ForceMode.Impulse);
            StartCoroutine(WaitBeforeMissileEnd());
        }

        if (_isExploded && !_isGrounded)
        {
            transform.Rotate(30 * new Vector3(1, 0, 0));
        }

        if (_isGrounded)
        {
            SphereRB.drag = DragOnGround;

            if (_canMove && _movementInput != Vector2.zero)
            {
                SphereRB.AddForce(_wantedDirection * ForwardAccel);
                SphereRB.velocity = Vector3.ClampMagnitude(SphereRB.velocity, MaximumSpeed * SlowFactor);
                Source.clip = MovingSound;
            }
        }
        else
        {
            SphereRB.drag = DragInTheAir;
            SphereRB.AddForce(new Vector3(0, -1, 0) * SphereRB.mass * AdditionalEarthGravity * 9.8f);
            Source.clip = BaseSound;
        }

        if (_isBumped)
        {
            SphereRB.AddForce(BumpDirection * BumpForce + new Vector3(0f, BumpForce, 0f), ForceMode.Impulse);
            _isBumped = false;
        }

        if (_isTouchedByMachineGun)
        {
            SphereRB.AddForce(ProjectileDirection * ProjectileForce, ForceMode.Impulse);
            _isTouchedByMachineGun = false;
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bonus>() != null)
        {
            switch (collision.gameObject.GetComponent<Bonus>().Type)
            {
                case BonusType.Attack:
                    if (AttacksContainer.transform.childCount != 0)
                        return;
                    Instantiate(AttackList[collision.gameObject.GetComponent<Bonus>().RndLvl], AttacksContainer);
                    ProfileUI.TookWeapon();
                    _hasAnAttackBonus = true;
                    break;
                case BonusType.Boost:
                    if (BoostsContainer.transform.childCount != 0)
                        return;
                    Instantiate(BoostList[collision.gameObject.GetComponent<Bonus>().RndLvl], BoostsContainer);
                    ProfileUI.TookBoost();
                    _hasABoost = true;
                    break;
            }

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == GroundLayerNumber && !_isGrounded)
            StartCoroutine(GetBackOnWheels());
        
        if (collision.gameObject.GetComponent<DestructorComponent>())
            GameManager.Instance.TriggerPlayerDestructionEvent(this);
    }

    #endregion

    #region Related to state in-game

    public void DiedEvent()
    {
        SphereReference.SetActive(false);
        Visual.SetActive(false);
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _boxCollider.enabled = false;

        _isExploded = false;
        _hitBySaw = false;
        _isBumped = false;
        _isTouchedByMachineGun = false;

        if (AttacksContainer.transform.childCount > 0)
            Destroy(AttacksContainer.transform.GetChild(0).gameObject);
        if (BoostsContainer.transform.childCount > 0)
            Destroy(BoostsContainer.transform.GetChild(0).gameObject);

        if (this.GetType() == typeof(AIController))
        {
            GetComponent<AIController>().StopAllCoroutines();
            Debug.Log(GetComponent<AIController>().Feedback);
        }

        PlayerState = PlayerState.DEAD;
    }

    public void RebornEvent(Transform positionOnReborn)
    {
        ClearMyBonus();

        ProfileUI.ResetProfile();

        SphereReference.SetActive(true);
        Visual.SetActive(true);
        _rigidbody.constraints = RigidbodyConstraints.None;
        _boxCollider.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        PlayerState = PlayerState.ALIVE;

        transform.rotation = positionOnReborn.rotation;
        SphereRB.velocity = Vector3.zero;
        SphereReference.transform.position = positionOnReborn.position;
        _lastRebornPosition = positionOnReborn.position;
    }

    public void AddPointsToScore(int points)
    {
        Score += points;
        PointsUI.ChangePointsCount(Score);
    }

    private IEnumerator WaitBeforeSawEnd()
    {
        yield return new WaitForSeconds(2f);
        _hitBySaw = false;
    }

    private IEnumerator WaitBeforeMissileEnd()
    {
        yield return new WaitForSeconds(2f);
        _isExploded = false;
    }

    private IEnumerator GetBackOnWheels()
    {
        yield return new WaitForSeconds(2f);
        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
    }

    private void ClearMyBonus()
    {
        if (BoostsContainer.transform.childCount != 0)
            Destroy(BoostsContainer.transform.GetChild(0).gameObject);

        if (AttacksContainer.transform.childCount != 0)
            Destroy(AttacksContainer.transform.GetChild(0).gameObject);
    }

    #endregion

    #region Related to UI

    private void CreatePlayerUis()
    {
        GameManager.Instance.TriggerUiCreationForPlayerEvent(this);
    }

    #endregion
}