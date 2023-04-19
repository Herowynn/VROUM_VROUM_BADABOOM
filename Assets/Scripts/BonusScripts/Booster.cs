using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

/// <summary>
/// This class is the generic class for the booster bonuses.
/// </summary>
public class Booster : MonoBehaviour
{
    [SerializeField] private float _durationAfterActivation;
    [SerializeField] private float _speedAdded;
    [SerializeField] private float _time;
    private float _timeIncrementation;

    [Header("Audio")]
    public AudioClip[] Sounds;
    private AudioSource _source;


    /// <summary>
    /// This method activate the sound effects and call the StartBoost method when the Boost is used.
    /// </summary>
    /// <param name="sphereRB"></param>
    /// <param name="car"></param>
    public void Boost(Rigidbody sphereRB, GameObject car)
    {
        StartCoroutine(StartBoost(sphereRB, car));
        _source = GetComponent<AudioSource>();
        _source.Play();
    }

    /// <summary>
    /// This coroutine add a force to the car in the forward direction during "_durationAfterActivation" seconds 
    /// and then destroy this booster object.
    /// </summary>
    /// <param name="sphereRB"></param>
    /// <param name="car"></param>
    /// <returns></returns>
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
