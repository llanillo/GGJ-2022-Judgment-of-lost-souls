using Godot;
using JudgmentOfLostSouls.Manager;

namespace JudgmentOfLostSouls.Extras
{
    public class Credits : Control
    {
        private const string MenuPath = "MainMenu/MainMenu.tscn";
        private GameManager _gameManager;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _gameManager = GetNode<GameManager>("/root/GameManager");
        }

        private void OnTextureButtonPressed()
        {
            _gameManager.LoadScene(MenuPath);
        }
    }
}