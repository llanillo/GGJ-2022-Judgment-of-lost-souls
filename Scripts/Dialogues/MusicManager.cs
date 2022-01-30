using Godot;
using System;
public enum GameMusic
{
    NONE,
    PURGATORY,
    HELL,
    HEAVEN,
    EARTH
}

public class MusicManager : AudioStreamPlayer
{
    [Export(PropertyHint.Dir)]
    public string MusicPath;

    private const string EarthDante = "/The_Earth_of_Dante.wav";
    private const string HeavenDante = "/The_Heaven_of_Dante.wav";
    private const string HellDante = "/The_Hell_of_Dante.wav";
    private const string Espejismo = "/Espejismo.wav";
    private DialogueManager _dialogueManager;

    private string _currentPlaying;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _dialogueManager = GetParent<Control>() as DialogueManager;
        _dialogueManager.Connect("TransitionSong", this, "AssignSong");
        _currentPlaying = "";
    }

    private void AssignSong(GameMusic gameMusic)
    {
        if(gameMusic == GameMusic.NONE)
        {
            StreamPaused = true;
        }
        else
        {
            switch (gameMusic)
            {
                case GameMusic.PURGATORY:
                    if (!_currentPlaying.Equals(Espejismo))
                    {
                        Stream = (AudioStream) GD.Load(MusicPath + Espejismo);
                        PlaySong(ref _currentPlaying, Espejismo);
                    }
                    break;
                case GameMusic.HELL:
                    if (!_currentPlaying.Equals(HellDante))
                    {
                        Stream = (AudioStream)GD.Load(MusicPath + HellDante);
                        PlaySong(ref _currentPlaying, HellDante);
                    }
                    break;
                case GameMusic.HEAVEN:
                    if (!_currentPlaying.Equals(HeavenDante))
                    {
                        Stream = (AudioStream)GD.Load(MusicPath + HeavenDante);
                        PlaySong(ref _currentPlaying, HeavenDante);
                    }
                    break;
                case GameMusic.EARTH:
                    if (!_currentPlaying.Equals(Espejismo))
                    {
                        Stream = (AudioStream)GD.Load(MusicPath + Espejismo);
                        PlaySong(ref _currentPlaying, Espejismo);
                    }
                    break;
            }
        }
    }

    public void PlaySong(ref string current, string next)
    {
        StreamPaused = false;
        current = next;
        Play(0);        
    }
}
