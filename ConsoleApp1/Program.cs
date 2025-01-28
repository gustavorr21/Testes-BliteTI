using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

//TODO: Codigo foi criado dentro do visual studio, se rodar o mesmo dentro 
// do visual, funciona corretamente, porem ao testar o mesmo codigo no codebyte
// o mesmo nao funciona, caso queiram apenas copiar esse codigo e testar no visual studio
// caso queiram ver criei um reposotorio com os dois projetos, de json Cleaning e o JWT
// https://github.com/gustavorr21/Testes-BliteTI
public class Program
{
    public static Dictionary<string, object> CleanJsonObject(Dictionary<string, object> json)
    {
        var cleanedJson = new Dictionary<string, object>();

        foreach (var property in json)
        {
            if (property.Value is Dictionary<string, object> nestedObject)
            {
                cleanedJson[property.Key] = CleanJsonObject(nestedObject);
            }
            else if (property.Value is List<object> array)
            {
                var cleanedArray = new List<object>();
                foreach (var item in array)
                {
                    if (item is string strItem && (strItem == "N/A" || strItem == "-" || string.IsNullOrWhiteSpace(strItem)))
                    {
                        continue;
                    }
                    cleanedArray.Add(item);
                }
                cleanedJson[property.Key] = cleanedArray;
            }
            else if (property.Value is string strValue && (strValue == "N/A" || strValue == "-" || string.IsNullOrWhiteSpace(strValue)))
            {
                continue;
            }
            else
            {
                cleanedJson[property.Key] = property.Value;
            }
        }

        return cleanedJson;
    }

    public static async Task Main(string[] args)
    {
        string url = "http://coderbyte.com/api/challenges/json/json-cleaning";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            var cleanedJson = CleanJsonObject(json);

            var cleanedJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(cleanedJson, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(cleanedJsonString);
        }
    }
}