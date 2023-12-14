using Newtonsoft.Json;

namespace HealthyLife.Utility
{
    public class SerializationSettings
    {
        public static JsonSerializerSettings Instance
        {
            get
            {
                return new JsonSerializerSettings();
            }
        }
    }
}