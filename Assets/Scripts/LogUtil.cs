using System.Runtime.InteropServices;
using UnityEngine;

namespace MoveCountWatch
{
    public class LogUtil : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ConsoleLog(string logString);

        /// <summary>
        /// ブラウザのコンソールにメッセージを出力する
        /// </summary>
        /// <param name="score"></param>
        public static void OutputConsoleLog(string message)
        {
            if (CheckWebGLPlatform())
            {
                ConsoleLog(message);
            }
        }

        public static bool CheckWebGLPlatform()
        {
            return Application.platform == RuntimePlatform.WebGLPlayer;
        }
    }
}