using Godot;
using System;

public class MenuButton : TextureButton
{
    [Export]
    private string ButtonText;
    [Export]
    private int ArrowMarginFromCenter = 100;

    private RichTextLabel _buttonLabel;
    private Sprite _leftArrow;
    private Sprite _rightArrow;
    private AudioStreamPlayer _audioHover;
    private AudioStreamPlayer _audioClick;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buttonLabel = GetNode<RichTextLabel>("Label");
        _leftArrow = GetNode<Sprite>("LeftArrow");
        _rightArrow = GetNode<Sprite>("RightArrow");
        _audioHover = GetNode<AudioStreamPlayer>("HoverSound");
        _audioClick = GetNode<AudioStreamPlayer>("ClickSound");

        HideArrows(new Sprite[] { _leftArrow, _rightArrow });
        SetupText();
    }

    private void SetupText()
    {
        _buttonLabel.BbcodeText = "[center]" + ButtonText + "[/center]";
    }

    private void ShowArrows(Sprite[] sprites)
    {
        foreach(Sprite sprite in sprites)
        {
            sprite.Visible = true;
            sprite.GlobalPosition = new Vector2(sprite.GlobalPosition.x, RectGlobalPosition.y + (RectSize.y / 3.0f));
        }

        float posX = RectGlobalPosition.x + (RectSize.x / 2.0f);
        _leftArrow.GlobalPosition = new Vector2(posX - ArrowMarginFromCenter, _leftArrow.GlobalPosition.y);
        _rightArrow.GlobalPosition = new Vector2(posX + ArrowMarginFromCenter, _rightArrow.GlobalPosition.y);
    }

    private void HideArrows(Sprite[] sprites)
    {
        foreach(Sprite sprite in sprites)
        {
            sprite.Visible = false;
        }
    }

    private void _on_TextureButton_focus_entered()
    {
        ShowArrows(new Sprite[] { _leftArrow, _rightArrow });
        _audioHover.Play();
    }

    private void _on_TextureButton_focus_exited()
    {
        HideArrows(new Sprite[] { _leftArrow, _rightArrow });
    }

    private void _on_TextureButton_mouse_entered()
    {
        GrabFocus();        
    }

    private void _on_TextureButton_pressed()
    {
        _audioClick.Play();
    }
}
