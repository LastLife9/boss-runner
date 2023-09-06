using UnityEngine;

public class MouseTouchInput : IInput
{
    private float _touchDelta;
    private float _touchStartPosition;
    private bool _isTouching = false;

    public float GetHorizontalInput()
    {
#if UNITY_EDITOR
        return Input.GetAxis("Horizontal");
#else
        // Handle input for mobile devices (touch)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _isTouching = true;
                    _touchStartPosition = touch.position.x;
                    break;

                case TouchPhase.Moved:
                    if (_isTouching)
                    {
                        _touchDelta = touch.position.x - _touchStartPosition;
                    }
                    break;

                case TouchPhase.Ended:
                    _isTouching = false;
                    _touchDelta = 0f;
                    break;
            }
        }
        else
        {
            _isTouching = false;
            _touchDelta = 0f;
        }

        return _touchDelta;
#endif
    }
}
