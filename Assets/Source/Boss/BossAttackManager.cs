using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BossAttackManager : MonoBehaviour, ITargetable
{
    public bool LockOnTarget;
    public bool BossIsAlive;
    public List<Rigidbody> _enemyList = new List<Rigidbody>();

    public PlayerManager PlayerManager { get; private set; }
    public Boss Boss { get; private set; }
    public GameObject DeathParticle { get; protected set; }
    public Rigidbody Rigidbody { get; protected set; }
    public Animator Animator { get; protected set; }

    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _attackMode;
    [SerializeField] private Transform _target;

    [Inject] private EventManager eventManager;

    public float MinDistance
    {
        get { return _minDistance; }
        set { _minDistance = value; }
    }

    public float MaxDistance
    {
        get { return _maxDistance; }
        set { _maxDistance = value; }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        Rigidbody = GetComponent<Rigidbody>(); ;
        Boss = GetComponent<Boss>();
        Animator = GetComponent<Animator>();
        MinDistance = _minDistance;
        MaxDistance = _maxDistance;
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

            if (!enemy.gameObject.activeSelf)
            {
                _enemyList.Remove(enemy);
            }
        }
        if (PlayerManager.RigidbodyList.Count <= 0)
        {
            PlayerManager.IsGame = false;
            eventManager.PlayerLose(Animator);
        }
    }

    public void MoveToTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
    }

    public void FightWithTarget(Transform target)
    {
        var rotation = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), 5f * Time.deltaTime);
    }

    public void ChangeBossAttack()
    {
        Animator.SetFloat("AttackMode", Random.Range(2, 4));
    }
}
