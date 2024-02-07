using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC Files Archive")]
public class NPC : ScriptableObject
{
    public string ghostName;
    public bool guessed = false;
    [TextArea(3, 15)]
    public string[] dialogue;
    public AudioClip[] audioClips;
}
