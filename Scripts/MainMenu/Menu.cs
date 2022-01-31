using Godot;
using System;

public class Menu : Node
{

    private const string CreditsPath = "Extras/Credits.tscn";
    private GameManager _gameManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<TextureButton>("VBox/VBox/StartButton").GrabFocus();
        GetNode<PlayerVariables>("/root/PlayerVariables").ResetValues();
        GetNode<MusicManager>("/root/Music").ChangeMusic(GameMusic.EARTH);
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _gameManager.Ended = false;
        _gameManager.SceneNumber = 0;
    }

    private void _on_StartButton_pressed()
    {
        _gameManager.StartGame();
    }

    private void _on_CreditsButton_pressed()
    {
        _gameManager.LoadScene(CreditsPath);
    }
    
    private void _on_ExitButton_pressed()
    {
        GetTree().Quit();
    }
}
