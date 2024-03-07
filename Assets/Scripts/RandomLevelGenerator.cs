using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGenerator : MonoBehaviour
{
    public List<Color> colorPalette; // Paleta de colores disponibles para las botellas
    public int numberOfBottles = 4; // Número de botellas en el nivel

    public GameObject bottlePrefab; // Prefab de la botella
    public Transform bottlesParent; // Padre para las botellas

    private List<BottlController> bottleControllers = new List<BottlController>(); // Lista de controladores de botellas en el nivel

    public Vector2Int gridDimensions = new Vector2Int(2, 2); // Dimensiones de la cuadrícula para colocar las botellas

    void Start()
    {
        GenerateRandomLevel();
    }

    public void GenerateRandomLevel()
    {
        ClearLevel();

        List<Color> availableColors = new List<Color>(colorPalette);

        // Calcular el tamaño de cada celda en la cuadrícula
        float cellWidth = 10f / gridDimensions.x;
        float cellHeight = 10f / gridDimensions.y;

        for (int y = 0; y < gridDimensions.y; y++)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                // Calcular la posición de la botella en esta celda
                float xPos = (x + 0.5f) * cellWidth - 5f;
                float yPos = (y + 0.5f) * cellHeight - 5f;

                Vector3 bottlePosition = new Vector3(xPos, yPos, 0f);

                // Instanciar una botella en la posición calculada
                GameObject newBottle = Instantiate(bottlePrefab, bottlePosition, Quaternion.identity, bottlesParent);
                BottlController bottleController = newBottle.GetComponent<BottlController>();
                bottleControllers.Add(bottleController);

                // Seleccionar un color aleatorio de la paleta y asignarlo a la botella
                int randomIndex = Random.Range(0, availableColors.Count);
                Color randomColor = availableColors[randomIndex];
                availableColors.RemoveAt(randomIndex);

                // Configurar la botella con el color aleatorio generado
                bottleController.SetBottleColors(new Color[] { randomColor, randomColor, randomColor, randomColor });
            }
        }
    }

    private void ClearLevel()
    {
        // Limpiar cualquier configuración de nivel existente
        foreach (var bottleController in bottleControllers)
        {
            Destroy(bottleController.gameObject);
        }
        bottleControllers.Clear();
    }
}
