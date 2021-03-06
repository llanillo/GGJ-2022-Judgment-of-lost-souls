using Godot;
using System;

public class BackgroundManager : PanelContainer
{
    [Export(PropertyHint.Dir)]
    public string BackgroundPath;

    private TextureRect _mainBackground;
    private Sprite _secondaryBackground;
    private AnimationPlayer _animationPlayerBackground;

    private DialogueManager _dialogueManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _dialogueManager = GetParent<Control>() as DialogueManager;
        _mainBackground = GetNode<TextureRect>("MainBackground");
        _secondaryBackground = GetNode<Sprite>("SecondaryBackground");
        _animationPlayerBackground = GetNode<AnimationPlayer>("AnimationPlayer");

        _dialogueManager.Connect("TransitionBackground", this, "AssignBackground");
    }

    private void AssignBackground(string background, bool playAnim)
    {
        if (playAnim)
        {            
            _animationPlayerBackground.Play("Transition");
        }
        else
        {
            _secondaryBackground.Modulate = new Color(1, 1, 1, 0);
        }

        _mainBackground.Texture = (Texture) GD.Load(BackgroundPath + "/" + background + ".png");
    }
}
