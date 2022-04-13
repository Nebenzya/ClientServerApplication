namespace Server
{
    public class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public int Course { get; set; }

        public string FullInfo 
        { 
            get 
            {
                return $"{FirstName} {LastName} {BirthYear} {Course}";
            } 
        }
    }
}
