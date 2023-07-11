using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This class manages the creation of random bonuses at random positions around the cars during the race.
/// </summary>
public class BonusManager : MonoBehaviour
{
    [Header("Instances")] 
    [SerializeField] private GameObject _boostPrefab;
    [SerializeField] private Transform _boostContainer;
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private Transform _attackContainer;

    [Header("GD")]
    [SerializeField] private int _maxTimeBetweenBonusSpawn;
    [SerializeField] private int _minTimeBetweenBonusSpawn;
    [SerializeField] private float _firstBonusSpawnTime;

    // Intern var
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

    /// <summary>
    /// This method generates bonus of a random type, at a random position aroud a random player and adds the bonus
    /// to the bonuses list.
    /// </summary>
    private void SpawnBonus()
    {
        int rndBonusType = Random.Range(0, 2);
        GameObject firstPlayerGo = GameManager.Instance.PlayersManager.RankedPlayers[0];
        
        Vector3 spawnPosition = new(Random.Range(-5, 5), Random.Range(1, 2), Random.Range(-5, 5));
        
        spawnPosition += firstPlayerGo.transform.forward * 15;
        spawnPosition += firstPlayerGo.transform.position;

        switch (rndBonusType)
        {
            case 0:
                _allBonus.Add(Instantiate(_attackPrefab, spawnPosition, Quaternion.identity, _attackContainer));
                break;
            case 1:
                _allBonus.Add(Instantiate(_boostPrefab, spawnPosition, Quaternion.identity, _boostContainer));
                break;
        }
    }

    /// <summary>
    /// This methods destroys all the bonuses on the track.
    /// </summary>
    public void ClearBonus()
    {
        foreach (var bonus in _allBonus)
        {
            Destroy(bonus);
        }
        
        _allBonus.Clear();
    }
}
