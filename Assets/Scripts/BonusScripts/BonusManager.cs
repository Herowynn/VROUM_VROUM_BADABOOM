using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

/// <summary>
/// This class manages the creation of random bonuses at random positions around the cars during the race.
/// </summary>
public class BonusManager : MonoBehaviour
{
    #region Public Fields

    [Header("Instaces")]
    public GameObject BoostPrefab;
    public GameObject BoostParent;
    public GameObject AttackPrefab;
    public GameObject AttackParent;

    #endregion

    #region Private Fields

    [Header("GD")]
    [SerializeField] private int _maxTimeBetweenBonusSpawn;
    [SerializeField] private int _minTimeBetweenBonusSpawn;
    [SerializeField] private float _firstBonusSpawnTime;

    private float _spawnTimer;
    private float _timeIncrementation;
    private List<GameObject> _allBonus;

    #endregion

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
