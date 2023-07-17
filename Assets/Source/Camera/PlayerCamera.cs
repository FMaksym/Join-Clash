using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cameraCinemachine;
    public PlayerManager PlayerManager { get; private set; }

    private void Awake()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (PlayerManager.RigidbodyList.Count > 0 && !PlayerManager.Attack)
        {
            if (PlayerManager.RigidbodyList[0].gameObject.activeSelf)
            {
                cameraCinemachine.Follow = PlayerManager.RigidbodyList[0].transform;
            }
        }
    }
}
