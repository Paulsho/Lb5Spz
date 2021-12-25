using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace lb5
{
    public partial class MainView : Form
    {
        private List<Student> students = new List<Student>();
        private List<Subject> subjects = new List<Subject>();
        private List<Mark> marks = new List<Mark>();

        // прив'язка даних
        BindingSource bsStudents = new BindingSource();
        BindingSource bsSubjects = new BindingSource();
        BindingSource bsMarks = new BindingSource();

        // підготовка до серіалізації/десеріалізації
        XmlSerializer formatStudents = new XmlSerializer(typeof(List<Student>));
        XmlSerializer formatSubjects = new XmlSerializer(typeof(List<Subject>));
        XmlSerializer formatMarks = new XmlSerializer(typeof(List<Mark>));
        public MainView()
        {
            InitializeComponent();

            using (FileStream file = new FileStream("students.xml", FileMode.Open))
            {
                students = formatStudents.Deserialize(file) as List<Student>;
            }
            using (FileStream file = new FileStream("subjects.xml", FileMode.Open))
            {
                subjects = formatSubjects.Deserialize(file) as List<Subject>;
            }
            using (FileStream file = new FileStream("marks.xml", FileMode.Open))
            {
                marks = formatMarks.Deserialize(file) as List<Mark>;
            }

            bsStudents.DataSource = students;
            bsSubjects.DataSource = subjects;
            bsMarks.DataSource = marks;

            dgStudents.DataSource = bsStudents;
            dgSubjects.DataSource = bsSubjects;
            dgMarks.DataSource = bsMarks;
        }

        private void btAddMark_Click(object sender, EventArgs e)
        {
           

            var studQuery = from stud in students
                             where stud.Name == dgStudents.SelectedCells[0].Value.ToString()
                              select stud;

            var subjQuery = from subj in subjects
                             where subj.Name == dgSubjects.SelectedCells[0].Value.ToString()
                              select subj;

            foreach (var stud in studQuery)
            {
                foreach (var subj in subjQuery)
                {
                    marks.Add(new Mark(stud, subj, (int)nMark.Value));
                    bsMarks.ResetBindings(true);
                }
            }
        }

        private void dgStudents_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var result = from item in students
                          where string.IsNullOrEmpty(item.Surname) == false
                           orderby item.Surname
                            select item;

            List<Student> sorted = result.ToList();
            students.Clear();
            students.AddRange(sorted);
            bsStudents.ResetBindings(true);
        }

        private void dgSubjects_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var result = from item in subjects
                          where String.IsNullOrEmpty(item.Name) == false
                           orderby item.Name
                            select item;

            List<Subject> sorted = result.ToList();
            subjects.Clear();
            subjects.AddRange(sorted);
            bsSubjects.ResetBindings(true);
        }

        private void dgMarks_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var result = from item in marks
                          where item.Subject.Name == dgMarks.SelectedCells[0].Value.ToString()
                           select item;

            List<Mark> sorted = result.ToList();
            marks.Clear();
            marks.AddRange(sorted);
            bsMarks.ResetBindings(true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            using (FileStream file = new FileStream("students.xml", FileMode.OpenOrCreate))
            {
                formatStudents.Serialize(file, students);
            }
            using (FileStream file = new FileStream("subjects.xml", FileMode.OpenOrCreate))
            {
                formatSubjects.Serialize(file, subjects);
            }
            using (FileStream file = new FileStream("marks.xml", FileMode.OpenOrCreate))
            {
                formatMarks.Serialize(file, marks);
            }

        }
    }
}
