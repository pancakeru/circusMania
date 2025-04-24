using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialDialogue", menuName = "Tutorial/TutorialDialogue")]
public class TutorialDialogue : ScriptableObject
{
    public Sprite sprite;
    public List<string> dialogueList;
}
