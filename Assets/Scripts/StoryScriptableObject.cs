using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story", menuName = "StoryChunk")]
public class StoryScriptableObject : ScriptableObject
{
    public List<string> chuncks;
    public Sprite npcSprite;
}


