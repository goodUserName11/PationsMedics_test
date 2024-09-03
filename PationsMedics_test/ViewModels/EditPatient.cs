namespace PationsMedics_test.ViewModels
{
    public class EditPatient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public int AreaId { get; set; }
    }
}
