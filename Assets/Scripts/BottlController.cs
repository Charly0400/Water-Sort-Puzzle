using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlController : MonoBehaviour
{
    public Color[] bottleColors;
    public SpriteRenderer botlleMaskSR;

    public float timetoRotate = 1.0f;

    public AnimationCurve scaleAndRotationMult;
    public AnimationCurve fillAmountCurve;
    public AnimationCurve rotationSpeedMult;

    void Start()
    {
        UpdateColorOnShader();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StartCoroutine(RotateBottle());
        }
    }


    void UpdateColorOnShader()
    {
        botlleMaskSR.material.SetColor("_C1", bottleColors[0]);
        botlleMaskSR.material.SetColor("_C2", bottleColors[1]);
        botlleMaskSR.material.SetColor("_C3", bottleColors[2]);
        botlleMaskSR.material.SetColor("_C4", bottleColors[3]);
    }
    //< >
    IEnumerator RotateBottle()
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        while (t < timetoRotate)
        {
            lerpValue = t/timetoRotate;
            angleValue = Mathf.Lerp(0.0f, 90.0f, lerpValue);

            transform.eulerAngles = new Vector3 (0, 0, angleValue);
            botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));
            botlleMaskSR.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));

            t += Time.deltaTime * rotationSpeedMult.Evaluate(angleValue);

            yield return new WaitForEndOfFrame();
        }

        angleValue = 90.0f;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));
        botlleMaskSR.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));

        StartCoroutine(RotationBack());
    }

    IEnumerator RotationBack()
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        while (t < timetoRotate)
        {
            lerpValue = t / timetoRotate;
            angleValue = Mathf.Lerp(90.0f, 0.0f, lerpValue);

            transform.eulerAngles = new Vector3(0, 0, angleValue);
            botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        angleValue = 0;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));

    }

}
