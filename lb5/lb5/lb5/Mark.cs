namespace lb5
{
    public class Mark
    {

        public Mark(Student st, Subject sb, int mark)
        {
            Student = st;
            Subject = sb;
            MarkData = mark;
        }

        public int MarkData { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
