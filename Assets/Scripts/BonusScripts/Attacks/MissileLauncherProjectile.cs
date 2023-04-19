using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissileLauncherProjectile : MonoBehaviour
{
    public float FollowSpeed;

    [SerializeField] private float _launchingSpeed;
    [SerializeField] private float _lastPhaseSpeed;
    private Rigidbody _targetRB;
    private Rigidbody _projectileRB;
    private Vector3 _direction;
    private float _stageOne = 1f;
    private float _stageTwo = 2f;
    private bool _isRotated = false;
    private Vector3 _explosionDir;
    private float _clock;
    private float _actualSpeed;

    [Header("Audio")]
    public AudioClip MissileFlySound;
    public AudioClip[] MissileExplosionSounds;
    private AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        _projectileRB = GetComponent<Rigidbody>();
        _clock = 0;
        StartCoroutine(WaitBeforeAutoDestroy());
    }

    /// <summary>
    /// This method plays the missile sound effect, assigns the target rigid body if the target is not null
    /// and set the missile direction variable.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="explodeDir"></param>
    public void Init(GameObject go, Vector3 explodeDir)
    {
        _source.clip = MissileFlySound;
        _source.loop = true;
        _source.Play();

       if(go) 
            _targetRB = go.GetComponent<Rigidbody>();

        _direction = transform.forward;
        _explosionDir = explodeDir;
    }

    /// <summary>
    /// This method manages the trajectory and the behaviour of the missile according to the missile course stage.
    /// </summary>
    private void FixedUpdate()
    {
        if (_clock < _stageOne)
        {

            _projectileRB.AddForce(_direction * 5, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _launchingSpeed);
        }

        if (_clock > _stageOne && _clock < _stageTwo)
        {
            if (_targetRB != null)
            {
                _projectileRB.velocity = new Vector3(0, 0, 0);
                transform.position = Vector3.Lerp(transform.position, _targetRB.transform.position, FollowSpeed * Time.deltaTime) ;
                transform.LookAt(_targetRB.transform);
            }
            else
            {
                if (_isRotated == false)
                {
                    transform.Rotate(new Vector3(30, 0, 0));
                    _direction = transform.forward;
                    _isRotated = true;
                }
                _projectileRB.AddForce(_direction * 1000f);
                _actualSpeed = _lastPhaseSpeed;
            }
        }

        if (_clock > _stageTwo)
        {
            if (_targetRB == null && _isRotated == true)
            {
                transform.Rotate(30, 0, 0);
                _isRotated = false;
            }
            _projectileRB.AddForce(transform.forward * 1000f);
            _actualSpeed = _lastPhaseSpeed;
        }

        _clock += Time.deltaTime;
        _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _actualSpeed);
    }

    /// <summary>
    /// This method verifies that the object touched is a car controller and sets sound effects and car variables
    /// accordingly if it is the case.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Vector3 dir = other.transform.position - _explosionDir;

        if (other.gameObject.TryGetComponent<CarController>(out var carControl))
        {
            carControl.Source.clip = MissileExplosionSounds[Random.Range(0, MissileExplosionSounds.Length)];
            carControl.Source.loop = false;
            carControl.Source.Play();

            carControl.ExplosionDirection = dir;
            carControl.IsExplosed = true;
            Destroy(gameObject);
        }
    }

    IEnumerator WaitBeforeAutoDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
