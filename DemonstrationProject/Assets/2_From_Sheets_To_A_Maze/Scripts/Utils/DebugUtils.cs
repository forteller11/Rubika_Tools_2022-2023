using UnityEngine.Assertions;

namespace Charly.SheetsToMaze.Utils
{
    public class DebugUtils
    {
        public static void AreNotNotNull(params object[] objects)
        {
            foreach (var o in objects)
                Assert.IsNotNull(o);
        }
    }
}