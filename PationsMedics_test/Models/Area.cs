namespace PationsMedics_test.Models
{
    public class Area
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public List<Patient>? Patients { get; set; }
        public List<Medic>? Medics { get; set; }
    }
}
