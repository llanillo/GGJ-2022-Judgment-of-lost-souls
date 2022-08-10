using Godot;

namespace JudgmentOfLostSouls.Animation
{
    public class TransitionScreen : Control
    {
        [Signal]
        public delegate void Transitioned();

        private AnimationPlayer _animationPlayer;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            StartTransition();
        }

        public void StartTransition()
        {
            _animationPlayer.Play("FadeToBlack");
        }

        private void HandleFinishedAnimation(string animationName)
        {
            if (animationName.Equals("FadeToBlack"))
            {
                GD.Print(animationName);
                EmitSignal("Transitioned");
                _animationPlayer.Play("FadeToNormal");
            }
            else if (animationName.Equals("FadeToNormal"))
            {
                GD.Print("Finished");
            }
        }
    }
}