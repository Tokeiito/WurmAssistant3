namespace AldursLab.WurmAssistant3.Areas.SoundManager
{
    public interface ISoundEngine
    {
        ISound Play2D(string soundFilename, bool playLooped, bool startPaused);
        float SoundVolume { get; set; }
        void StopAllSounds();
    }
}