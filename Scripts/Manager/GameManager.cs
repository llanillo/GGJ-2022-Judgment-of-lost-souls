using Godot;
using Godot.Collections;
using System;

public class GameManager : Node
{
    private enum GameState
    {
        OPTION_BOX,
        DIALOGUE_BOX
    }

    private const string ScenesPath = "res://Scenes/";
    private const string OptionBoxPath = "Dialogue/OptionBox.tscn";
    private const string DialogueBoxPath = "Dialogue/DialogueBox.tscn";
    private const string CreditsPath = "Extras/Credits.tscn";

    private GameState _currentState;
    private string _modifier = "";

    public int SceneNumber { get; set; }
    public bool Ended { get; set; }

    private Node CurrentScene { get; set; }

    private PlayerVariables _playerVariables;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SceneNumber = 0;
        Ended = false;
        _currentState = GameState.DIALOGUE_BOX;
        _playerVariables = GetNode<PlayerVariables>("/root/PlayerVariables");

        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public Dictionary LoadDialogueFromJson(string jsonPath, string dialogueType)
    {
        File file = new File();
        string completeJsonPath = jsonPath + "/" + SceneNumber.ToString() + dialogueType + _modifier + ".json";
        GD.Print(completeJsonPath);
        if (file.FileExists(completeJsonPath))
        {
            file.Open(completeJsonPath, File.ModeFlags.Read);
            string jsonValues = file.GetAsText();
            file.Close();
            JSONParseResult jsonResult = JSON.Parse(jsonValues);
            if (jsonResult.Error == 0)
            {
                return jsonResult.Result as Dictionary;
            }
            else
            {
                GD.Print("Error");
                GD.Print(jsonResult.ErrorString);
            }
        }
        GD.Print("Fin del juego=???");
        return new Dictionary();
    }

    public void StartGame()
    {
        CallDeferred("DeferredSwitchScene");
    }

    public void LoadScene(string path)
    {
        CallDeferred("DeferredLoadScene", path);
    }

    private void DeferredLoadScene(string path)
    {
        PackedScene scene = (PackedScene) GD.Load(ScenesPath + path);
        CurrentScene.Free();
        CurrentScene = scene.Instance();
        GetTree().Root.AddChild(CurrentScene);
        GetTree().CurrentScene = CurrentScene;
    }

    private void DeferredSwitchScene()
    {
        if (Ended)
        {
            DeferredLoadScene(CreditsPath);
        }
        else if(_currentState == GameState.DIALOGUE_BOX)
        {
            if (SceneNumber == 2)
            {
                float karma = _playerVariables.Karma;
                float bless = _playerVariables.Bless;

                if(EvaluateRange(karma, -5, 5) && EvaluateRange(bless, -5, 5))
                {
                    _modifier = "Generic";
                }
                //else if(EvaluateRange(karma, -20, 20) && EvaluateRange(bless, -20, 20))
                else
                {
                    _modifier = "HighValues";
                }
            }
            else if(SceneNumber == 3)
            {
                float karma = _playerVariables.Karma;
                float bless = _playerVariables.Bless;

                if (EvaluateRange(karma, -6, 6) && EvaluateRange(bless, -6, 6))
                {
                    _modifier = "Generic";
                }
                else if(EvaluateRange(karma, 6, 20) && EvaluateRange(bless, -20, 6))
                {
                    _modifier = "HighKarma";
                }
                else if(EvaluateRange(karma, -20, 6) && EvaluateRange(bless, 6, 20)){
                    _modifier = "HighBless";
                }
                else if (EvaluateRange(karma, -20, 6) && EvaluateRange(bless, -20, 6))
                {
                    _modifier = "LowValues";
                }
                else if(EvaluateRange(karma, 6, 20) && EvaluateRange(bless, 6, 20)) // Puede
                {
                    _modifier = "HighValues";
                }
                else
                {
                    GD.Print("No esta en rango para el 3");
                }
                Ended = true;
            }

            DeferredLoadScene(DialogueBoxPath);
            _currentState = GameState.OPTION_BOX;            
            
        }
        else
        {
            _modifier = "";
            DeferredLoadScene(OptionBoxPath);
            _currentState = GameState.DIALOGUE_BOX;
            SceneNumber++;
        }
    }

    private bool EvaluateRange(float value, int leftRange, int rightRange)
    {
        return value >= leftRange && value <= rightRange;
    }
}
