using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoveCountWatch
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private Text _latitudeText; // 緯度
        [SerializeField] private Text _longitudeText; // 経度
        [SerializeField] private Text _totalDistance; // 移動距離

        [DllImport("__Internal")]
        private static extern void GetCurrentPosition();

        private int _frameCount = 0;
        private LocationInfo _lastLocation = null;
        private double _moveDistance = 0f;

        private void Start()
        {
            _latitudeText.text = "latitude: 0";
            _longitudeText.text = "longitude: 0";
            _totalDistance.text = "total distance: 0 m";
        }
        
        private void Update()
        {
            // 60フレームごとに現在地を取得する
            if (_frameCount == 60)
            {
                // JavaScriptの呼び出し
                GetCurrentPosition();
                _frameCount = 0;
            }

            _frameCount++;
        }

        /// <summary>
        /// JavaScriptから呼び出す関数。
        /// 緯度と経度を画面に表示する。
        /// </summary>
        public void ShowLocation(string location)
        {
            var newLocation = new LocationInfo(location);

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

public class LocationInfo
{
    public double Latitude { get; }
    public double Longitude { get; }

    public LocationInfo(string location)
    {
        var locations = location.Split(',');
        var latitude = double.Parse(locations[0]);
        var longitude = double.Parse(locations[1]);
        
        Latitude = latitude;
        Longitude = longitude;
    }

    public LocationInfo(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public bool Equals(LocationInfo other)
    {
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }
}

public static class LocationManagerExtension
{
    public static double CalcMoveDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // Radius of the Earth in kilometers
        const int R = 6378137;
        
        // Haversine formula
        var dLat = (lat2 - lat1) * (Math.PI / 180);
        var dLon = (lon2 - lon1) * (Math.PI / 180);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = R * c;
        return distance;
    }
}