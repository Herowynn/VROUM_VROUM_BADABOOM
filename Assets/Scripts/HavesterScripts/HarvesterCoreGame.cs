using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterCoreGame : MonoBehaviour
{
    public static HarvesterCoreGame Instance;

    public GameObject CheckPointsParentObject;
    public GameObject Harvester;
    public int CarLayerNumber;
    public int BonusLayerNumber;


    private List<Transform> _checkPointsList = new List<Transform>();
    private int _nextCheckPointIndex;
    private Vector3 _direction;
    private Vector3 _realPointToReach;

    public int NextCheckpointIndex { get { return _nextCheckPointIndex; } set { _nextCheckPointIndex = value; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        for(int i = 0; i < CheckPointsParentObject.transform.childCount; i++)
        {
            _checkPointsList.Add(CheckPointsParentObject.transform.GetChild(i));
        }

        _nextCheckPointIndex = 0;
    }
        
    void Update()
    {
        if (_nextCheckPointIndex >= CheckPointsParentObject.transform.GetChildCount())
            _nextCheckPointIndex = 0;

        _realPointToReach = new Vector3(_checkPointsList[_nextCheckPointIndex].position.x, Harvester.transform.position.y, _checkPointsList[_nextCheckPointIndex].position.z);
        _direction = (_realPointToReach - Harvester.transform.position).normalized;
        Harvester.GetComponent<Harvester>().direction = _direction;
    }
}
