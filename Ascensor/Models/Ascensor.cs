namespace Ascensor.Models
{
    public class Ascensor
    {
        public int CurrentFloor { get; set; } = 1;
        public List<int> TotalFloors { get; set; } = new List<int> {1};

    }
}
