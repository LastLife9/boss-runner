using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    private const string _trackTag = "Track";
    private ObjectPooling _pooling;
    private Queue<GameObject> _spawnedTracks = new Queue<GameObject>();
    private Vector3 _spawnPosition;

    [SerializeField] private float _trackOffset = 30;
    [SerializeField] private int _tracksForwardCount = 4;
    private int _keepTracksBehind = 3;

    private void OnEnable()
    {
        EventBus.OnTrackSegmentPassed += TrackProcessing;
    }

    private void OnDisable()
    {
        EventBus.OnTrackSegmentPassed -= TrackProcessing;
    }

    private void Start()
    {
        _pooling = ObjectPooling.Instance;
        _spawnPosition = Vector3.zero;

        for (int i = 0; i < _tracksForwardCount; i++)
            SpawnTrack();
    }

    private void TrackProcessing()
    {
        SpawnTrack();

        if(_spawnedTracks.Count >= _tracksForwardCount + _keepTracksBehind)
            RemoveOldTrack();
    }

    private void SpawnTrack()
    {
        _spawnedTracks.Enqueue(_pooling.SpawnFromPool(_trackTag, _spawnPosition, Quaternion.identity));
        _spawnPosition += Vector3.forward * _trackOffset;
    }

    private void RemoveOldTrack()
    {
        GameObject oldrack = _spawnedTracks.Dequeue();
        oldrack.SetActive(false);
    }
}
