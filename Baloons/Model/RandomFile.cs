using System;
using System.Collections.Generic;
using System.IO;

namespace Baloons.Model
{
    public class RandomFile
    {
        private readonly string folder, pattern;
        private readonly Random random = new();
        private List<string> files;

        public RandomFile(string folder, string pattern)
        {
            this.folder = folder;
            this.pattern = pattern;
            files = GetFileList();
        }

        public string Next() => files.Count > 0 ? files[random.Next(files.Count)] : string.Empty;

        public string ExclusiveNext()
        {
            string result = string.Empty;

            if (files.Count <= 0)
            {
                files = GetFileList();
            }

            if (files.Count > 0)
            {
                int index = random.Next(files.Count);
                result = files[index];
                files.RemoveAt(index);
            }

            return result;
        }

        private List<string> GetFileList() => new(Directory.GetFiles(folder, pattern));
    }
}
