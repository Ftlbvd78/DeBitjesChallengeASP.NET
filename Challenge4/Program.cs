using System.Text;
using Involved.HTF.Common;
using Involved.HTF.Common.Dto;
using Newtonsoft.Json;

HackTheFutureClient hackTheFutureClient = new HackTheFutureClient();
await hackTheFutureClient.Login("de-bitjes", "405e7e16-8c07-4a2a-a945-0c2b99d0bbb0");

var startApiCall = await hackTheFutureClient.GetAsync("/api/b/medium/start");

Console.WriteLine(startApiCall.Content.ReadAsStringAsync().Result);

var sampleApiCall = await hackTheFutureClient.GetAsync("/api/b/medium/puzzle");

var sampleData = await sampleApiCall.Content.ReadAsStringAsync();

Console.WriteLine(sampleData);

ZyphoraTheWaitingWorldDto waitingWorld = JsonConvert.DeserializeObject<ZyphoraTheWaitingWorldDto>(sampleData);

DateTime startTime = waitingWorld.SendDateTime;

double travelTimeMinutes = (waitingWorld.Distance / (double)waitingWorld.TravelSpeed) * 2;

double travelTimeHours = travelTimeMinutes / 60.0; // Uren
double travelTimePlanetDays = travelTimeHours / waitingWorld.DayLength;

int fullPlanetDays = (int)travelTimePlanetDays;
double remainingHours = (travelTimePlanetDays - fullPlanetDays) * waitingWorld.DayLength;
int fullHours = (int)remainingHours;
double remainingMinutes = (remainingHours - fullHours) * 60;
int fullMinutes = (int)remainingMinutes;
double remainingSeconds = (remainingMinutes - fullMinutes) * 60;
int fullSeconds = (int)remainingSeconds;

DateTime expectedResponseTime = waitingWorld.SendDateTime
    .AddDays(fullPlanetDays);
Console.WriteLine(remainingMinutes);
Console.WriteLine(fullMinutes);
Console.WriteLine(fullSeconds);
if ((fullHours + startTime.Hour) < waitingWorld.DayLength)
{
    expectedResponseTime = expectedResponseTime.AddHours(fullHours);
}
else
{
    while ((fullHours + expectedResponseTime.Hour) < waitingWorld.DayLength)
    {
        expectedResponseTime = expectedResponseTime.AddDays(1).AddHours(fullHours - (fullHours - startTime.Hour));
    }
}

expectedResponseTime = expectedResponseTime.AddMinutes(fullMinutes).AddSeconds(fullSeconds);

Console.WriteLine(expectedResponseTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

var json = JsonConvert.SerializeObject(expectedResponseTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));

var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await hackTheFutureClient.PostAsync("/api/b/medium/puzzle", content);

Console.WriteLine(await response.Content.ReadAsStringAsync());