using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FinishObject : MonoBehaviour
{
    bool trig;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !trig)
        {
            SpawnManager.Finish();
            Destroy(gameObject);
            trig = true;
        }
    }
}