using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Literally copy pasted from a unity learn tutorial because mad skills
/// </summary>

namespace SumNeighbours
{
    [RequireComponent(typeof(Camera))]
    public class PinchZoom : MonoBehaviour
    {
        public float perspectiveZoomSpeed = 0.3f;        // The rate of change of the field of view in perspective mode.
        public float orthoZoomSpeed = 0.05f;        // The rate of change of the orthographic size in orthographic mode.
        public float minFov;
        public float maxFov;
        [SerializeField] private Camera _camera;

        private void Reset()
        {
            _camera = GetComponent<Camera>();
        }
        void Update()
        {
            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                if (_camera.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    _camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 5f, 1f);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    var fieldOfView = _camera.fieldOfView;
                    fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                    _camera.fieldOfView = fieldOfView;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    _camera.fieldOfView = Mathf.Clamp(fieldOfView, minFov, maxFov);
                }
            }
        }
    }
}