using System.Collections;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public Material Lighted, LightedSecond;
    float h, bh, s, v, pulse;
    public Light Dir;
    Color temp, startClr;
    public Color color;
    public float mnoj, trig, collided;
    Coroutine bang;

    private void OnEnable()
    {
        startClr = Dir.color;
        EventManager.ChangeColorHandler += ChangeColor;
        EventManager.MusicPulseHandler += (v) => { if (v < trig) pulse = 0; else pulse = v * mnoj; };
        EventManager.PostStopGameHandler += () => color = startClr;
        EventManager.CollisionTrashHandler += () => { if (bang != null) StopCoroutine(bang); bang = StartCoroutine(Bang()); };
        StartCoroutine(ChangeClr());
    }

    private void OnDisable()
    {
        EventManager.ChangeColorHandler -= ChangeColor;
    }

    void SetColor(Color color)
    {
        Dir.color = color;

        Lighted.SetColor("_Color", color);
        LightedSecond.SetColor("_Color", Color.HSVToRGB(h + 0.5f, 0.7f , 2, false));
    }

    public void ChangeColor(Color clr)
    {
        color = clr;
        //if (chclr != null) StopCoroutine(chclr);
        //chclr = StartCoroutine(ChangeClr(clr));
    }

    IEnumerator ChangeClr()
    {
        //float t = 6f;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (true)
        {
            Color.RGBToHSV(color, out h, out s, out v);

            temp = Color.Lerp(Dir.color, Color.HSVToRGB(h, s - (pulse - collided)/ 2, v + pulse + collided, false), 5f * Time.deltaTime);
            SetColor(temp);
            //t--;
            yield return wait;
        }
    }

    IEnumerator Bang()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.2f);
        Color temp = Dir.color;
        collided = 5;
        ChangeColor(Color.red);
        yield return wait;
        collided = 0;
        ChangeColor(temp);
    }
}