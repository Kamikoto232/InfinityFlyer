
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash")) Destroy(other.gameObject);
        if (other.CompareTag("Map")) Destroy(other.transform.parent.parent.gameObject);
    }
}