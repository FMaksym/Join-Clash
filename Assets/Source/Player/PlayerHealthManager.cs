using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    public PlayerManager PlayerManager { get; private set; }
    public CapsuleCollider CapsuleCollider { get; private set; }
    public GameObject DeathParticle { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [SerializeField] private int _health;
    [SerializeField] private float particleDuration;
    [SerializeField] private GameObject _deathParticle;
    [SerializeField] private float _heightSpawnParticle = 0.33f;

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        DeathParticle = _deathParticle;
        Health = _health;
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

    public void GetDamage()
    {
        Health--;
        Die();
    }

    public void Die()
    {
        foreach (var player in PlayerManager.RigidbodyList)
        {
            if (Health <= 0 && player.gameObject.activeSelf)
            {
                CreateParticleEffect();
                PlayerManager.EventManager.PlayerZeroHealthDeath(PlayerManager.RigidbodyList, CapsuleCollider, gameObject, transform);
                Rigidbody.velocity = Vector3.zero;
                break;
            }
        }
    }

    public void DieFromObstacle()
    {
        CreateParticleEffect();
        PlayerManager.EventManager.PlayerObstacleDeath(PlayerManager.RigidbodyList, CapsuleCollider, gameObject, transform);
        Rigidbody.velocity = Vector3.zero;
    }

    private void CreateParticleEffect()
    {
        GameObject particleEffect = Instantiate(DeathParticle, new Vector3(transform.position.x, _heightSpawnParticle, transform.position.z), transform.rotation, PlayerManager._road);
        Destroy(particleEffect, particleDuration);
    }
}
