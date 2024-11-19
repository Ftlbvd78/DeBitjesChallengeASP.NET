using System.Text;
using Involved.HTF.Common;
using System.Text.Json;

HackTheFutureClient hackTheFutureClient = new HackTheFutureClient();
await hackTheFutureClient.Login("de-bitjes", "405e7e16-8c07-4a2a-a945-0c2b99d0bbb0");

var startApiCall = await hackTheFutureClient.GetAsync("/api/a/easy/start");

Console.WriteLine(startApiCall.Content.ReadAsStringAsync().Result);

var sampleApiCall = await hackTheFutureClient.GetAsync("/api/a/easy/puzzle");

var sampleData = await sampleApiCall.Content.ReadAsStringAsync();

string[] sampleDataArray = sampleData.Split(',');


int currentDepth = 0;
int depth = 0;
int distance = 0;
foreach (string sampleDataString in sampleDataArray)
{
    string command = "";
    if (sampleDataString.Contains("commands"))
    {
        string filteredSampleDataString = sampleDataString.Substring(13);
        command = filteredSampleDataString;
        Console.WriteLine(filteredSampleDataString);
    }
    else if (sampleDataString.Contains("}"))
    {
        string filteredSampleDataString = sampleDataString.Substring(0, sampleDataString.Length - 2);
        command = filteredSampleDataString;
        Console.WriteLine(filteredSampleDataString);
    }
    else
    {
        command = sampleDataString;
        Console.WriteLine(sampleDataString);
    }

    switch (command.Substring(0, 1))
    {
        case "U":
            up(getNumberFromCommand(command));
            break;
        case "D":
            down(getNumberFromCommand(command));
            break;
        case "F":
            forward(getNumberFromCommand(command));
            break;
    }

    Console.WriteLine(" current depth: " + currentDepth);
    Console.WriteLine(" depth: " + depth);
    Console.WriteLine(" distance: " + distance);
}


int answer = currentDepth * distance;

var jsonString = JsonSerializer.Serialize(answer);

var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

var response = await hackTheFutureClient.PostAsync("/api/a/easy/puzzle", content);

Console.WriteLine(await response.Content.ReadAsStringAsync());

int getNumberFromCommand(string command)
{
    string number = "";
    foreach (var character in command.ToCharArray())
    {
        if (char.IsDigit(character))
        {
            number += character;
        }
    }

    return int.Parse(number);
}

void up(int n)
{
    depth -= n;
}

void down(int n)
{
    depth += n;
}

void forward(int n)
{
    currentDepth = currentDepth + (n * depth);

    distance += n;
}