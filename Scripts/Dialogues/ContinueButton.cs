using Godot;
using System;

public class ContinueButton : TextureButton
{
    private const string BounceAnimation = "Bounce";
    

    public override void _Ready()
    {
        AnimationPlayer _animationPlayer = GetChild<AnimationPlayer>(0);
        _animationPlayer.Play(BounceAnimation);
    }

}
