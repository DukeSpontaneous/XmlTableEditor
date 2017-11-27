using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XmlTableEditor.Data;

namespace XmlTableEditor.Data
{
    public class Students
    {
        Students()
        {
            List = new List<Student>();
        }

        Students(IEnumerable<Student> students)
        {
            List = students.ToList();
        }

        [XmlElement("Student")]
        public List<Student> List { get; set; }

        public static IEnumerable<Student> LoadFile(string path)
        {
            using (var file = File.OpenRead(path))
            {
                var students = (Students)new XmlSerializer(typeof(Students)).Deserialize(file);
                Student.ResetPoolID(students.List.Select(s => s.ID));
                return students.List;
            }            
        }

        public static void SaveFile(string path, IEnumerable<Student> students)
        {
            var nameless = new XmlSerializerNamespaces();
            nameless.Add(string.Empty, string.Empty);

            using (var file = File.CreateText(path))
                new XmlSerializer(typeof(Students)).Serialize(file, new Students(students), nameless);
        }        
    }
}
