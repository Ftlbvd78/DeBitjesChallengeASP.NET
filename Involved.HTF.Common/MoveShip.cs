namespace Involved.HTF.Common;

public class MoveShip
{
    public int angle { get; set; }
    public string speed { get; set; }

    public MoveShip(int angle, string speed)
    {
        this.angle = angle;
        this.speed = speed;
    }
}