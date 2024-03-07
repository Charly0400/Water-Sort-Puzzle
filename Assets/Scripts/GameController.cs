using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottlController firstBottle;
    public BottlController secondBottle;

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
                BottlController hitBottle = hit.collider.GetComponent<BottlController>();
                if (hitBottle != null)
                {
                    if (firstBottle == null)
                    {
                        firstBottle = hitBottle;
                    }
                    else
                    {
                        if (firstBottle == hitBottle)
                        {
                            firstBottle = null;
                        }
                        else
                        {
                            secondBottle = hitBottle;

                            firstBottle.bottleControllerRef = secondBottle;
                            firstBottle.updateTopColorValues();
                            secondBottle.updateTopColorValues();

                            if (secondBottle.FillBottleCheck(firstBottle.topColor))
                            {
                                firstBottle.StartColorTransfer();
                            }

                            firstBottle = null;
                            secondBottle = null;
                        }
                    }
                }
            }
        }
    }
}
