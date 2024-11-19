using System.Text;
using System.Text.Json.Serialization;
using Involved.HTF.Common;
using Involved.HTF.Common.Dto;
using Newtonsoft.Json;

HackTheFutureClient hackTheFutureClient = new HackTheFutureClient();
await hackTheFutureClient.Login("de-bitjes", "405e7e16-8c07-4a2a-a945-0c2b99d0bbb0");


for (int i = 0; i < 2; i++)
{
    MoveShip move = new MoveShip(10, "M");

    var json = JsonConvert.SerializeObject(move);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await hackTheFutureClient.PostAsync("/api/Team/move", content);

    Console.WriteLine(response.StatusCode);
    
    
}

