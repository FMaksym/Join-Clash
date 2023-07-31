using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthManager : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health;
    [SerializeField] private float particleDuration;
    [SerializeField] private GameObject _deathParticle;
    [SerializeField] private Slider _healthbar;
    [SerializeField] private TMP_Text _healthbarAmount;

    public bool BossIsAlive { get; private set; }

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _healthbar.value = _healthbar.maxValue = _health;
        _healthbarAmount.text = _health.ToString();
        BossIsAlive = true;
    }

    public void GetDamage()
    {
        if (Health > 0 && BossIsAlive)
        {
            Health -= 1;
            _healthbar.value = Health;
            _healthbarAmount.text = Health.ToString();
        }
        else if (Health <= 0 && BossIsAlive)
        {
            Die();
        }
    }

    public void Die()
    {
        Health = 0;
        BossIsAlive = false;
        GameObject particleEffect = Instantiate(_deathParticle, transform.position, transform.rotation);
        Destroy(particleEffect, particleDuration);
        gameObject.SetActive(false);
    }
}
