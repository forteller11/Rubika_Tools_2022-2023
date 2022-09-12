using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Charly.SheetsToMaze.Utils
{
    public static class IOUtils
    {
        public const int ByteToKB = 1024;
        public const int KBToMB = 1024;
        public const int MBToGB = 1024;
        public const int ByteToMB = ByteToKB * KBToMB;

        
        public static async Task<Hash128> GetFileHash(string path, byte [] buffer)
        {
            try
            {
                Hash128 hash = new Hash128();

                Debug.Log($"Trying to open {path}");
                await using var fs = File.OpenRead(path);
                if (!fs.CanRead)
                {
                    Debug.LogError($"Couldn't read from {nameof(fs)} at {path}. Access Control: {fs.GetAccessControl()}");
                    hash.Append(Random.value);
                    return hash;
                }
                
                int bytesRead = 1;
                while (bytesRead > 0)
                {
                    bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length);
                    hash.Append(buffer, 0, bytesRead);
                }
                return hash;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw new Exception();
            }
            
        }
    }
}