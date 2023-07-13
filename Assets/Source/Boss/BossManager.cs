using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossManager : Fighter
{
    public bool LockOnTarget;
    public bool BossIsAlive;

    [SerializeField] private int _health;
    [SerializeField] private float particleDuration;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _attackMode;
    [SerializeField] private List<Rigidbody> _enemyList = new List<Rigidbody>();
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnParticle;
    [SerializeField] private GameObject _deathParticle;
    [SerializeField] private Slider _healthbar;
    [SerializeField] private TMP_Text _healthbarAmount;
    public PlayerManager PlayerManager { get; private set;}
    public Boss Boss { get; private set;}

    private void Start()
    {
        _healthbar.value = _healthbar.maxValue = _health = 50;
        _healthbarAmount.text = _health.ToString();
        BossIsAlive = true;
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        Boss = GetComponent<Boss>();
        Animator = GetComponent<Animator>();
        Health = _health;
        MinDistance = _minDistance;
        MaxDistance = _maxDistance;
        SpawnParticle = _spawnParticle;
        DeathParticle = _deathParticle;
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

            }
        }

        if (PlayerManager.RigidbodyList.Count == 0)
        {
            Animator.SetBool("Fight", false);
            Animator.SetFloat("AttackMode", 5f);
        }
    }

    public override void GetDamage()
    {
        Health--;
        _healthbar.value = Health;
        _healthbarAmount.text = Health.ToString();
        Die();
    }

    public override void Die()
    {
        if (Health <= 0 && BossIsAlive)
        {
            Health = 0;
            GameObject particleEffect = Instantiate(DeathParticle, transform.position, transform.rotation);
            // ”ничтожаем эффект по окончании длительности
            Destroy(particleEffect, particleDuration);
            //Instantiate(DeathParticle, SpawnParticle.position, Quaternion.identity);
            //gameObject.SetActive(false);
            Destroy(gameObject);
            BossIsAlive = false;
            // ”станавливаем Rigidbody босса в null
            Rigidbody = null;
        }
    }

    public void ChangeBossAttack()
    {
        Animator.SetFloat("AttackMode", Random.Range(2, 4));
    }
}
