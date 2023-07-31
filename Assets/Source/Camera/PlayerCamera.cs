using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cameraCinemachine;
    public PlayerManager PlayerManager { get; private set; }
    private Transform _currentFollowObject;

    private void Awake()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        cameraCinemachine.Follow = _currentFollowObject = PlayerManager.RigidbodyList[0].transform;
    }

    private void FixedUpdate()
    {
        if (PlayerManager.RigidbodyList.Count > 0 && !PlayerManager.Attack)
        {
            if (PlayerManager.RigidbodyList[0].gameObject.activeSelf)
            {
                if (_currentFollowObject != PlayerManager.RigidbodyList[0].transform)
                {
                    _currentFollowObject = PlayerManager.RigidbodyList[0].transform;
                    cameraCinemachine.Follow = _currentFollowObject;
                    cameraCinemachine.transform.position = new Vector3(_currentFollowObject.position.x, cameraCinemachine.transform.position.y, cameraCinemachine.transform.position.z);
                }
            }
        }
        if (cameraCinemachine.transform.position.x != _currentFollowObject.position.x)
        {
            cameraCinemachine.transform.position = new Vector3(_currentFollowObject.position.x, cameraCinemachine.transform.position.y, cameraCinemachine.transform.position.z);
        }
    }
}
