namespace Involved.HTF.Common.Dto;

public class BattleSimulatorSolutionDto
{
    public string winningTeam { get; set; }
    public int remainingHealth { get; set; }

    public BattleSimulatorSolutionDto(string winningTeam, int remainingHealth)
    {
        this.winningTeam = winningTeam;
        this.remainingHealth = remainingHealth;
    }
}