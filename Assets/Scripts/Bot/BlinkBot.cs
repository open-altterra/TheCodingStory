using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBot : MonoBehaviour
{
    [SerializeField]
    private Texture2D defaultFace, blink1, blink2;

    [SerializeField]
    private Material faceMaterial;

    private void Awake()
    {
        faceMaterial.mainTexture = defaultFace;
    }

    private System.Random random = new System.Random();
    private Coroutine currentCoroutine;
    private void Update()
    {
        if(random.Next(0, 300) == 10)
        {
            if(currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(BlinkAnimation());
            }
        }
    }

    private IEnumerator BlinkAnimation()
    {
        faceMaterial.mainTexture = blink1;
        yield return new WaitForSeconds(0.05f);
        faceMaterial.mainTexture = blink2;
        yield return new WaitForSeconds(0.05f + random.Next(0, 300) / 1000f);
        faceMaterial.mainTexture = blink1;
        yield return new WaitForSeconds(0.05f);
        faceMaterial.mainTexture = defaultFace;

        currentCoroutine = null;
    }
}
