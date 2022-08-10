using Godot;

public enum GameMusic
{
    NONE,
    PURGATORY,
    HELL,
    HEAVEN,
    EARTH
}

public class MusicManager : Control
{
    private const string EarthDante = "/The_Earth_of_Dante.wav";
    private const string HeavenDante = "/The_Heaven_of_Dante.wav";
    private const string HellDante = "/The_Hell_of_Dante.wav";
    private const string Espejismo = "/Espejismo.wav";

    private const float MusicVolume = -10.0f;
    private const float MinVolume = -60.0f;

    private AudioStreamPlayer _audioPlayer;

    public GameMusic _currentPlaying;
    private Timer _timer;

    [Export(PropertyHint.Dir)] public string MusicPath;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _audioPlayer = GetNode<AudioStreamPlayer>("Player");
        _timer = GetNode<Timer>("Timer");
        _currentPlaying = GameMusic.NONE;
        Connect("FadeInMusic", this, "IncreaseMusicVolume");
    }

    public void ChangeMusic(GameMusic gameMusic)
    {
        switch (gameMusic)
        {
            case GameMusic.NONE:
                PlaySong(GameMusic.NONE, null);
                break;
            case GameMusic.PURGATORY:
                PlaySong(GameMusic.PURGATORY, (AudioStream)GD.Load(MusicPath + Espejismo));
                break;
            case GameMusic.HELL:
                PlaySong(GameMusic.HELL, (AudioStream)GD.Load(MusicPath + HellDante));
                break;
            case GameMusic.HEAVEN:
                PlaySong(GameMusic.HEAVEN, (AudioStream)GD.Load(MusicPath + HeavenDante));
                break;
            case GameMusic.EARTH:
                PlaySong(GameMusic.EARTH, (AudioStream)GD.Load(MusicPath + EarthDante));
                break;
        }
    }

    private void PlaySong(GameMusic next, AudioStream audioStream)
    {
        if (_currentPlaying != next)
        {
            _currentPlaying = next;
            DecreaseMusicVolume();
            _audioPlayer.Stream = audioStream;
        }
    }

    private async void DecreaseMusicVolume()
    {
        while (_audioPlayer.VolumeDb > MinVolume)
        {
            _audioPlayer.VolumeDb -= 10;
            _timer.Start();
            await ToSignal(_timer, "timeout");
        }

        EmitSignal("FadeInMusic");
    }

    private async void IncreaseMusicVolume()
    {
        _audioPlayer.Play();
        _audioPlayer.StreamPaused = false;

        while (_audioPlayer.VolumeDb < MusicVolume)
        {
            _audioPlayer.VolumeDb += 10;
            _timer.Start();
            await ToSignal(_timer, "timeout");
        }
    }

    [Signal]
    private delegate void FadeInMusic();
}