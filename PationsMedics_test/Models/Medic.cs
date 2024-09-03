namespace PationsMedics_test.Models
{
    public class Medic
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int CabinetId { get; set; }
        public int SpecializationId { get; set; }
        public int? AreaId { get; set; }

        public Cabinet? Cabinet { get; set; }
        public Specialization? Specialization { get; set; }
        public Area? Area { get; set; }
    }
}
