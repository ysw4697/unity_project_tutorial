using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10.0f;
    
    bool _moveToDestination = false;
    Vector3 _destinationPosition;
    
    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    private void Update()
    {
        if (_moveToDestination)
        {
            Vector3 direction = _destinationPosition - transform.position;
            if (direction.magnitude < 0.0001f)
            {
                _moveToDestination = false;
            }
            else
            {
                float moveDistance = Mathf.Clamp(_moveSpeed * Time.deltaTime, 0.0f, direction.magnitude);
                transform.position += direction.normalized * moveDistance;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10.0f * Time.deltaTime);
            }
        }
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * (_moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * (_moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * (_moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * (_moveSpeed * Time.deltaTime);
        }
        
        _moveToDestination= false;
    }

    void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        if (mouseEvent != Define.MouseEvent.Click)
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destinationPosition = hit.point;
            _moveToDestination = true;
        }
    }
}
