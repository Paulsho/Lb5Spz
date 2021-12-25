namespace lb5
{
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}