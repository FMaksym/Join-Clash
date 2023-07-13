using UnityEngine;
using Zenject;

public class Recruitment : MonoBehaviour
{
    public PlayerManager PlayerManager { get; private set; }

    private void Start()
    {
        PlayerManager = GetComponentInParent<PlayerManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<PlayerToAdd>())
        {
            Destroy(other.collider.GetComponent<PlayerToAdd>());
            PlayerManager.RigidbodyList.Add(other.collider.GetComponent<Rigidbody>());

            other.gameObject.GetComponent<PlayerBossFightController>().Member = true;

            other.collider.transform.parent = null;
            other.collider.transform.parent = PlayerManager.transform;

            if (!other.collider.GetComponent<Recruitment>())
            {
                other.collider.gameObject.AddComponent<Recruitment>();
            }
        }
    }
}
