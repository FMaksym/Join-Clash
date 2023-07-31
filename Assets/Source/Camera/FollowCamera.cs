using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FollowCamera : MonoBehaviour
{
    private Camera _camera;
    private Transform _currentFollowObject;
    [Inject] private PlayerManager _playerManager;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        _currentFollowObject = _playerManager.RigidbodyList[0].transform;
        _camera.transform.position = new Vector3(_currentFollowObject.position.x, _camera.transform.position.y, _camera.transform.position.z);
    }

    private void LateUpdate()
    {
        if (_playerManager.RigidbodyList.Count > 0 && !_playerManager.Attack)
        {
            if (_playerManager.RigidbodyList[0].gameObject.activeSelf)
            {
                if (_currentFollowObject != _playerManager.RigidbodyList[0].transform)
                {
                    _currentFollowObject = _playerManager.RigidbodyList[0].transform;
                }
            }
        }
        _camera.transform.position = new Vector3(_currentFollowObject.position.x, _camera.transform.position.y, _camera.transform.position.z);
    }
}
