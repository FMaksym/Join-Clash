using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class BossManager : Fighter
{
    public bool LockOnTarget;
    public bool BossIsAlive;

    [SerializeField] private int _health;
    [SerializeField] private float particleDuration;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _attackMode;
    public List<Rigidbody> _enemyList = new List<Rigidbody>();
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnParticle;
    [SerializeField] private GameObject _deathParticle;
    [SerializeField] private Slider _healthbar;
    [SerializeField] private TMP_Text _healthbarAmount;

    public PlayerManager PlayerManager { get; private set;}
    public Boss Boss { get; private set;}
    [Inject] private EventManager eventManager;

    public new int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        _healthbar.value = _healthbar.maxValue = _health = 150;
        _healthbarAmount.text = _health.ToString();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        Boss = GetComponent<Boss>();
        Animator = GetComponent<Animator>();
        MinDistance = _minDistance;
        MaxDistance = _maxDistance;
        SpawnParticle = _spawnParticle;
        DeathParticle = _deathParticle;
        Rigidbody = _rigidbody;
        BossIsAlive = true;
    }

    private void Update()
    {
        _enemyList = PlayerManager.RigidbodyList;
        foreach (var enemy in _enemyList)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                var enemyDistance = enemy.transform.position - transform.position;
                if (enemyDistance.sqrMagnitude < MaxDistance * MaxDistance && !LockOnTarget)
                {
                    _target = enemy.transform;
                    Animator.SetBool("Fight", true);

                    transform.position = Vector3.MoveTowards(transform.position, _target.position, 1f * Time.deltaTime);
                }
                if (enemyDistance.sqrMagnitude < MinDistance * MinDistance)
                {
                    LockOnTarget = true;
                }
            }
            else
            {
                LockOnTarget = false;
                break;
            }

            if (LockOnTarget)
            {
                FightWithTarget(_target);
                Rigidbody.velocity = Vector3.zero;
            }
        }
        if (PlayerManager.RigidbodyList.Count <= 0)
        {
            PlayerManager.IsGame = false;
            eventManager.PlayerLose(Animator);
        }
    }

    public override void GetDamage()
    {
        if (Health > 0 && BossIsAlive)
        {
            _health--;
            Health -= 1;
            _healthbar.value = Health;
            _healthbarAmount.text = Health.ToString();
            Debug.Log("Boss Take Damage");
        }
        else if (Health <= 0 && BossIsAlive)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("Boss Die");
        Health = 0;
        BossIsAlive = false;
        GameObject particleEffect = Instantiate(DeathParticle, transform.position, transform.rotation);
        Destroy(particleEffect, particleDuration);
        gameObject.SetActive(false);
        Rigidbody = null;
    }

    public void ChangeBossAttack()
    {
        Animator.SetFloat("AttackMode", Random.Range(2, 4));
    }
}
