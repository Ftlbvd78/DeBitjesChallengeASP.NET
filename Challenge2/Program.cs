using System.Text;
using System.Text.Json;
using Involved.HTF.Common;

HackTheFutureClient hackTheFutureClient = new HackTheFutureClient();
await hackTheFutureClient.Login("de-bitjes", "405e7e16-8c07-4a2a-a945-0c2b99d0bbb0");

var startApiCall = await hackTheFutureClient.GetAsync("/api/b/easy/start");

Console.WriteLine(startApiCall.Content.ReadAsStringAsync().Result);

var sampleApiCall = await hackTheFutureClient.GetAsync("/api/b/easy/puzzle");

var alphabetApiCall = await hackTheFutureClient.GetAsync("/api/b/easy/alphabet");

var sampleData = await sampleApiCall.Content.ReadAsStringAsync();
sampleData = sampleData.Replace("\"", "").Replace("{", "").Replace("}", "").Replace(":", "")
    .Replace("alienMessage", "");

var alphabetData = await alphabetApiCall.Content.ReadAsStringAsync();
alphabetData = alphabetData.Replace("\"", "").Replace("{", "").Replace("}", "");
string[] test = alphabetData.Split(",");


List<KeyValuePair<string, string>> alphabetDataList = new List<KeyValuePair<string, string>>();
foreach (var pair in test)
{
    KeyValuePair<string, string> pairValue = new KeyValuePair<string, string>(pair.Split(':')[0], pair.Split(':')[1]);
    alphabetDataList.Add(pairValue);
    Console.WriteLine(pairValue);
}

StringBuilder stringBuilder = new StringBuilder();
bool letterFound = false;
foreach (var character in sampleData.ToCharArray())
{
    foreach (var pair in alphabetDataList)
    {
        if (pair.Value == character.ToString())
        {
            stringBuilder.Append(pair.Key);
            letterFound = true;
        }
    }

    if (!letterFound)
    {
        stringBuilder.Append(character);
    }

    letterFound = false;
}

Console.WriteLine(stringBuilder.ToString());

var jsonString = JsonSerializer.Serialize(stringBuilder.ToString());

var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

var response = await hackTheFutureClient.PostAsync("/api/b/easy/puzzle", content);

Console.WriteLine(await response.Content.ReadAsStringAsync());