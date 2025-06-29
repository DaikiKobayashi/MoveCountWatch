#if UNITY_WEBGL
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoveCountWatch
{
    public class LocationWebEventReceiver : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void GetCurrentPosition();

        private UniTaskCompletionSource<LocationInfo> _tcs;
        
        public async UniTask<LocationInfo> GetCurrentPositionCore()
        {
            _tcs = new UniTaskCompletionSource<LocationInfo>();
            GetCurrentPosition();

            return await _tcs.Task;
        }
        
        public void GetCurrentPositionCallBack(string locationStr)
        {
            _tcs.TrySetResult(new LocationInfo(locationStr));
        }
    }
}
#endif
