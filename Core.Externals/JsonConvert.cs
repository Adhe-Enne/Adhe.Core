namespace Core.Externals
{
    public static class JsonConvert
    {
        public static string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(obj);
        }

        public static bool IsValid(string jsonString)
        {
            if (jsonString.StartsWith("{") && jsonString.EndsWith("}"))
                return true;

            if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
                return true;

            return false;
        }
    }
}
