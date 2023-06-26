using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Harvester : MonoBehaviour
{
    [HideInInspector] public List<Transform> NodesToFollow;
    [HideInInspector] public int TargetNode;

    [Header("GD")]
    public float InitialSpeed;
    public float IncrementSpeed = 0.01f;
    public float MaximumDelayBeforeMoving;

    [Header("Audio")]
    public AudioClip LoopSound;
    public AudioClip[] HornSounds;
    public float minTimeBetweenHorn = 20;
    public float maxTimeBetweenHorn = 30;
    public AudioSource Source;


    private float _speed;
    private float _carsAverageSpeed;
    private bool _roundBeginning;
    private bool _canMove;
    private List<GameObject> _visibleParts = new List<GameObject>();

    public bool CanMove { get { return _canMove; } }


    /// <summary>
    /// This method plays the horn sound effect and starts the Horn() coroutine.
    /// </summary>
    private void Start()
    {
        _speed = InitialSpeed;
        Source = GetComponent<AudioSource>();
        StartCoroutine(Horn(minTimeBetweenHorn, maxTimeBetweenHorn));
        AudioClip horn = HornSounds[Random.Range(0, HornSounds.Length)];
        if (horn != null)
        {
            Source.loop = false;
            Source.clip = horn;
            Source.Play();
        }

        _canMove = false;
        _roundBeginning = true;

        GameObject body = transform.GetChild(0).GetChild(0).gameObject;
        _visibleParts.Add(body);
        for (int i = 0; i < body.transform.childCount; i++)
            _visibleParts.Add(body.transform.GetChild(i).gameObject);

        StartCoroutine(WaitBeforeMoving());
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && _roundBeginning)
        {
            int visibleParts = 0;

            foreach(GameObject visiblePart in _visibleParts)
            {
                if (visiblePart.GetComponent<Renderer>().isVisible)
                    visibleParts++;
            }

            if (visibleParts == 0)
            {
                _canMove = true;
                _roundBeginning = false;
            }
        }

        _carsAverageSpeed = GameManager.Instance.PlayersManager.WeightedCarsSpeed;

        if (GameManager.Instance.GameState == GameState.RACING && _canMove)
        {
            UpdateMove(NodesToFollow);
            _speed += IncrementSpeed * Time.deltaTime;
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(MaximumDelayBeforeMoving + GameManager.Instance.RoundManager.TimeToRestartRound);
        _canMove = true;
    }

    public void InitiateNodesToFollow(List<Transform> nodes)
    {
        NodesToFollow = nodes;
        TargetNode = 0;
    }

    public void ResetToTransform(Transform resetTransform)
    {
        transform.position = resetTransform.position;
        transform.rotation = resetTransform.rotation;
        _speed = InitialSpeed;
        _roundBeginning = true;
        _canMove = false;
        UpdateTargetNodeAfterReset();
        StartCoroutine(WaitBeforeMoving());
    }

    /// <summary>
    /// This method gets the closest node in front of the harvester and sets the next node index to its value.
    /// It is used after all positions have been reset when one player or less are still alive.
    /// </summary>
    private void UpdateTargetNodeAfterReset()
    {
        int newTargetNode = 0;

        float distance = float.MaxValue;

        for (int i = 0; i < NodesToFollow.Count; i++)
        {
            if (distance > Vector3.Distance(NodesToFollow[i].transform.position, transform.position) && Mathf.Rad2Deg *
                Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.forward.normalized, (NodesToFollow[i].transform.position - transform.position).normalized))) < 90f)
            {
                newTargetNode = i;
                distance = Vector3.Distance(NodesToFollow[i].transform.position, transform.position);
            }
        }

        TargetNode = newTargetNode;
    }

    /// <summary>
    /// This method calculates the next node to reach "target", the direction "direction" and the current distance between
    /// the harvester and the next node to reach "distance".
    /// It increases the target node index if the distance towards the next node is inferior to the harvester step "moveStep".
    /// It resets the target node index if the index is superior to the nodes list length.
    /// Finally, it increments the harvester position with the "moveStep" distance.
    /// </summary>
    /// <param name="path"></param>
    private void UpdateMove(List<Transform> path)
    {
        Vector3 target = path[TargetNode].transform.position;
        Vector3 direction = target - transform.position;
        float moveStep = (_speed > _carsAverageSpeed ? _speed : _carsAverageSpeed) * Time.deltaTime;
        float distance = Vector3.Distance(target, transform.position);

        while (moveStep > distance)
        {
            TargetNode++;

            if (TargetNode >= path.Count)
                TargetNode = 0;

            target = path[TargetNode].transform.position;
            moveStep = (_speed > _carsAverageSpeed ? _speed : _carsAverageSpeed) * Time.deltaTime;
            distance = Vector3.Distance(target, transform.position);
            direction = target - transform.position;
            
            //orientation
            transform.rotation = path[TargetNode].transform.rotation;
        }

        direction.Normalize();
        transform.position += moveStep * direction;
    }

    /// <summary>
    /// This coroutine plays a random horn sound effect every random amount of time bewteen min seconds and max seconds.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public IEnumerator Horn(float min, float max)
    {
        yield return new WaitForSecondsRealtime(Random.Range(min, max));
        AudioClip horn = HornSounds[Random.Range(0, 2)];
        Source.loop = false;
        Source.clip = horn;
        Source.Play();
        StartCoroutine(Horn(minTimeBetweenHorn, maxTimeBetweenHorn));
    }
}
