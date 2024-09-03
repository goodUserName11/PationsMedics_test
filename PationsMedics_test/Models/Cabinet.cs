namespace PationsMedics_test.Models
{
    public class Cabinet
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public List<Medic>? Medics { get; set; }
    }
}
