using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.JSON
{
    public class JsonDataManager
    {
        public string Path
        {
            get; private set;
        }

        public JsonDataManager(string path)
        {
            Path = path;
        }

        public void SaveDataInJson(IEnumerable<object> dataArray)
        {
            var serializer = new JsonSerializer();
            using (var streamWriter = new StreamWriter(Path))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, dataArray);
                }
            }
        }

        public void SaveDataInJson(object data)
        {
            var serializer = new JsonSerializer();
            using (var streamWriter = new StreamWriter(Path, false))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, data);
                }
            }
        }

        public T LoadDataFromJson<T>()
        {
            var serializer = new JsonSerializer();
            using (var streamReader = new StreamReader(Path))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}