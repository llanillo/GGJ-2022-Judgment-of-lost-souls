using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public abstract class Dialogue : Control
{
    [Export(PropertyHint.Dir)]
    public string JsonPath;

    [Export(PropertyHint.Dir)]
    public string CharacterImagesPath;

    public abstract string NodePath { get; }
    public abstract string JsonName { get; }
    
    protected Queue<Dictionary> QueueDialogues = new Queue<Dictionary>();    

    protected GameManager GameManager { get; private set; }    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameManager = GetNode<GameManager>("/root/GameManager");        

        AddDialogueDictionary(QueueDialogues, JsonPath, JsonName);
    }


    protected void AddDialogueDictionary(Queue<Dictionary> queue, string jsonPath, string dialogueType)
    {
        Dictionary dictionary = GameManager.LoadDialogueFromJson(jsonPath, dialogueType);
        for (int i = 0; i < dictionary.Count; i++)
        {
            Dictionary dialogue = dictionary[i.ToString()] as Dictionary;            
            queue.Enqueue(dialogue);
        }
    }
}
