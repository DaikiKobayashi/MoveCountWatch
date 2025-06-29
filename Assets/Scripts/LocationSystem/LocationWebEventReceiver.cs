#if UNITY_WEBGL
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace MoveCountWatch
{
    public class LocationWebEventReceiver : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void GetCurrentPosition();

        private TaskCompletionSource<LocationInfo> _tcs;
        
        public async ValueTask<LocationInfo> GetCurrentPositionCore()
        {
            _tcs = new TaskCompletionSource<LocationInfo>();
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
