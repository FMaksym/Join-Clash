using UnityEngine;
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

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        BossManager = GameObject.FindObjectOfType<BossManager>();
        Boss = GameObject.FindObjectOfType<BossManager>().transform;
        _capsCollider = GetComponent<CapsuleCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        Health = _health;
        MinDistance = _minDistance;
        MaxDistance = _maxDistance;
        SpawnParticle = _spawnParticle;
        DeathParticle = _deathParticle;
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
                        PlayerManager.IsGame = false;
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
                GameObject particleEffect = Instantiate(DeathParticle, new Vector3(transform.position.x, 0.33f, transform.position.z), transform.rotation, PlayerManager._road);
                Destroy(particleEffect, particleDuration);

                PlayerManager.EventManager.PlayerZeroHealthDeath(PlayerManager.RigidbodyList, BossManager._enemyList, _capsCollider, gameObject, transform);
                Rigidbody.velocity = Vector3.zero;
                break;
            }
        }
    }

    private void DieFromObstacle()
    {
        PlayerManager.EventManager.PlayerObstacleDeath(PlayerManager.RigidbodyList, BossManager._enemyList, _capsCollider, gameObject, transform);

        GameObject particleEffect = Instantiate(DeathParticle, new Vector3(transform.position.x, 0.33f, transform.position.z) , transform.rotation, PlayerManager._road);
        Destroy(particleEffect, particleDuration);
        Rigidbody.velocity = Vector3.zero;
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
        if (Random.Range(0,10) > 0)
        {
            BossManager.GetDamage();
        }
    }
}
