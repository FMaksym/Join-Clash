using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EventManager : MonoBehaviour
{ 
    public delegate void PlayerDeathHandler(List<Rigidbody> playerList, CapsuleCollider capsuleCollider, GameObject gameObject, Transform transform); 
    public delegate void PlayerZeroHealthHandler(List<Rigidbody> playerList, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform); 
    public delegate void PlayerWinHandler(List<Rigidbody> list); 
    public delegate void PlayerLoseHandler(Animator animator);
    public delegate void PlayerCountHandler(int count);

    public static event PlayerDeathHandler OnPlayerDeath;
    public static event PlayerZeroHealthHandler OnPlayerZeroHealthDeath;
    public static event PlayerWinHandler OnPlayerWin;
    public static event PlayerLoseHandler OnPlayerLose;
    public static event PlayerCountHandler OnPlayerCount;
    public PlayerManager PlayerManager { get; private set; }
    public BossAttackManager BossManager { get; private set; }

    private void Start()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        BossManager = GameObject.FindObjectOfType<BossAttackManager>();
    }

    private void HandlePlayerZeroHealthDeath(List<Rigidbody> playerList, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        player.SetActive(false);
        playerTransform.transform.parent = null;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList.ElementAt(i).name == player.name || playerList.ElementAt(i).gameObject.activeSelf == false)
            {
                playerList.RemoveAt(i);
            }
        }
        BossManager.LockOnTarget = false;
    }
    
    private void HandlePlayerObstacleDeath(List<Rigidbody> playerList, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        player.SetActive(false);
        playerTransform.parent = null;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList.ElementAt(i).name == player.name || playerList.ElementAt(i).gameObject.activeSelf == false)
            {
                playerList.RemoveAt(i);
                break;
            }
        }
    }

    private void HandlePlayerWin(List<Rigidbody> list)
    {
        foreach (var _playerRigidbody in list)
        {
            _playerRigidbody.GetComponent<Animator>().SetBool("Fight", false);
            _playerRigidbody.GetComponent<Animator>().SetBool("Win", true);
        }
        Invoke("ReloadScene", 5f);
    }

    private void HandlePlayerLose(Animator animator)
    {
        animator.SetBool("Fight", true);
        animator.SetFloat("AttackMode", 5f);
        Invoke("ReloadScene", 5f);
        if (Application.isMobilePlatform)
        {
            Handheld.Vibrate();
        }
    }

    private void ReloadScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }

    public void PlayerObstacleDeath(List<Rigidbody> playerList, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        OnPlayerDeath?.Invoke(playerList, capsuleCollider, player, playerTransform);
    }
    
    public void PlayerZeroHealthDeath(List<Rigidbody> playerList, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        OnPlayerZeroHealthDeath?.Invoke(playerList, capsuleCollider, player, playerTransform);
    }
    
    public void PlayerLose(Animator animator)
    {
        OnPlayerLose?.Invoke(animator);
    }
    
    public void PlayerWin(List<Rigidbody> list)
    {
        OnPlayerWin?.Invoke(list);
    }

    private void OnEnable()
    {
        OnPlayerDeath += HandlePlayerObstacleDeath;
        OnPlayerZeroHealthDeath += HandlePlayerZeroHealthDeath;
        OnPlayerLose += HandlePlayerLose;
        OnPlayerWin += HandlePlayerWin;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= HandlePlayerObstacleDeath;
        OnPlayerZeroHealthDeath -= HandlePlayerZeroHealthDeath;
        OnPlayerLose -= HandlePlayerLose;
        OnPlayerWin -= HandlePlayerWin;
    }
}
