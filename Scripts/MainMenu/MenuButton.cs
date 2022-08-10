using Godot;

public class MenuButton : TextureButton
{
    private AudioStreamPlayer _audioClick;
    private AudioStreamPlayer _audioHover;

    private RichTextLabel _buttonLabel;
    private Sprite _leftArrow;
    private Sprite _rightArrow;

    [Export] private int ArrowMarginFromCenter = 100;

    [Export] private string ButtonText;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buttonLabel = GetNode<RichTextLabel>("Label");
        _leftArrow = GetNode<Sprite>("LeftArrow");
        _rightArrow = GetNode<Sprite>("RightArrow");
        _audioHover = GetNode<AudioStreamPlayer>("HoverSound");
        _audioClick = GetNode<AudioStreamPlayer>("ClickSound");

        HideArrows(new[] { _leftArrow, _rightArrow });
        SetupText();
    }

    private void SetupText()
    {
        _buttonLabel.BbcodeText = "[center]" + ButtonText + "[/center]";
    }

    private void ShowArrows(Sprite[] sprites)
    {
        foreach (var sprite in sprites)
        {
            sprite.Visible = true;
            sprite.GlobalPosition = new Vector2(sprite.GlobalPosition.x, RectGlobalPosition.y + RectSize.y / 3.0f);
        }

        var posX = RectGlobalPosition.x + RectSize.x / 2.0f;
        _leftArrow.GlobalPosition = new Vector2(posX - ArrowMarginFromCenter, _leftArrow.GlobalPosition.y);
        _rightArrow.GlobalPosition = new Vector2(posX + ArrowMarginFromCenter, _rightArrow.GlobalPosition.y);
    }

    private void HideArrows(Sprite[] sprites)
    {
        foreach (var sprite in sprites) sprite.Visible = false;
    }

    private void _on_TextureButton_focus_entered()
    {
        ShowArrows(new[] { _leftArrow, _rightArrow });
        _audioHover.Play();
    }

    private void _on_TextureButton_focus_exited()
    {
        HideArrows(new[] { _leftArrow, _rightArrow });
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