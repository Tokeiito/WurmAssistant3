﻿using System;

namespace AldursLab.WurmAssistant3.Areas.SoundManager.Irrklang
{
    class IrrklangSoundEngineProxy : ISoundEngine
    {
        readonly IrrKlang.ISoundEngine engine;

        public IrrklangSoundEngineProxy()
        {
            engine = new IrrKlang.ISoundEngine();
        }

        public float SoundVolume
        {
            get => engine.SoundVolume;
            set => engine.SoundVolume = value;
        }

        public ISound Play2D(string soundFilename, bool playLooped, bool startPaused)
        {
            return new IrrklangSound(engine.Play2D(soundFilename, playLooped, startPaused));
        }

        public void StopAllSounds()
        {
            engine.StopAllSounds();
        }
    }
}
