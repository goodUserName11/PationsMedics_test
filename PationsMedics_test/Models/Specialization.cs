namespace PationsMedics_test.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Medic>? Medics { get; set; }
    }
}
