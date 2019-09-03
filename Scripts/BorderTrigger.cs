using System.Collections;
using UnityEngine;
public class BorderTrigger : MonoBehaviour
{
    public LayerMask mask;
    Coroutine c;
    bool active;

    private void Start()
    {
        EventManager.PostStartGameHandler += () => active = true;
        EventManager.StopGameHandler += () => { active = false; if(c!= null) StopCoroutine(c); //CameraManager.cm.anim.Play("BorderExit");
        };
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerManager.pm.Dead) return;
        if (!active) return;
        if (other.gameObject.layer == 9)
        {
           // CameraManager.cm.anim.Play("BorderEnter");
            if (c != null) StopCoroutine(c);
                c = StartCoroutine(Damage());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerManager.pm.Dead) return;
        if (!active) return;

        if (other.gameObject.layer == 9)
        {
            //CameraManager.cm.anim.Play("BorderExit");
            if (c != null) StopCoroutine(c);
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1);
        while (active)
        {
            EventManager.OnDamage(11);

            yield return new WaitForSeconds(1f);
        }
    }
}