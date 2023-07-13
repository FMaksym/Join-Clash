using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{ 
    public delegate void PlayerDeathHandler(); 
    public delegate void BossDeathHandler(); 
    public delegate void PlayerWinHandler(); 
    public delegate void PlayerLoseHandler();

    public static event PlayerDeathHandler OnPlayerDeath;
    public static event BossDeathHandler OnBossDeath;
    public static event PlayerWinHandler OnPlayerWin;
    public static event PlayerLoseHandler OnPlayerLose;
    public PlayerManager PlayerManager { get; private set; }
    public BossManager BossManager { get; private set; }

    private void Start()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        BossManager = GameObject.FindObjectOfType<BossManager>();
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died");
    }

    private void HandleBossDeath()
    {
        Debug.Log("Boss died");
        BossManager.BossIsAlive = false;
    }

    private void HandlePlayerWin()
    {
        Debug.Log("Player won");
        PlayerManager.SetBoolParametres(PlayerManager.RigidbodyList, "Fight", false);
        PlayerManager.SetBoolParametres(PlayerManager.RigidbodyList, "Win", true);
        Invoke("ReloadScene", 3f);
    }

    private void HandlePlayerLose()
    {
        Debug.Log("Player lost");
        Invoke("ReloadScene", 3f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        OnPlayerDeath += HandlePlayerDeath;
        OnBossDeath += HandleBossDeath;
        OnPlayerWin += HandlePlayerWin;
        OnPlayerLose += HandlePlayerLose;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= HandlePlayerDeath;
        OnBossDeath -= HandleBossDeath;
        OnPlayerWin -= HandlePlayerWin;
        OnPlayerLose -= HandlePlayerLose;
    }
}
