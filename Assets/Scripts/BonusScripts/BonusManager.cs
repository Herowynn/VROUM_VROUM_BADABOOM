using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [Header("Instaces")]
    public GameObject BoostPrefab;
    public GameObject BoostParent;
    public GameObject AttackPrefab;
    public GameObject AttackParent;
    
    [Header("GD")]
    [SerializeField] private int _maxTimeBetweenBonusSpawn;
    [SerializeField] private int _minTimeBetweenBonusSpawn;
    [SerializeField] private float _firstBonusSpawnTime;

    //intern var
    private float _spawnTimer;
    private float _timeIncrementation;
    private List<GameObject> _allBonus;

    void Start()
    {
        _spawnTimer = _firstBonusSpawnTime;
        _allBonus = new List<GameObject>();
    }

    void Update()
    {
        if(GameManager.Instance.GameState != GameState.RACING)
            return;

        _timeIncrementation += Time.deltaTime;

        if (_timeIncrementation >= _spawnTimer)
        {
            _timeIncrementation = 0;
            SpawnBonus();
            _spawnTimer = Random.Range(_minTimeBetweenBonusSpawn, _maxTimeBetweenBonusSpawn);
        }
    }

    private void SpawnBonus()
    {
        int randomX = Random.Range(-5, 5);
        int randomY = Random.Range(3, 10);
        int randomZ = Random.Range(-5, 5);
        int rndBonusType = Random.Range(0, 2);
        int rndPlayer = Random.Range(0, GameManager.Instance.PlayersManager.Players.Count);

        
        Vector3 spawnPosition = new(randomX, randomY, randomZ);
        spawnPosition += GameManager.Instance.PlayersManager.Players[rndPlayer].transform.forward * 3;
        spawnPosition += GameManager.Instance.PlayersManager.Players[rndPlayer].transform.position;

        switch (rndBonusType)
        {
            case 0:
                _allBonus.Add(Instantiate(AttackPrefab, spawnPosition, Quaternion.identity, AttackParent.transform));
                break;
            case 1:
                _allBonus.Add(Instantiate(BoostPrefab, spawnPosition, Quaternion.identity, BoostParent.transform));
                break;
        }
    }

    public void ClearBonus()
    {
        foreach (var bonus in _allBonus)
        {
            Destroy(bonus);
        }
        
        _allBonus.Clear();
    }
}
