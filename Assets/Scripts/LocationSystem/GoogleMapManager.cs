using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MoveCountWatch
{
    public class GoogleMapManager
    {
        public ValueTask<Texture2D> GetMapTexture(double latitude, double longitude)
        {
            return GetMapTextureWithGoogle(latitude, longitude);
        }

        private async ValueTask<Texture2D> GetMapTextureWithGoogle(double latitude, double longitude)
        {
            const string GoogleApiKey = "AIzaSyA5dQSvqDAhUkyApTeLb1gB9afwWcgbnjM";
            
            // ベース URL
            var url = @"https://maps.googleapis.com/maps/api/staticmap?";
            // 中心座標
            url += "center=" + latitude + "," + longitude;
            // ズーム
            url += "&zoom=" + 18; // デフォルトで 0 なので適当なサイズにしておく
            // 画像サイズ（640x640まで）
            url += "&size=" + 640 + "x" + 640;
            // API Key（Google Maps Platform で発行されるキー）
            url += "&key=" + GoogleApiKey;
            
            url = UnityWebRequest.UnEscapeURL(url);
            var req = UnityWebRequestTexture.GetTexture(url);
            await req.SendWebRequest();
            
            var texture = new Texture2D(1024, 1024);
            texture.LoadImage(req.downloadHandler.data);
            return texture;
        }
    }
}
