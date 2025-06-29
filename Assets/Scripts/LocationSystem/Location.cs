using System.Threading.Tasks;

namespace MoveCountWatch
{
    public interface ILocation
    {
        public LocationSystemState State { get; }
        
        public bool IsGettingLocation { get; }
        
        public void Initialize();
        
        public ValueTask<LocationInfo> GetLocationAsync();
    }

    public enum LocationSystemState
    {
        None,       // 未初期化
        Initialize, // 初期化中
        Running,    // 実行中
        NotRunning, // 実行が許可されなかった
    }
}