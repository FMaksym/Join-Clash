using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttackManager : MonoBehaviour, ITargetable
{
    public bool Member;

    public PlayerManager PlayerManager { get; private set; }
    public BossHealthManager BossHealthManager { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Boss { get; private set; }

    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool Fight;
    
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

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        BossHealthManager = GameObject.FindObjectOfType<BossHealthManager>();
        Boss = GameObject.FindObjectOfType<BossAttackManager>().transform;
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        MinDistance = _minDistance;
        MaxDistance = _maxDistance;
    }

    private void Update()
    {
        if (BossHealthManager.BossIsAlive)
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

                if (_distanceToBoss.sqrMagnitude <= MinDistance * MinDistance && BossHealthManager.BossIsAlive)
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

    public void TakeDamage()
    {
        if (Random.Range(0,10) > 0)
        {
            BossHealthManager.GetDamage();
        }
    }

    public void MoveToTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.5f * Time.deltaTime);
        var rotation = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), 5f * Time.deltaTime);
        Animator.SetFloat("Run", 1f);
        Rigidbody.velocity = Vector3.zero;
    }

    public void FightWithTarget(Transform target)
    {
        var rotation = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), 5f * Time.deltaTime);
        Animator.SetBool("Fight", true);
    }

    public void ChangeAttack()
    {
        Animator.SetFloat("AttackMode", Random.Range(0, 2));
    }

    public void ChangeDance()
    {
        Animator.SetFloat("DanceMode", Random.Range(0, 2));
    }
}
