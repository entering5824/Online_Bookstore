using System.Web;
using Newtonsoft.Json;

namespace Online_Bookstore.Utils
{
    public static class SessionExtensions
    {
        public static void SetObject(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = JsonConvert.SerializeObject(value);
        }

        public static T GetObject<T>(this HttpSessionStateBase session, string key)
        {
            var value = session[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value.ToString());
        }
    }
}
