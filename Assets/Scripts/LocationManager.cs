using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MoveCountWatch
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private Text _latitudeText;  // 緯度
        [SerializeField] private Text _longitudeText; // 経度
        [SerializeField] private Text _totalDistance; // 移動距離


        [Space(10)] 
        [SerializeField] private Button _executeMapFetchButton;
        [SerializeField] private Image _mapImage;
        
        private ILocation _location;
        private GoogleMapManager _mapManager;
        
        [DllImport("__Internal")]
        private static extern void GetCurrentPosition();

        private int _frameCount = 0;
        private LocationInfo _lastLocation = null;
        private double _moveDistance = 0f;

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR   
            UnityEngine.Application.logMessageReceived += (string logString, string stackTrace, LogType type) => {
                LogUtil.OutputConsoleLog($"[{type}] {logString}");
            };
#endif
            
            _latitudeText.text = "latitude: 0";
            _longitudeText.text = "longitude: 0";
            _totalDistance.text = "total distance: 0 m";
            
            // Initialize LocationSystem
            _location =
#if UNITY_WEBGL && !UNITY_EDITOR
                new LocationWeb();  
#else
                new LocationOther();
#endif
            _location.Initialize();
            
            _executeMapFetchButton.onClick.AddListener(() =>
            {
                if (_lastLocation == null) return;

                Task.Run(async () =>
                {
                    var tex = await _mapManager.GetMapTexture(_lastLocation.Latitude, _lastLocation.Longitude);
                    
                    // スプライト（インスタンス）を動的に生成
                    _mapImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                });
            });
        }
        
        private void Update()
        {
            if (_location.State != LocationSystemState.Running)
            {
                Debug.LogWarning("LocationSystem is not running.");
                return;
            }

            // 60フレームごとに現在地を取得する
            if (_frameCount == 60)
            {
                _frameCount = 0;

                Task.Run(async () =>
                {
                    try
                    {
                        var location = await _location.GetLocationAsync();
                        if (location != null)
                        {
                            ShowLocation(location);
                        }
                    }
                    catch(Exception error)
                    {
                        Debug.LogError(error);
                    }
                    finally { }
                });
            }

            _frameCount++;
        }
        
        private void ShowLocation(LocationInfo newLocation)
        {
            _latitudeText.text = $"latitude: {newLocation.Latitude}";
            _longitudeText.text = $"longitude: {newLocation.Longitude}";
            
            if (_lastLocation != null && !_lastLocation.Equals(newLocation))
            {
                var distance = LocationManagerExtension.CalcMoveDistance(
                    _lastLocation.Latitude, _lastLocation.Longitude,
                    newLocation.Latitude, newLocation.Longitude);

                // 移動距離が一定以下の場合破棄不用意に増えないよう破棄
                if (1 <= distance)
                {
                    _moveDistance += distance;
                }

                _totalDistance.text = $"total distance: {_moveDistance} m";
            }
            
            _lastLocation = newLocation;
        }
    }
}