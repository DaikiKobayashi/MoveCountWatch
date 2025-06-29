#if UNITY_WEBGL
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoveCountWatch
{
    public class LocationWeb : ILocation
    {
        private LocationWebEventReceiver receiver;
        
        public LocationSystemState State => LocationSystemState.Running;
        
        public bool IsGettingLocation { get; set; } = false;

        public void Initialize()
        {
            var receiverObject = new GameObject("LocationWebEventReceiver");
            receiver = receiverObject.AddComponent<LocationWebEventReceiver>();
        }

        public async UniTask<LocationInfo> GetLocationAsync()
        {
            if (State != LocationSystemState.Running)
            {
                Debug.LogWarning("LocationSystem is not running.");
                return null;
            }
            
            IsGettingLocation = true;
            try
            {
                return await receiver.GetCurrentPositionCore();
            }
            catch(Exception error)
            {
                Debug.LogError(error);
            }
            finally
            {
                IsGettingLocation = false;
            }

            return null;
        }
    }
}
#endif
