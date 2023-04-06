using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterCoreGame : MonoBehaviour
{
    public static HarvesterCoreGame Instance;

    public GameObject checkpointsParentObject;
    public GameObject harvester;
    public int groundLayerNumber;


    private List<Transform> _checkpointsList = new List<Transform>();
    private int _nextCheckpointIndex;
    private Vector3 _direction;
    private Vector3 _realPointToReach;

    public int nextCheckpointIndex { get { return _nextCheckpointIndex; } set { _nextCheckpointIndex = value; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        for(int i = 0; i < checkpointsParentObject.transform.childCount; i++)
        {
            _checkpointsList.Add(checkpointsParentObject.transform.GetChild(i));
        }

        _nextCheckpointIndex = 0;
    }

    void Update()
    {
        _realPointToReach = new Vector3(_checkpointsList[_nextCheckpointIndex].position.x, harvester.transform.position.y, _checkpointsList[_nextCheckpointIndex].position.z);
        _direction = (_realPointToReach - harvester.transform.position).normalized;
        harvester.GetComponent<Harvester>().direction = _direction;
    }
}
