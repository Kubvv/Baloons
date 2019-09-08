using System;
using System.IO;
using System.Reflection;

namespace Baloons.Model
{
    public class RandomMusic
    {
        private readonly Random random = new Random();
        private readonly string[] sounds;

        public RandomMusic()
        {
            string soundsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds");
                //Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "Sounds");
            sounds = Directory.GetFiles(soundsFolder, "*.mp3");
        }

        public Uri RandomSound()
        {
            return new Uri(sounds[random.Next(sounds.Length)]);
        }
    }
}
