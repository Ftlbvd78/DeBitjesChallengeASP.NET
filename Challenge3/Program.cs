using System.Text;
using System.Text.Json.Serialization;
using Involved.HTF.Common;
using Involved.HTF.Common.Dto;
using Newtonsoft.Json;

HackTheFutureClient hackTheFutureClient = new HackTheFutureClient();
await hackTheFutureClient.Login("de-bitjes", "405e7e16-8c07-4a2a-a945-0c2b99d0bbb0");

var startApiCall = await hackTheFutureClient.GetAsync("/api/a/medium/start");

Console.WriteLine(startApiCall.Content.ReadAsStringAsync().Result);

var sampleApiCall = await hackTheFutureClient.GetAsync("/api/a/medium/puzzle");

var sampleData = await sampleApiCall.Content.ReadAsStringAsync();

Console.WriteLine(sampleData);

BattleOfNovaCentauriDto aliens = JsonConvert.DeserializeObject<BattleOfNovaCentauriDto>(sampleData);

List<Alien> teamA = new List<Alien>(aliens.TeamA);
List<Alien> teamB = new List<Alien>(aliens.TeamB);

foreach (Alien alienA in aliens.TeamA)
{
    foreach (Alien alienB in aliens.TeamB)
    {
        bool turnIsAlienA = checkIfAlienAStarts(alienA, alienB);
        while (alienA.Health > 0 && alienB.Health > 0)
        {
            if (turnIsAlienA)
            {
                alienB.Health -= alienA.Strength;
                turnIsAlienA = false;
            }
            else
            {
                alienA.Health -= alienB.Strength;
                turnIsAlienA = true;
            }
        }
        //removes alien from list when defeated
        if (alienA.Health <= 0)
        {
            teamA.Remove(alienA);
            break;
        }
        else
        {
            teamB.Remove(alienB);
        }
    }
}

var json = JsonConvert.SerializeObject(generateOutput(teamA, teamB));

var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await hackTheFutureClient.PostAsync("/api/a/medium/puzzle", content);

Console.WriteLine(await response.Content.ReadAsStringAsync());

BattleSimulatorSolutionDto generateOutput(List<Alien> teamA, List<Alien> teamB)
{
    string teamWon = "";
    int totalHealth = 0;
    teamA.ForEach(t => totalHealth +=  t.Health);
    if (totalHealth > 0)
    {
        teamWon = "TeamA";
    }
    else
    {
        teamWon = "TeamB";
    }
    teamB.ForEach(t => totalHealth += t.Health);
    return new BattleSimulatorSolutionDto(teamWon, totalHealth);
}

bool checkIfAlienAStarts(Alien alienA, Alien alienB)
{
    bool turnIsAlienA = true;
    if (alienB.Speed > alienA.Speed)
    {
        turnIsAlienA = false;
    }
    return turnIsAlienA;
}