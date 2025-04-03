using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteExtractor : MonoBehaviour
{
    public List<GameObject> performAnimals = new List<GameObject>();
    [HideInInspector] public Dictionary<string, List<Sprite>> animalSprites = new Dictionary<string, List<Sprite>>();
    void Start()
    {
        List<string> animalNames = new List<string>();
        foreach (animalProperty animal in GlobalManager.instance.allAnimals.properies)
        {
            animalNames.Add(animal.animalName);
        }

        foreach (GameObject animal in ShowManager.instance.animalPerformancePrefabs)
        {
            foreach (string animalName in animalNames)
            {
                if (animal.name.Contains(animalName))
                {
                    PerformAnimalControl animalControl = animal.GetComponent<PerformAnimalControl>();
                    animalSprites[animalName] = new List<Sprite>();
                    animalSprites[animalName] = animalControl.displaySprites;
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
