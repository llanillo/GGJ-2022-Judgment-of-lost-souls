using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;

public class OptionManager : Dialogue
{

    public override string NodePath { get; } = "Panel/";
    public override string JsonName { get; } = "/Option";

    private readonly List<Points> CurrentOptions = new List<Points>();
    private const string LabelPath = "Panel/Margin/VBox/";
    private const string HudPath = "Hud/";
    
    private PlayerVariables _playerVariables;
    private TextureRect _portrait;
    private TextEdit _textEdit;
    private Label _nameLabel;
    private Label _karmaLabel;
    private Label _blessLabel;
    private Label _ageLabel;
    private Label _deathCauseLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _nameLabel = GetNode<Label>(NodePath + "NamePanel/Margin/Name");
        _ageLabel = GetNode<Label>(LabelPath + "Age");
        _deathCauseLabel = GetNode<Label>(LabelPath + "DeathText");
        _playerVariables = GetNode<PlayerVariables>("/root/PlayerVariables");
        _karmaLabel = GetNode<Label>(HudPath + "KarmaBox/Karma");
        _blessLabel = GetNode<Label>(HudPath + "BlessBox/Bless");
        _portrait = GetNode<TextureRect>("Portrait");
        _textEdit = GetNode<TextEdit>(LabelPath + "Text");
        DisplayText(QueueDialogues, CurrentOptions);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _karmaLabel.Text = "Karma: " + _playerVariables.Karma.ToString();
        _blessLabel.Text = "Bendicion: " + _playerVariables.Bless.ToString();
    }

    private void DisplayText(Queue<Dictionary> dialoguesDictionary, List<Points> currentPoints)
    {
        if (QueueDialogues.Count > 0)
        {
            // Get the next options from the Queue
            Dictionary options = dialoguesDictionary.Dequeue();

            // Create a new Array with all the Options in the dialogue
            foreach (var option in options["options"] as Godot.Collections.Array)
            {
                Dictionary dictionary = option as Dictionary;
                float karma = float.Parse((string) dictionary["karma"], CultureInfo.InvariantCulture.NumberFormat);
                float bless = float.Parse((string) dictionary["bless"], CultureInfo.InvariantCulture.NumberFormat);
                CurrentOptions.Add(new Points(karma, bless));
            }

            _portrait.Texture = (Texture) GD.Load(CharacterImagesPath + "/" + (string) options["image"] + ".png");
            _ageLabel.Text = "Edad " + (string) options["age"];
            _textEdit.Text = (string) options["story"];
            _nameLabel.Text = (string) options["name"];
            _deathCauseLabel.Text = (string) options["death"];
        }
        else
        {
            GameManager.StartGame();
        }
    }


    private void OnHeavenButtonPressed()
    {
        PressButton(_playerVariables, CurrentOptions[0]);
    }

    private void OnPurgatoryButtonPressed()
    {
        PressButton(_playerVariables, CurrentOptions[1]);
    }

    private void OnHellButtonPressed()
    {
        PressButton(_playerVariables, CurrentOptions[2]);
    }

    private void PressButton(PlayerVariables playerVariables, Points points)
    {
        _playerVariables.AddPlayerValues(points);
        CurrentOptions.Clear();
        DisplayText(QueueDialogues, CurrentOptions);
    }
}
