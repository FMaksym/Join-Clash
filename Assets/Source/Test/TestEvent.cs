using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestEvent : MonoBehaviour
{
    public EventManager eventManager;
    public PlayerManager player;
    public TMP_Text playerText;

    void Start()
    {
        // ������������� �� ������� OnHealthChanged
        EventManager.OnPlayerCount += HandleHealthChanged;
        HandleHealthChanged(player.RigidbodyList.Count);
    }

    public void HandleHealthChanged(int count)
    {
        // ��������� ����� ��������
        playerText.text = "Players: " + count;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerCount -= HandleHealthChanged;
    }
}
