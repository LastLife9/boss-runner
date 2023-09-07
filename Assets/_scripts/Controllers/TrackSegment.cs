using UnityEngine;

public class TrackSegment : MonoBehaviour
{
    public bool _bossTrack = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_bossTrack)
            {
                EventBus.OnBossFightStart?.Invoke();
                return;
            }

            EventBus.OnTrackSegmentPassed?.Invoke();
        }
    }
}
