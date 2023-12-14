using Newtonsoft.Json;
namespace HealthyLife.Utility
{
    public static class JsonHelper
    {
        public static string Serialize(object entity)
        {
            try
            {
                return JsonConvert.SerializeObject(entity, SerializationSettings.Instance);
            }
            catch
            {
                return null;
            }
        }

        public static TEntity Deserialize<TEntity>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<TEntity>(data, SerializationSettings.Instance);
            }
            catch
            {
                return default(TEntity);
            }
        }
    }
}