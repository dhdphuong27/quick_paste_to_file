using System.IO;
using System.Media;
using System.Windows.Media;

namespace PasteToFile.Core
{
    public static class SoundEffectPlayer
    {
        private static Uri _soundUri;
        private static double _volume = 0.5;
        public static void PlayCustomSoundFromFile()
        {
            try
            {

                // Look for save_sound.wav in the same directory as the executable
                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chime-sound.wav");

                if (File.Exists(soundPath))
                {
                    var _player = new MediaPlayer();
                    _player.Open(new Uri(soundPath, UriKind.RelativeOrAbsolute));
                    _player.Volume = 0.5;

                    _player.MediaEnded += (s, e) =>
                    {
                        // Stop and Close the player
                        _player.Stop();
                        // Setting the Source to null helps with cleanup.
                        _player.Close();
                    };
                    _player.Play();
                }
                else
                {
                    SystemSounds.Asterisk.Play();
                }
            }
            catch (Exception)
            {
                // Fallback to system sound on error
                try { SystemSounds.Asterisk.Play(); } catch { }//or SystemSounds.Beep, .Hand, .Question, .Exclamation
            }
        }

    }
}