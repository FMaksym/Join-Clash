using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EventManager : MonoBehaviour
{ 
    public delegate void PlayerDeathHandler(List<Rigidbody> playerList, List<Rigidbody> list, CapsuleCollider capsuleCollider, GameObject gameObject, Transform transform); 
    public delegate void PlayerZeroHealthHandler(List<Rigidbody> playerList, List<Rigidbody> list, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform); 
    public delegate void BossDeathHandler(bool IsAlive); 
    public delegate void PlayerWinHandler(List<Rigidbody> list); 
    public delegate void PlayerLoseHandler(Animator animator);
    public delegate void PlayerCountHandler(int count);

    public static event PlayerDeathHandler OnPlayerDeath;
    public static event PlayerZeroHealthHandler OnPlayerZeroHealthDeath;
    public static event BossDeathHandler OnBossDeath;
    public static event PlayerWinHandler OnPlayerWin;
    public static event PlayerLoseHandler OnPlayerLose;
    public static event PlayerCountHandler OnPlayerCount;
    public PlayerManager PlayerManager { get; private set; }
    public BossManager BossManager { get; private set; }

    private void Start()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        BossManager = GameObject.FindObjectOfType<BossManager>();
    }

    private void HandlePlayerZeroHealthDeath(List<Rigidbody> playerList, List<Rigidbody> list, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        if (player.name != PlayerManager.RigidbodyList.ElementAt(0).name)
        {
            player.SetActive(false);
            playerTransform.transform.parent = null;
        }
        else
        {
            capsuleCollider.enabled = false;
            playerTransform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < PlayerManager.RigidbodyList.Count; i++)
        {
            if (PlayerManager.RigidbodyList.ElementAt(i).name == player.name)
            {
                PlayerManager.RigidbodyList.RemoveAt(i);
            }
        }
        BossManager.LockOnTarget = false;
    }
    
    private void HandlePlayerObstacleDeath(List<Rigidbody> playerList, List<Rigidbody> list, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        if (player.name != playerList.ElementAt(0).name)
        {
            player.SetActive(false);
            playerTransform.parent = null;
        }
        else
        {
            capsuleCollider.enabled = false;
            playerTransform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list.ElementAt(i).name == player.name)
            {
                list.RemoveAt(i);
                break;
            }
        }
    }

    private void HandleBossDeath(bool IsAlive)
    {
        IsAlive = false;
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

    public void PlayerObstacleDeath(List<Rigidbody> playerList, List<Rigidbody> list, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        OnPlayerDeath?.Invoke(playerList, list, capsuleCollider, player, playerTransform);
    }
    
    public void PlayerZeroHealthDeath(List<Rigidbody> playerList, List<Rigidbody> list, CapsuleCollider capsuleCollider, GameObject player, Transform playerTransform)
    {
        OnPlayerZeroHealthDeath?.Invoke(playerList, list, capsuleCollider, player, playerTransform);
    }
    
    public void PlayerLose(Animator animator)
    {
        OnPlayerLose?.Invoke(animator);
    }
    
    public void PlayerWin(List<Rigidbody> list)
    {
        OnPlayerWin?.Invoke(list);
    }
    
    public void BossDie(bool IsAlive)
    {
        OnBossDeath?.Invoke(IsAlive);
    }

    private void OnEnable()
    {
        OnPlayerDeath += HandlePlayerObstacleDeath;
        OnPlayerZeroHealthDeath += HandlePlayerZeroHealthDeath;
        OnBossDeath += HandleBossDeath;
        OnPlayerLose += HandlePlayerLose;
        OnPlayerWin += HandlePlayerWin;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= HandlePlayerObstacleDeath;
        OnPlayerZeroHealthDeath -= HandlePlayerZeroHealthDeath;
        OnBossDeath -= HandleBossDeath;
        OnPlayerLose -= HandlePlayerLose;
        OnPlayerWin -= HandlePlayerWin;
    }
}
