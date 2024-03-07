/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bottlePrefab;
    public Transform bottlesParent;

    private List<BottleController> bottles = new List<BottleController>();
    private bool levelCompleted = false;

    void Start()
    {
        GenerateRandomLevel();
    }

    void Update()
    {
        if (!levelCompleted)
        {
            // Verificar si se completó el nivel
            if (AllBottlesFilled() && BottlesInCorrectOrder())
            {
                levelCompleted = true;
                StartCoroutine(GenerateNewLevel());
            }
        }
    }

    void GenerateRandomLevel()
    {
        // Generar entre 4 y 6 botellas
        int numBottles = Random.Range(4, 7);
        int emptyBottles = 0;
        int maxColors = 3; // Máximo de colores distintos para las botellas

        // Lista de colores posibles
        List<Color> possibleColors = new List<Color>();
        possibleColors.Add(Color.red);
        possibleColors.Add(Color.blue);
        possibleColors.Add(Color.green);
        possibleColors.Add(Color.yellow);

        for (int i = 0; i < numBottles; i++)
        {
            GameObject newBottle = Instantiate(bottlePrefab, bottlesParent);

            // Asignar un color aleatorio a la botella
            BottleController bottleController = newBottle.GetComponent<BottleController>();
            Color[] colors = new Color[4];
            int colorIndex = 0;
            for (int j = 0; j < 4; j++)
            {
                if (colorIndex >= maxColors)
                    colorIndex = 0;
                colors[j] = possibleColors[colorIndex];
                colorIndex++;
            }
            bottleController.SetBottleColors(colors);

            // Asignar un nivel de llenado aleatorio a la botella
            int fillLevel = Random.Range(0, 5); // Nivel de llenado entre 0 y 4
            bottleController.numberOfColorInBottle = fillLevel;

            bottles.Add(bottleController);

            // Al menos dos botellas estarán vacías
            if (emptyBottles < 2)
            {
                bottleController.ClearBottle();
                emptyBottles++;
            }
        }
    }

    bool AllBottlesFilled()
    {
        foreach (BottleController bottle in bottles)
        {
            if (bottle.numberOfColorInBottle != 4)
            {
                return false;
            }
        }
        return true;
    }

    bool BottlesInCorrectOrder()
    {
        Color lastColor = bottles[0].topColor;

        foreach (BottleController bottle in bottles)
        {
            if (bottle.topColor != lastColor)
            {
                return false;
            }
            lastColor = bottle.topColor;
        }
        return true;
    }

    IEnumerator GenerateNewLevel()
    {
        // Esperar un segundo antes de generar un nuevo nivel
        yield return new WaitForSeconds(1.0f);

        // Destruir las botellas actuales
        foreach (BottleController bottle in bottles)
        {
            Destroy(bottle.gameObject);
        }
        bottles.Clear();

        // Generar un nuevo nivel
        GenerateRandomLevel();
    }
}*/