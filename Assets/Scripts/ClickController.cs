using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    public BottleController firsBottle;
    public BottleController secondBottle;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<BottleController>() != null)
                {
                    if (firsBottle == null)
                    {
                        firsBottle = hit.collider.GetComponent<BottleController>();
                    }
                    else
                    {
                        if (firsBottle == hit.collider.GetComponent<BottleController>())
                        {
                            firsBottle = null;
                        }
                        else
                        {
                            secondBottle = hit.collider.GetComponent<BottleController>();
                            firsBottle.bottleControllerRef = secondBottle;

                            firsBottle.UpdateTopColorValues();
                            secondBottle.UpdateTopColorValues();

                            if (secondBottle.FillBottleCheck(firsBottle.topColor) == true)
                            {
                                firsBottle.StartColorTransfer();
                                firsBottle = null;
                                secondBottle = null;
                            }
                            else
                            {
                                firsBottle = null;
                                secondBottle = null;
                            }
                        }
                    }
                }
            }
        }
    }
}