using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public int Health { get; protected set; }
    public float MinDistance { get; protected set; }
    public float MaxDistance { get; protected set; }
    public Transform SpawnParticle { get; protected set; }
    public GameObject DeathParticle { get; protected set; }
    public Rigidbody Rigidbody { get; protected set; }
    public Animator Animator { get; protected set; }

    public abstract void GetDamage();
    public abstract void Die();

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
}
