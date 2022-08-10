using Godot;

namespace JudgmentofLostSouls.Scripts.Dialogues
{
    public class ContinueButton : TextureButton
    {
        private const string BounceAnimation = "Bounce";


        public override void _Ready()
        {
            var animationPlayer = GetChild<AnimationPlayer>(0);
            animationPlayer.Play(BounceAnimation);
        }
    }
}