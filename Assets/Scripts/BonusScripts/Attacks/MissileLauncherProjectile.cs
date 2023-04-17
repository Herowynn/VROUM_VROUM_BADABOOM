using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissileLauncherProjectile : MonoBehaviour
{
    public float FollowSpeed;
    public float _clock;


    Rigidbody _targetRB;
    Rigidbody _projectileRB;
    Vector3 _direction;
    [SerializeField] float _launchingSpeed;
    [SerializeField] float _lastPhaseSpeed;
    float _stageOne = 1f;
    float _stageTwo = 2f;
    float _dist;
    bool _isRotated = false;
    Vector3 _explosionDir;

    [Header("Audio")]
    public AudioClip MissileFlySound;
    public AudioClip[] MissileExplosionSounds;
    AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        _projectileRB = GetComponent<Rigidbody>();
       
        //_dist = (_targetRB.transform.position - transform.position).magnitude;
    }

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

    void Update()
    {
        if (_clock < _stageOne)
        {
            
            _projectileRB.AddForce(_direction*5, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _launchingSpeed);
        }

        if ( _clock > _stageOne && _clock < _stageTwo)
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
                _projectileRB.AddForce(_direction * -0.5f, ForceMode.Acceleration);
            }
            
        }

        if (_clock > _stageTwo)
        {
            if(_targetRB == null && _isRotated == true)
            {
                transform.Rotate(30, 0, 0);
                _isRotated = false;
            }
            _projectileRB.AddForce(transform.forward * 1.5f, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _lastPhaseSpeed);
        }

        _clock += Time.deltaTime;
    }

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
}
