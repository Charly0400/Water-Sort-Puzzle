using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottlController firstBottle;
    public BottlController secondBottle;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                BottlController clickedBottle = hit.collider.GetComponent<BottlController>();

                if (clickedBottle != null)
                {
                    if (firstBottle == null)
                    {
                        firstBottle = clickedBottle;
                    }
                    else
                    {
                        if (firstBottle == clickedBottle)
                        {
                            firstBottle = null;
                        }
                        else
                        {
                            secondBottle = clickedBottle;

                            // Asignar referencias correctamente en ambas direcciones
                            firstBottle.bottleControllerRef = secondBottle;
                            secondBottle.bottleControllerRef = firstBottle;

                            // Verificar si la botella de destino puede recibir el líquido
                            if (secondBottle.FillBottleCheck(firstBottle.topColor))
                            {
                                firstBottle.StartColorTransfer();
                            }

                            // Limpiar referencias después de la transferencia
                            firstBottle = null;
                            secondBottle = null;
                        }
                    }
                }
            }
        }
    }
}
