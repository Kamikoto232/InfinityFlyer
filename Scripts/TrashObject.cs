using System.Collections;
using UnityEngine;

public class TrashObject : MonoBehaviour
{
    public float dmg, fMin, fMax, rMin, rMax;
    Rigidbody rb;
    bool collid, Flyed;

    private void OnEnable()
    {
        EventManager.PostStopGameHandler += DestroyIt;
    }

    private void OnDisable()
    {
        EventManager.PostStopGameHandler -= DestroyIt;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-(Vector3.forward * Random.Range(10 + fMin, 10 + fMax)));
        rb.AddTorque(new Vector3(Random.Range(rMin, rMax), Random.Range(rMin, rMax), Random.Range(rMin, rMax)));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") && collid.Equals(false))
        {
            EventManager.OnDamage(dmg);
            EventManager.OnCollisionTrash();
            ParticleSystem ps = Instantiate(EffectsManager.em.Boom, rb.position, Quaternion.identity).GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule shape = ps.shape;
            ParticleSystem.ForceOverLifetimeModule force = ps.forceOverLifetime;
            shape.rotation = rb.rotation.eulerAngles;
            shape.mesh = GetComponent<MeshFilter>().mesh;
            force.x = rb.velocity.x;
            force.y = rb.velocity.y;
            force.z = rb.velocity.z;
            Destroy(gameObject);
            collid = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !Flyed)
        {
            StartCoroutine(Timer());
            Flyed = true;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (collid) yield break;
        EventManager.OnNearFly();
    }

    void DestroyIt()
    {
        Destroy(gameObject);
    }
}