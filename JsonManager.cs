using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonMini.JsonMini;

/*
 * Module: JsonMini (C#) (For BlazeEngine)
 * Discord: BlazeBest#4974
 * https://github.com/BlazeBest/JsonMini/
 * https://discord.gg/8mMGM43
 * ---
 * Comment: under development
 */

namespace JsonMini
{
    public class JsonManager
    {
        public static T ReadData<T>(JsonData data) => data.ReadData<T>();

        public static Dictionary<string, JsonData> Reader(string file)
        {
            if (!File.Exists(file))
                throw new Exception("Not found file!");

            Dictionary<string, JsonData> result = new Dictionary<string, JsonData>();
            // {"Test1":false,"Test2":1,"Test3":"Text"}
            //string source = string.Empty;
            string source = WorkingOnHead("{\"Test1\":false,\"Test2\":1,\"Test3\":\"Text\"}");
            
            while (!string.IsNullOrEmpty(source))
            {
                source = source.Trim();

                string[] resource = source.Split(new char[] { ':' }, 1);
                if (resource.Length != 1)
                    throw new Exception("Fail to read json data [3]");
                source = resource[1];

                if (!source.StartsWith("\""))
                    throw new Exception("Fail to read json data [4]");
                resource[0] = resource[0].Remove(0, resource[0].IndexOf('"')+1);

                if (!source.EndsWith("\""))
                    throw new Exception("Fail to read json data [5]");
                resource[0] = resource[0].Remove(resource[0].LastIndexOf('"'));

                JsonType type = JsonType.None;
                object obj = null;
                if (resource[1].StartsWith("{"))
                {
                    type = JsonType.Array;
                }
                else
                {
                    string res = resource[1].Remove(resource[1].IndexOf(','));
                    source = source.Remove(0, resource[1].IndexOf(',')+1);

                    UnboxText(res, out JsonData data)
                }
                result.Add(resource[0].Trim(), new JsonData(type, obj));
            }

            return result;
        }

        internal static string WorkingOnHead(string text)
        {
            string result = text.Trim();

            if (result[0] != '{')
                throw new Exception("Fail to read json data [1]");
            result = result.Remove(0, 1);

            if (result[result.Length - 1] != '}')
                throw new Exception("Fail to read json data [2]");
            result = result.Remove(result.Length - 1, 1);

            result = result.Replace("\0", "");
            result = result.Replace("\n", "");
            result = result.Replace("\r", "");
            result = result.Replace("\t", "");

            return result;
        }

        internal static bool UnboxText(string text, out JsonData data)
        {
            text = text.Trim();
            data = new JsonData();
            data.type = JsonType.None;
            if (text.Contains("null"))
            {
                data.type = JsonType.Object;
                data.data = null;
            }
            else if (text.Contains("true"))
            {
                data.type = JsonType.Boolean;
                data.data = true;
            }
            else if (text.Contains("false"))
            {
                data.type = JsonType.Boolean;
                data.data = false;
            }
            else if (text[0] == '"' && text[text.Length - 1] == '"' && text.Length > 1)
            {
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1, 1);

                data.type = JsonType.String;
                data.data = text;
            }
            else if (text.IndexOf('.') < text.Length)
            {
                data.type = JsonType.Double;
                data.data = double.Parse(text);
            }
            return data.type != JsonType.None;
        }
    }
}
