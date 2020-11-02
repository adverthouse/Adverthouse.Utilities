using Newtonsoft.Json;

namespace Adverthouse.Core.Configuration
{
    public interface IConfig
    {
        [JsonIgnore]
        string Name => GetType().Name;
    }
}
