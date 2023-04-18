using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip LoopSound;
    public AudioClip[] HornSounds;
    public float minTimeBetweenHorn = 60, maxTimeBetweenHorn = 180;
    public AudioSource Source;


    public float speed;

    private Vector3 _direction;

    public Vector3 direction { set { _direction = value; } }

    private void Start()
    {
        //Source = GetComponent<AudioSource>();
        //Source.clip = LoopSound;
        //Source.Play();
        //Source.loop = true;
        StartCoroutine(Horn(minTimeBetweenHorn, maxTimeBetweenHorn));
    }
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            transform.position += _direction.normalized * speed * Time.deltaTime;
            transform.forward = _direction;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
    public IEnumerator Horn(float min, float max)
    {
        yield return new WaitForSecondsRealtime(Random.Range(min, max));
        AudioClip horn = HornSounds[Random.Range(0, 2)];
        Source.loop = false;
        Source.clip = horn;
        Source.Play();
        //yield return new WaitForSecondsRealtime(1);
        //Source.clip = LoopSound;
        //Source.Play();
        //Source.loop = true;
        StartCoroutine(Horn(minTimeBetweenHorn, maxTimeBetweenHorn));
    }

}

