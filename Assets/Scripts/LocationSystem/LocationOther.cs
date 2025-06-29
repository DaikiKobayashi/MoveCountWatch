using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoveCountWatch
{
    public class LocationOther : ILocation
    {
        public LocationSystemState State { get; set; } = LocationSystemState.None;

        public bool IsGettingLocation { get; set; } = false;
        
        public void Initialize()
        {
            State = LocationSystemState.Initialize;
            
            // 座標取得を初期化
            Input.location.Start();
            
            // GPSが許可されているかチェック
            if (!Input.location.isEnabledByUser)
            {
                State = LocationSystemState.NotRunning;
                return;
            }
            
            // サービスが有効になるまで待機
            UniTask.Void(async () =>
            {
                while (true)
                {
                    if (Input.location.status is LocationServiceStatus.Initializing)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                    }
                    else
                    {
                        break;
                    }
                }

                switch (Input.location.status)
                {
                    case LocationServiceStatus.Running:
                        State = LocationSystemState.Running;
                        break;
                    
                    case LocationServiceStatus.Stopped:
                    case LocationServiceStatus.Failed:
                        State = LocationSystemState.NotRunning;
                        break;
                }
                
                Debug.Log($"LocationServiceStatus: {Input.location.status}");
            });
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
                var curr = Input.location.lastData;
                return new LocationInfo(curr.latitude, curr.longitude);
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
