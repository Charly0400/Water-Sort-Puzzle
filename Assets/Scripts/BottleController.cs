using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public Color[] bottleColors;
    public SpriteRenderer botlleMaskSR;

    public float timetoRotate = 1.0f;

    public AnimationCurve scaleAndRotationMult;
    public AnimationCurve fillAmountCurve;
    public AnimationCurve rotationSpeedMult;

    public float[] fillAmount;
    public float[] rotationValue;

    private int rotationIndex = 0;

    [Range(0, 4)]
    public int numberOfColorInBottle = 4;

    public Color topColor;
    public int numberOfTopColorLayer = 1;

    public BottleController bottleControllerRef;
    public bool justThisBottle = false;
    private int numberOfColorToTransfer = 0;

    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform chosenRotationPoint;

    private float directionMultiplier = 1.0f;

    Vector3 originalPosition;
    Vector3 startPosition;
    Vector3 endPosition;

    void Start()
    {
        botlleMaskSR.material.SetFloat("_FillAmount", fillAmount[numberOfColorInBottle]);
        originalPosition = transform.position;
        UpdateColorOnShader();
        UpdateTopColorValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && justThisBottle == true)
        {
            UpdateTopColorValues();

            if (bottleControllerRef.FillBottleCheck(topColor))
            {
                ChoseRotationDirection();

                numberOfColorToTransfer = Mathf.Min(numberOfTopColorLayer, 4 - bottleControllerRef.numberOfColorInBottle);

                for (int i = 0; i < numberOfColorToTransfer; i++)
                {
                    bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorInBottle + i] = topColor;
                }
                bottleControllerRef.UpdateColorOnShader();
            }
            CalculateRotationIndex(4 - bottleControllerRef.numberOfColorInBottle);
            StartCoroutine(RotateBottle());
        }
    }

    public void StartColorTransfer()
    {
        ChoseRotationDirection();

        numberOfColorToTransfer = Mathf.Min(numberOfTopColorLayer, 4 - bottleControllerRef.numberOfColorInBottle);

        for (int i = 0; i < numberOfColorToTransfer; i++)
        {
            bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorInBottle + i] = topColor;
        }
        bottleControllerRef.UpdateColorOnShader();

        CalculateRotationIndex(4 - bottleControllerRef.numberOfColorInBottle);

        transform.GetComponent<SpriteRenderer>().sortingOrder += 2;
        botlleMaskSR.sortingOrder += 2;

        StartCoroutine(MoveBottle());
    }

    IEnumerator MoveBottle()
    {
        startPosition = transform.position;
        if (chosenRotationPoint == leftRotationPoint)
        {
            endPosition = bottleControllerRef.rightRotationPoint.position;
        }
        else
        {
            endPosition = bottleControllerRef.leftRotationPoint.position;
        }

        float t = 0;

        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;

        StartCoroutine(RotateBottle());
    }

    IEnumerator MoveBottleBack()
    {
        startPosition = transform.position;
        endPosition = originalPosition;
        float t = 0;

        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;
        transform.GetComponent<SpriteRenderer>().sortingOrder -= 2;
        botlleMaskSR.sortingOrder -= 2;
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

        float lastAngleValue = 0;

        while (t < timetoRotate)
        {
            lerpValue = t / timetoRotate;
            angleValue = Mathf.Lerp(0.0f, directionMultiplier * rotationValue[rotationIndex], lerpValue);

            //transform.eulerAngles = new Vector3 (0, 0, angleValue);
            transform.RotateAround(chosenRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));

            if (fillAmount[numberOfColorInBottle] > fillAmountCurve.Evaluate(angleValue))
            {
                botlleMaskSR.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));

                bottleControllerRef.FillUp(fillAmountCurve.Evaluate(lastAngleValue) - fillAmountCurve.Evaluate(angleValue));
            }

            t += Time.deltaTime * rotationSpeedMult.Evaluate(angleValue);
            lastAngleValue = angleValue;
            yield return new WaitForEndOfFrame();
        }

        angleValue = directionMultiplier * rotationValue[rotationIndex];
        //transform.eulerAngles = new Vector3(0, 0, angleValue);
        botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));
        botlleMaskSR.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));

        numberOfColorInBottle -= numberOfColorToTransfer;
        bottleControllerRef.numberOfColorInBottle += numberOfColorToTransfer;
        StartCoroutine(RotationBack());
    }

    IEnumerator RotationBack()
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = directionMultiplier * rotationValue[rotationIndex];

        while (t < timetoRotate)
        {
            lerpValue = t / timetoRotate;
            angleValue = Mathf.Lerp(directionMultiplier * rotationValue[rotationIndex], 0.0f, lerpValue);

            //transform.eulerAngles = new Vector3(0, 0, angleValue);
            transform.RotateAround(chosenRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));

            lastAngleValue = angleValue;

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        UpdateTopColorValues();
        angleValue = 0;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        botlleMaskSR.material.SetFloat("_SARM", scaleAndRotationMult.Evaluate(angleValue));


        StartCoroutine(MoveBottleBack());
    }

    public void UpdateTopColorValues()
    {
        if (numberOfColorInBottle != 0)
        {
            numberOfTopColorLayer = 1;

            topColor = bottleColors[numberOfColorInBottle - 1];

            if (numberOfColorInBottle == 4)
            {
                if (bottleColors[3].Equals(bottleColors[2]))
                {
                    numberOfTopColorLayer = 2;
                    if (bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberOfTopColorLayer = 3;
                        if (bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberOfTopColorLayer = 4;
                        }
                    }
                }
            }

            else if (numberOfColorInBottle == 3)
            {
                if (bottleColors[2].Equals(bottleColors[1]))
                {
                    numberOfTopColorLayer = 2;
                    if (bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberOfTopColorLayer = 3;
                    }
                }
            }

            else if (numberOfColorInBottle == 2)
            {
                if (bottleColors[1].Equals(bottleColors[0]))
                {
                    numberOfTopColorLayer = 2;
                }
            }

            rotationIndex = 3 - (numberOfColorInBottle - numberOfTopColorLayer);
        }
    }

    public bool FillBottleCheck(Color colorCheck)
    {
        if (numberOfColorInBottle == 0)
        {
            return true;
        }
        else
        {
            if (numberOfColorInBottle == 4)
            {
                return false;
            }
            else
            {
                if (topColor.Equals(colorCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void CalculateRotationIndex(int numberOfEmptyEspaceInBottle)
    {
        rotationIndex = 3 - (numberOfColorInBottle - Mathf.Min(numberOfEmptyEspaceInBottle, numberOfTopColorLayer));
    }

    private void FillUp(float fillAmountToAdd)
    {
        botlleMaskSR.material.SetFloat("_FillAmount", botlleMaskSR.material.GetFloat("_FillAmount") + fillAmountToAdd);
    }

    private void ChoseRotationDirection()
    {
        if (transform.position.x > bottleControllerRef.transform.position.x)
        {
            chosenRotationPoint = leftRotationPoint;
            directionMultiplier = -1.0f;
        }
        else
        {
            chosenRotationPoint = rightRotationPoint;
            directionMultiplier = 1.0f;
        }
    }
}