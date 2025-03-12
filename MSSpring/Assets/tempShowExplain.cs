using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempShowExplain : MonoBehaviour
{
    [SerializeField] private GameObject explainObj; // 需要在 Inspector 里赋值
    public float width = 150;
    public float up = 300;

    public List<Sprite> images;
    public Sprite MechnnicExplain;

    public void StartExplain(RectTransform target, bool IfDown, animalProperty property)
    {
        if (explainObj == null) return;

        explainObj.SetActive(true);

        Vector3 targetPosition = target.position;
        float screenMidX = Screen.width / 2f;

        if (IfDown)
        {
            // 设置到目标的上方
            explainObj.transform.position = new Vector3(targetPosition.x, targetPosition.y + up, targetPosition.z);
        }
        else
        {
            // 判断左右位置
            if (targetPosition.x < screenMidX)
            {
                // 目标在屏幕左侧 -> 显示在右侧
                explainObj.transform.position = new Vector3(targetPosition.x + width, targetPosition.y, targetPosition.z);
            }
            else
            {
                // 目标在屏幕右侧 -> 显示在左侧
                explainObj.transform.position = new Vector3(targetPosition.x - width, targetPosition.y, targetPosition.z);
            }
        }
        // 获取 Image 组件
        Image explainImage = explainObj.GetComponent<Image>();

        // 赋值 Sprite
        explainImage.sprite = AnimalFactory(property.animalName);

        // 获取 RectTransform 并直接使用 Sprite 的大小
        RectTransform explainRect = explainObj.GetComponent<RectTransform>();
        if (explainRect != null)
        {
            explainRect.sizeDelta = AnimalFactory(property.animalName).rect.size;
        }
    }

    public void StartMechanicExplain(RectTransform target)
    {
        if (explainObj == null) return;

        explainObj.SetActive(true);

        // 设置位置
        Vector3 targetPosition = target.position;
        explainObj.transform.position = new Vector3(targetPosition.x +320, targetPosition.y-80, targetPosition.z);

        // 获取 Image 组件
        Image explainImage = explainObj.GetComponent<Image>();
        if (explainImage == null || MechnnicExplain == null) return;

        // 赋值 Sprite
        explainImage.sprite = MechnnicExplain;

        // 获取 RectTransform 并直接使用 Sprite 的大小
        RectTransform explainRect = explainObj.GetComponent<RectTransform>();
        if (explainRect != null)
        {
            explainRect.sizeDelta = MechnnicExplain.rect.size;
        }
    }

   
    public void StartExplain(Vector3 worldPosition, bool IfDown, animalProperty property)
    {
        if (explainObj == null) return;

        explainObj.SetActive(true);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        float screenMidX = Screen.width / 2f;

        if (IfDown)
        {
            // 设置到目标的上方
            explainObj.transform.position = new Vector3(screenPosition.x, screenPosition.y + up, screenPosition.z);
        }
        else
        {
            // 判断左右位置
            if (screenPosition.x < screenMidX)
            {
                // 目标在屏幕左侧 -> 显示在右侧
                explainObj.transform.position = new Vector3(screenPosition.x + width, screenPosition.y, screenPosition.z);
            }
            else
            {
                // 目标在屏幕右侧 -> 显示在左侧
                explainObj.transform.position = new Vector3(screenPosition.x - width, screenPosition.y, screenPosition.z);
            }
        }
        explainObj.GetComponent<Image>().sprite = AnimalFactory(property.animalName);
    }

    public Sprite AnimalFactory(string name)
    {
        switch (name)
        {
            case "Monkey":
                return images[0];

            case "Elephant":
                return images[1];

            case "Bear":
                return images[2];

            case "Lion":
                return images[3];

            case "Giraffe":
                return images[4];

            case "Snake":
                return images[5];

            case "Fox":
                return images[6];

            case "Seal":
                return images[7];

            case "Ostrich":
                return images[8];

            case "Kangaroo":
                return images[9];

            case "Buffalo":
                return images[10];

            case "Goat":
                return images[11];

            case "Lizard":
                return images[12];
        }
        return null;
    }

    public void DownExplain()
    {
        if (explainObj != null)
        {
            explainObj.SetActive(false);
        }
    }
}
