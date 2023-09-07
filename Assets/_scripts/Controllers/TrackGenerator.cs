using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    private const string _trackTag = "Track";
    private ObjectPooling _pooling;
    private Queue<GameObject> _spawnedTracks = new Queue<GameObject>();
    private Vector3 _trackSpawnPosition;
    private Vector3 _enemySpawnPosition;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private GameObject _bossTrack;
    [SerializeField] private float _trackOffset = 30;
    [SerializeField] private int _tracksForwardCount = 4;
    private float _timeToSpawnBoss;
    private float _enemySpawnAmplitude = 2.2f;
    private float _enemySpawnDelay;
    private int _keepTracksBehind = 3;
    private bool _gameStarted = false;
    private bool _spawning = true;
    private bool _bossSpawned = false;

    private void OnEnable()
    {
        EventBus.OnTrackSegmentPassed += TrackProcessing;
        EventBus.OnGameStart += () => { _gameStarted = true; };
    }

    private void OnDisable()
    {
        EventBus.OnTrackSegmentPassed -= TrackProcessing;
        EventBus.OnGameStart -= () => { _gameStarted = true; };
    }

    private void Start()
    {
        _pooling = ObjectPooling.Instance;
        _trackSpawnPosition = Vector3.zero;
        _timeToSpawnBoss = GameConfig.Instance.GameParameters.timeToSpawnBoss;
        _enemySpawnDelay = GameConfig.Instance.GameParameters.delayBetweenEnemies;

        for (int i = 0; i < _tracksForwardCount; i++)
            SpawnTrack();
    }

    private void Update()
    {
        if (!_gameStarted) return;

        if(_timeToSpawnBoss <= 0)
        {
            _spawning = false;
            _timeToSpawnBoss = GameConfig.Instance.GameParameters.timeToSpawnBoss;
        }

        _timeToSpawnBoss -= Time.deltaTime;
    }

    private void TrackProcessing()
    {
        if (_spawning)
        {
            SpawnTrack();

            if (_spawnedTracks.Count >= _tracksForwardCount + _keepTracksBehind)
                RemoveOldTrack();
        }
        else
        {
            if (_bossSpawned) return;
            SpawnBoss();
        }
    }

    private void SpawnTrack()
    {
        _spawnedTracks.Enqueue(_pooling.SpawnFromPool(_trackTag, _trackSpawnPosition, Quaternion.identity));

        if(_spawnedTracks.Count >= _keepTracksBehind)
        {
            float enemyCount = _trackOffset / _enemySpawnDelay;
            for (int i = 0; i < enemyCount; i++)
            {
                _enemySpawnPosition = _trackSpawnPosition + Vector3.right * Random.Range(-_enemySpawnAmplitude, _enemySpawnAmplitude);
                _enemySpawnPosition += i * Vector3.forward * GameConfig.Instance.GameParameters.delayBetweenEnemies;
                Instantiate(_enemyPrefab, _enemySpawnPosition, _enemyPrefab.transform.rotation);
            }
        }

        _trackSpawnPosition += Vector3.forward * _trackOffset;
    }

    private void SpawnBoss()
    {
        Instantiate(_bossTrack, _trackSpawnPosition, Quaternion.identity);
        _bossSpawned = true;
        EventBus.OnBossSpawn?.Invoke();
    }

    private void RemoveOldTrack()
    {
        GameObject oldrack = _spawnedTracks.Dequeue();
        oldrack.SetActive(false);
    }
}
