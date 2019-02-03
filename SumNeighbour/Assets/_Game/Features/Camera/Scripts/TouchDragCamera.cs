using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class TouchDragCamera : MonoBehaviour
{
    private Vector3 _touchStart;
    [SerializeField] private Camera _camera;
    private float _groundZ = 0;

    private float _minX, _maxX, _minY, _maxY;

    public void SetBounds(Vector3 bottomLeft, Vector3 topRight)
    {
        _minX = bottomLeft.x;
        _minY = bottomLeft.y;
        _maxX = topRight.x;
        _maxY = topRight.y;
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) && Input.touchCount < 2){
            _touchStart = GetWorldPosition(_groundZ);
        }
        
        if (Input.GetMouseButton(0) && Input.touchCount < 2){
            Vector3 delta = _touchStart - GetWorldPosition(_groundZ);
            _camera.transform.position += delta;
            
            _camera.transform.position = new Vector3(
                Mathf.Clamp(_camera.transform.position.x, _minX, _maxX),
                Mathf.Clamp(_camera.transform.position.y, _minY, _maxY),
                _camera.transform.position.z);
        }
    }
    
    private Vector3 GetWorldPosition(float z){
        Ray mousePos = _camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0,0,z));
        ground.Raycast(mousePos, out float distance);
        return mousePos.GetPoint(distance);
    }
}