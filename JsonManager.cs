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
                throw new FileNotFoundException();

            Dictionary<string, JsonData> result = new Dictionary<string, JsonData>();
            // {"Test1":false,"Test2":1,"Test3":"Text"}
            //string source = string.Empty;
            string source = WorkingOnHead("{\"Test1\":false,\"Test2\":1,\"Test3\":\"Text\"}");
            string[] args = source.Split(',');
            foreach(string arg in args)
            {
                string buffer = arg.Trim();
                string[] content = buffer.Split(new char[] { ':' }, 1);
                if (content.Length < 2)
                    continue;

                if (content[0].Length < 1 && content[1].Length < 1)
                    continue;

                // [0] ~ String Name
                if (content[0][0] != '"')
                    throw new FileLoadException();
                content[0] = content[0].Remove(0, 1);
               
                if (content[0][content[0].Length - 1] != '"')
                    throw new FileLoadException();
                content[0] = content[0].Remove(content[0].Length - 1, 1);
                // ~ ~ ~ ~ ~ ~ ~ ~ ~ ~

                // [1] ~ Source
                if (!UnboxText(content[1], out JsonData data))
                    continue;
                // ~ ~ ~ ~ ~ ~ ~ ~ ~ ~

                result.Add(content[0], data);
            }

            return result;
        }

        internal static string WorkingOnHead(string text)
        {
            string result = text.Trim();

            if (result[0] != '{')
                throw new FileLoadException();
            result = result.Remove(0, 1);

            if (result[result.Length - 1] != '}')
                throw new FileLoadException();
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
