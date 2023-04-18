using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Harvester : MonoBehaviour
{
    [HideInInspector] public Transform[] NodesToFollow;
    
    [Header("GD")]
    public float Speed;

    [Header("Audio")]
    public AudioClip LoopSound;
    public AudioClip[] HornSounds;
    public float minTimeBetweenHorn = 20, maxTimeBetweenHorn = 30;
    private AudioSource _source;

    private Vector3 _direction;
    private int _targetNode;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(Horn(minTimeBetweenHorn, maxTimeBetweenHorn));
        AudioClip horn = HornSounds[Random.Range(0, HornSounds.Length)];
        if (horn != null)
        {
            _source.loop = false;
            _source.clip = horn;
            _source.Play();
        }
    }
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            UpdateMove(NodesToFollow);
        }
    }

    public void ResetToTransform(Transform resetTransform)
    {
        transform.position = resetTransform.position;
        transform.rotation = resetTransform.rotation;
        
        UpdateTargetNodeAfterReset();
    }

    private void UpdateTargetNodeAfterReset()
    {
        int newTargetNode = 0;
        
        float distance = float.MaxValue;
        
        for (int i = 0; i < NodesToFollow.Length; i++)
        {
            if (distance > Vector3.Distance(NodesToFollow[i].transform.position, transform.position))
            {
                newTargetNode = i;
                distance = Vector3.Distance(NodesToFollow[i].transform.position, transform.position);
            }
        }

        _targetNode = newTargetNode;
    }

    public void InitiateNodesToFollow(Transform[] nodes)
    {
        NodesToFollow = nodes;
        _targetNode = 0;
    }

    private void UpdateMove(Transform[] path)
    {
        Vector3 target = path[_targetNode].transform.position;
        Vector3 direction = target - transform.position;
        float moveStep = Speed * Time.deltaTime;
        float distance = Vector3.Distance(target, transform.position);

        //transform.rotation = path[_targetNode].transform.rotation;

        while (moveStep > distance)
        {
            _targetNode++;

            if (_targetNode >= path.Length)
                _targetNode = 0;

            target = path[_targetNode].transform.position;
            moveStep = Speed * Time.deltaTime;
            distance = Vector3.Distance(target, transform.position);
            direction = target - transform.position;
            
            //orientation
            transform.rotation = path[_targetNode].transform.rotation;
        }
        
        direction.Normalize();
        transform.position += moveStep * direction;
    }


    public IEnumerator Horn(float min, float max)
    {
        yield return new WaitForSecondsRealtime(Random.Range(min, max));
        AudioClip horn = HornSounds[Random.Range(0, 2)];
        _source.loop = false;
        _source.clip = horn;
        _source.Play();
        //yield return new WaitForSecondsRealtime(1); 
        //Source.clip = LoopSound; 
        //Source.Play(); 
        //Source.loop = true; 
        StartCoroutine(Horn(minTimeBetweenHorn, maxTimeBetweenHorn));
    }
}
