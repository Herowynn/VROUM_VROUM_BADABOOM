using System.Collections;
using UnityEngine;

/// <summary>
/// This class is the generic class for the booster bonuses.
/// </summary>
public class Booster : MonoBehaviour
{
    [SerializeField] private float _durationAfterActivation;
    [SerializeField] private float _speedAdded;
    [SerializeField] private float _addedSpeedFactorForAI;
    [SerializeField] private float _time;
    private float _timeIncrementation;
    private bool _hasBeenUsed = false;

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
        if (_hasBeenUsed == false) 
        { 
            StartCoroutine(StartBoost(sphereRB, car));
            _source = GetComponent<AudioSource>();
            _source.Play();
            _hasBeenUsed= true;
        }
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
            if (car.GetComponent<CarController>())
                sphereRB.AddForce(car.transform.forward * _speedAdded, ForceMode.VelocityChange);
            else if (car.GetComponent<AIController>())
                car.GetComponent<AIController>().Speed += _speedAdded * _addedSpeedFactorForAI;

            _timeIncrementation += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (car.GetComponent<AIController>())
        {
            car.GetComponent<AIController>().Speed = (GameManager.Instance.MultipleInputManager.AiDifficulty == AIDifficulty.Brutal) ?
                car.GetComponent<AIController>().BrutalDiffSpeed : car.GetComponent<AIController>().ClassicSpeed;
        }

        Destroy(gameObject);
    }
}
