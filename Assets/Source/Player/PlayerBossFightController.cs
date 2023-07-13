using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerBossFightController : Fighter
{
    [SerializeField] private int _health;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float particleDuration;
    [SerializeField] private bool Fight;
    [SerializeField] public bool Member;
    [SerializeField] private CapsuleCollider _capsCollider;
    [SerializeField] private Transform _spawnParticle;
    [SerializeField] private GameObject _deathParticle;
    public PlayerManager PlayerManager { get; private set; }
    public BossManager BossManager { get; private set; }
    public Transform Boss { get; private set; }

    public static event Action OnPlayerWinEvent;

    [Inject] private EventManager eventManager;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        BossManager = GameObject.FindObjectOfType<BossManager>();
        Boss = GameObject.FindObjectOfType<BossManager>().transform;
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        Health = _health;
        MinDistance = _minDistance;
        MaxDistance = _maxDistance;
        SpawnParticle = _spawnParticle;
        DeathParticle = _deathParticle;

        OnPlayerWinEvent += PlayerWin;
    }

    private void Update()
    {
        if (BossManager.BossIsAlive)
        {
            if (Boss != null && Boss.gameObject.activeSelf)
            {
                var _distanceToBoss = Boss.position - transform.position;

                if (!Fight)
                {
                    if (_distanceToBoss.sqrMagnitude <= MaxDistance * MaxDistance)
                    {
                        PlayerManager.Attack = true;
                        PlayerManager.GameState = false;
                    }

                    if (PlayerManager.Attack && Member)
                    {
                        MoveToTarget(Boss);
                    }
                }

                if (_distanceToBoss.sqrMagnitude <= MinDistance * MinDistance && BossManager.BossIsAlive)
                {
                    Fight = true;

                    FightWithTarget(Boss);

                    MinDistance = MaxDistance;

                    Rigidbody.velocity = Vector3.zero;
                }

                else
                {
                    Fight = false;
                }
            }
        }
        else
        {
            if (!BossManager.BossIsAlive)
            {
                //EventManager.OnPlayerWin?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BossHummer>())
        {
            GetDamage();
        }

        if (other.gameObject.GetComponent<Obstacle>())
        {
            DieFromObstacle();
        }
    }

    public override void GetDamage()
    {
        Health--;
        Die();
    }

    public override void Die()
    {
        foreach (var player in PlayerManager.RigidbodyList)
        {
            if (Health <= 0 && player.gameObject.activeSelf)
            {
                GameObject particleEffect = Instantiate(DeathParticle, transform.position, transform.rotation);
                Destroy(particleEffect, particleDuration);

                if (gameObject.name != PlayerManager.RigidbodyList.ElementAt(0).name)
                {
                    gameObject.SetActive(false);
                    transform.parent = null;
                }
                else
                {
                    _capsCollider.enabled = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                }

                for (int i = 0; i < PlayerManager.RigidbodyList.Count; i++)
                {
                    if (PlayerManager.RigidbodyList.ElementAt(i).name == gameObject.name)
                    {
                        PlayerManager.RigidbodyList.RemoveAt(i);
                    }
                }
                Destroy(gameObject);
                BossManager.LockOnTarget = false;
                break;
            }
        }
    }

    private void DieFromObstacle()
    {
        GameObject particleEffect = Instantiate(DeathParticle, transform.position, transform.rotation);
        Destroy(particleEffect, particleDuration);

        gameObject.SetActive(false);
        transform.parent = null;

        for (int i = 0; i < PlayerManager.RigidbodyList.Count; i++)
        {
            if (PlayerManager.RigidbodyList.ElementAt(i).name == gameObject.name)
            {
                PlayerManager.RigidbodyList.RemoveAt(i);
            }
        }
    }

    public void ChangeAttack()
    {
        Animator.SetFloat("AttackMode", Random.Range(0, 2));
    }

    public void ChangeDance()
    {
        Animator.SetFloat("DanceMode", Random.Range(0, 2));
    }

    public void TakeDamage()
    {
        BossManager.GetDamage();
    }

    private void PlayerWin()
    {

    }
}
