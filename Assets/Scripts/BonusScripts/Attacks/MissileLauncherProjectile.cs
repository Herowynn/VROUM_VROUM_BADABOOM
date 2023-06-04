using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissileLauncherProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _incrementalSpeed;
    private Rigidbody _targetRB;
    private Rigidbody _projectileRB;
    private Vector3 _direction;
    private float _stageOne = 1f;
    private float _stageTwo = 4f;
    private Vector3 _explosionDir;
    private float _clock;

    [Header("Audio")]
    public AudioClip MissileFlySound;
    public AudioClip[] MissileExplosionSounds;
    private AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _source.volume = 0.12f;
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

        
        _explosionDir = explodeDir;
    }

    /// <summary>
    /// This method manages the trajectory and the behaviour of the missile according to the missile course stage.
    /// </summary>
    private void Update()
    {
        _direction = transform.forward;
        _clock += Time.deltaTime;
        _speed += _incrementalSpeed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        _projectileRB.velocity = Vector3.zero;
       
            _projectileRB.AddForce(_direction*_speed, ForceMode.Acceleration);
           // _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _launchingSpeed);
        

        if (_clock > _stageOne)
        {
            if (_targetRB != null)
            {
                if((_targetRB.transform.position-transform.position).magnitude > 2)transform.LookAt(_targetRB.transform);
                _projectileRB.AddForce(_direction * _speed, ForceMode.Acceleration);
            }    
            else _projectileRB.AddForce(_direction * _speed, ForceMode.Acceleration);
        }

        //if (_clock > _stageTwo)
        //{
        //    _projectileRB.AddForce(_direction * _speed, ForceMode.Acceleration);
        //}

       
       
    }

    /// <summary>
    /// This method verifies that the object touched is a car controller and sets sound effects and car variables
    /// accordingly if it is the case.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Vector3 dir = other.transform.position - _explosionDir;

        if (other.gameObject.TryGetComponent<GlobalController>(out var carControl))
        {
            _source.clip = MissileExplosionSounds[Random.Range(0, MissileExplosionSounds.Length)];
            _source.loop = false;
            _source.Play();

            carControl.ExplosionDirection = dir;
            carControl.IsExploded = true;
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    IEnumerator WaitBeforeAutoDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
