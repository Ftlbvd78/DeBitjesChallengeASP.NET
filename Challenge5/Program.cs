using Involved.HTF.Common;

HackTheFutureClient hackTheFutureClient = new HackTheFutureClient();
await hackTheFutureClient.Login("de-bitjes", "405e7e16-8c07-4a2a-a945-0c2b99d0bbb0");

var startApiCall = await hackTheFutureClient.GetAsync("/api/b/hard/start");

Console.WriteLine(startApiCall.Content.ReadAsStringAsync().Result);

var sampleApiCall = await hackTheFutureClient.GetAsync("/api/b/hard/sample");

var sampleData = await sampleApiCall.Content.ReadAsStringAsync();

Console.WriteLine(sampleData);

string [] XYAxis = sampleData.Split(',');

Console.WriteLine(XYAxis[0]);