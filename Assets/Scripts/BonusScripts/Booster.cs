using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Booster : MonoBehaviour
{
    [SerializeField] private float _durationAfterActivation;
    [SerializeField] private float _speedAdded;
    [SerializeField] private float _time;
    private float _timeIncrementation;

    [Header("Audio")]
    public AudioClip[] Sounds;
    private AudioSource _source;

    public void Boost(Rigidbody sphereRB, GameObject car)
    {
        StartCoroutine(StartBoost(sphereRB, car));
        _source = GetComponent<AudioSource>();
        _source.Play();
    }

    IEnumerator StartBoost(Rigidbody sphereRB, GameObject car)
    {
        _timeIncrementation = 0;
        while (_timeIncrementation < _durationAfterActivation)
        {
            sphereRB.AddForce(car.transform.forward * _speedAdded, ForceMode.VelocityChange);
            _timeIncrementation += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
    }
}
