using System;
using Godot;
using Object = Godot.Object;

namespace JudgmentOfLostSouls.Dialogues
{
    public class TextManager : Panel
    {
        private const float TextSpeedRate = 0.02f;
        private const string Path = "Margin/VBox/";

        private AudioStreamPlayer _audioPlayerText;
        private TextureButton _continueButton;
        private Label _nameLabel;
        private Label _textLabel;
        private Tween _textTween;

        public PlayerState CurrentState { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            CurrentState = PlayerState.READY;
            _nameLabel = GetNode<Label>("../NamePanel/Margin/Name");
            _audioPlayerText = GetNode<AudioStreamPlayer>(Path + "AudioPlayer");
            _continueButton = GetNode<TextureButton>(Path + "ContinueButton");
            _textLabel = GetNode<Label>(Path + "Text");
            _textTween = GetNode<Tween>(Path + "TextTween");
            _continueButton.Visible = false;
        }

        public override void _Input(InputEvent @event)
        {
            if (!@event.IsActionPressed("ui_accept")) return;

            switch (CurrentState)
            {
                case PlayerState.READING:
                    OnTextTweenTweenCompleted(null, null);
                    break;
                case PlayerState.FINISHED:
                    HandleFinishedState();
                    break;
                case PlayerState.READY:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleFinishedState()
        {
            _textLabel.Text = "";
            _nameLabel.Text = "";
            SwitchState(PlayerState.READY);
        }

        private void OnTextTweenTweenCompleted(Object obj, NodePath node)
        {
            SwitchState(PlayerState.FINISHED);
            _textLabel.PercentVisible = 1;
            _continueButton.Visible = true;
            _audioPlayerText.StreamPaused = true;
            _textTween.RemoveAll();
        }

        public void DisplayText(string name, string text)
        {
            SwitchState(PlayerState.READING);
            _audioPlayerText.StreamPaused = false;
            _continueButton.Visible = false;
            _nameLabel.Text = name;
            _textLabel.Text = text;
            _textLabel.PercentVisible = 0.0f;
            _textTween.InterpolateProperty(_textLabel, "percent_visible", 0.0f, 1.0f,
                _textLabel.Text.Length * TextSpeedRate);
            _textTween.Start();
        }

        public void OnContinueButtonPressed()
        {
            HandleFinishedState();
        }

        private void SwitchState(PlayerState nextState)
        {
            CurrentState = nextState;
        }
    }
}