using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public enum PlayerState
{
    READY,
    READING,
    FINISHED
}

public class DialogueManager : Dialogue
{

    [Signal]
    public delegate void TransitionBackground(string backgroundPath, bool playAnim);
    [Signal]
    public delegate void TransitionSong(GameMusic gameMusic);

    public override string JsonName { get; } = "/Dialogue";
    public override string NodePath { get; } = "TextPanel/Margin/VBox/";

    private AudioStreamPlayer _audioPlayer;
    private TextureRect _leftPortrait;
    private TextureRect _rightPortrait;
    private TextureRect _middlePortrait;
    private TextManager _textManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _leftPortrait = GetNode<TextureRect>("LeftPortrait");
        _middlePortrait = GetNode<TextureRect>("MiddlePortrait");
        _rightPortrait = GetNode<TextureRect>("RightPortrait");
        _textManager = GetNode<Node>("TextPanel") as TextManager;
        _audioPlayer = GetNode<AudioStreamPlayer>("Music");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_textManager.CurrentState == PlayerState.READY)
        {
            DisplayText(QueueDialogues);
        }
    }

    private void DisplayText(Queue<Dictionary> dictionary)
    {
        if(QueueDialogues.Count > 0)
        {
            Dictionary dialogue = dictionary.Dequeue();

            if (dialogue.Contains("remove"))
            {
                foreach (var position in dialogue["remove"] as Godot.Collections.Array)
                {
                    Dictionary optionsDictionary = position as Dictionary;
                    switch ((string) optionsDictionary["position"])
                    {
                        case "0":
                            _leftPortrait.Visible = false;
                            break;
                        case "1":
                            _rightPortrait.Visible = false;
                            break;
                        case "2":
                            _middlePortrait.Visible = false;
                            break;
                    }
                }
            }
            else
            {
                if (dialogue.Contains("background"))
                {
                    string background = (string) dialogue["background"];
                    EmitSignal("TransitionBackground", background, bool.Parse((string) dialogue["animation"]));
                    if (background.Equals("Purgatory") || background.Equals("Waiting_Room"))
                    {
                        EmitSignal("TransitionSong", GameMusic.PURGATORY);
                    }
                    else if (background.Contains("Black"))
                    {
                        EmitSignal("TransitionSong", GameMusic.NONE);
                    }
                    else if (background.Contains("Heaven"))
                    {
                        EmitSignal("TransitionSong", GameMusic.HEAVEN);
                    }
                    else if (background.Contains("Hell"))
                    {
                        EmitSignal("TransitionSong", GameMusic.HELL);
                    }
                    else if (background.Contains("Earth"))
                    {
                        EmitSignal("TransitionSong", GameMusic.EARTH);
                    }
                    else
                    {
                        GD.Print("No se encontro cancion");
                    }                    
                }

                string image = (string) dialogue["image"] + ".png";
                switch (int.Parse((string) dialogue["position"]))
                {
                    case 0:
                        ModifyPortrait(image, _leftPortrait, new TextureRect[] { _middlePortrait, _rightPortrait });
                        break;
                    case 1:
                        ModifyPortrait(image, _rightPortrait, new TextureRect[] { _leftPortrait, _middlePortrait });
                        break;
                    case 2:
                        ModifyPortrait(image, _middlePortrait, new TextureRect[] { _rightPortrait, _leftPortrait });
                        break;
                }

                _textManager.DisplayText((string)dialogue["name"], (string)dialogue["text"]);                
            }
        }
        else
        {
            GameManager.StartGame();
        }
              
    }

    private void ModifyPortrait(string image, TextureRect currentTexture, TextureRect[] secondaryTexture)
    {
        currentTexture.Visible = true;
        currentTexture.Texture = (Texture) GD.Load(CharacterImagesPath + "/" + image);
        currentTexture.Modulate = new Color(1, 1, 1, 1);
        foreach(TextureRect texture in secondaryTexture)
        {
            texture.Modulate = new Color(0.25f, 0.25f, 0.25f, 1);
        }
    }
}
