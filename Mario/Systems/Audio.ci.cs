public class SystemAudio : GameSystem
{
    public SystemAudio()
    {
        currentMusic = null;
        currentMusicAudio = null;
        wasPaused = false;
        audio = new DictionaryStringAudioData();
        wasLoaded = false;
    }

    string currentMusic;
    AudioCi currentMusicAudio;
    DictionaryStringAudioData audio;
    bool wasPaused;
    bool wasLoaded;

    public override void Update(Game game, float dt)
    {
        if (game.assetsLoaded.value != 1)
        {
            return;
        }

        if (!wasLoaded)
        {
            wasLoaded = true;
            game.audio.ClearSounds();
            Preload(game);
        }

        // Play music
        if (currentMusic != game.audio.audioPlayMusic)
        {
            if (currentMusic != null)
            {
                game.platform.AudioPause(currentMusicAudio);
            }
            currentMusic = game.audio.audioPlayMusic;
            AudioData data = GetAudioData(game, currentMusic);
            currentMusicAudio = game.platform.AudioCreate(data);
            game.platform.AudioPlay(currentMusicAudio);
        }

        // Loop music
        if (currentMusic != null)
        {
            if (game.platform.AudioFinished(currentMusicAudio))
            {
                game.platform.AudioPlay(currentMusicAudio);
            }
        }

        // Play sounds
        for (int i = 0; i < game.audio.audioPlaySoundsCount; i++)
        {
            string sound = game.audio.audioPlaySounds[i];
            if (sound == null)
            {
                continue;
            }

            AudioData data = GetAudioData(game, sound);
            AudioCi audio_ = game.platform.AudioCreate(data);
            game.platform.AudioPlay(audio_);
            game.audio.audioPlaySounds[i] = null;
        }
        game.audio.audioPlaySoundsCount = 0;

        // Stop playing music when game is paused
        if (currentMusicAudio != null)
        {
            if ((!wasPaused) && game.gamePaused)
            {
                game.platform.AudioPause(currentMusicAudio);
            }
            else if (wasPaused && (!game.gamePaused))
            {
                game.platform.AudioPlay(currentMusicAudio);
            }
            wasPaused = game.gamePaused;
        }
    }

    void Preload(Game game)
    {
        for (int k = 0; k < game.assets2.count; k++)
        {
            string s = game.assets2.items[k].name;
            string sound = game.platform.StringReplace(s, ".ogg", "");
            if (s == sound)
            {
                continue;
            }
            GetAudioData(game, s);
        }
    }

    AudioData GetAudioData(Game game, string sound)
    {
        if (!audio.Contains(sound))
        {
            AudioData a = game.platform.AudioDataCreate(game.GetFile(sound), game.GetFileLength(sound));
            audio.Set(sound, a);
        }
        return audio.GetById(audio.GetId(sound));
    }

    public override void OnFocusChanged(Game game, bool focus)
    {
        if (currentMusic == null) { return; }
        if (!focus)
        {
            if (!game.gamePaused)
            {
                game.AudioPlay("Pause");
                Update(game, one / 100);
                game.platform.AudioPause(currentMusicAudio);
            }
        }
        else
        {
            if (!game.gamePaused)
            {
                game.AudioPlay("Pause");
                game.platform.AudioPlay(currentMusicAudio);
            }
        }
    }
}
