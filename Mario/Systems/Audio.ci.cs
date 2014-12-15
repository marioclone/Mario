public class SystemAudio : GameSystem
{
    public SystemAudio()
    {
        currentMusic = null;
        currentMusicAudio = null;
        wasPaused = false;
        audio = new DictionaryStringAudio[audioMax];
        for (int i = 0; i < audioMax; i++)
        {
            audio[i] = new DictionaryStringAudio();
        }
        currentAudio = 0;
        wasLoaded = false;
    }

    string currentMusic;
    AudioCi currentMusicAudio;
    // Multiple dictionaries, to allow playing multiple sounds of the same type
    DictionaryStringAudio[] audio;
    const int audioMax = 5;
    int currentAudio;
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
        }

        // Play music
        if (currentMusic != game.audio.audioPlayMusic)
        {
            if (currentMusic != null)
            {
                game.platform.AudioPause(currentMusicAudio);
            }
            currentMusic = game.audio.audioPlayMusic;
            currentMusicAudio = game.platform.AudioCreate(game.GetFile(currentMusic), game.GetFileLength(currentMusic));
            game.platform.AudioPlay(currentMusicAudio);
        }

        // Play sounds
        for (int i = 0; i < game.audio.audioPlaySoundsCount; i++)
        {
            string sound = game.audio.audioPlaySounds[i];
            if (sound == null)
            {
                continue;
            }

            if (!audio[currentAudio].Contains(sound))
            {
                AudioCi a = game.platform.AudioCreate(game.GetFile(sound), game.GetFileLength(sound));
                audio[currentAudio].Set(sound, a);
            }
            game.platform.AudioPlay(audio[currentAudio].GetById(audio[currentAudio].GetId(sound)));
            game.audio.audioPlaySounds[i] = null;
            currentAudio++;
            currentAudio = currentAudio % audioMax;
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
