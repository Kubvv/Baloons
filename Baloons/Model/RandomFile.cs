using System;
using System.Collections.Generic;
using System.IO;

namespace Baloons.Model
{
    public class RandomFile
    {
        private readonly string folder, pattern;
        private readonly Random random = new Random();
        private List<string> files;

        public RandomFile(string folder, string pattern)
        {
            this.folder = folder;
            this.pattern = pattern;
            Reset();
        }

        public void Reset()
        {
            files = new List<string>(Directory.GetFiles(folder, pattern));
        }

        public string Next()
        {
            return files[random.Next(files.Count)];
        }

        public string ExclusiveNext()
        {
            string result = null;

            if (files.Count <= 0) Reset();

            if (files.Count > 0)
            {
                int index = random.Next(files.Count);
                result = files[index];
                files.RemoveAt(index);
            }

            return result;
        }
    }
}
