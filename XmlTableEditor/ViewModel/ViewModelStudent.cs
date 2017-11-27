using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlTableEditor.Data;
using System.ComponentModel.DataAnnotations;

namespace XmlTableEditor.ViewModel
{
    class ViewModelStudent : ViewModelBase, IDataErrorInfo
    {
        public Student Model { get; private set; }
        
        public ViewModelStudent()
        {
            Model = new Student();
        }

        public ViewModelStudent(Student model)
        {
            Model = model;
        }

        [Required]
        public int ID
        {
            get => Model.ID;
            set => Model.ID = value;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required Full Name!")]
        [RegularExpression(@"^[\w]+\s[\w]+$", ErrorMessage = "Required First Name and Last Name!")]
        public string FullName
        {
            get
            {
                string fn = Model.FirstName ?? string.Empty;
                string ln = Model.Last ?? string.Empty;

                var sb = new StringBuilder();

                sb.Append(fn);
                if (ln.Equals(string.Empty) == false)
                    sb.Append(" ");
                sb.Append(ln);

                return sb.ToString();
            }
            set
            {
                string[] names = value.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
                if (names.Length < 2)
                    names = new string[] { value };

                Model.FirstName = names.Length > 0 ? names[0] : string.Empty;
                Model.Last = names.Length > 1 ? names[1] : string.Empty;
            }
        }

        [RegularExpression(@"^(1[6-9]|[2-9][\d]|100)[\D]+$", ErrorMessage = "Required value in the range from 16 to 100!")]
        public string Age
        {
            get
            {
                int age;
                if (int.TryParse(Model.Age, out age) == false)
                    return null;

                int year = age % 10;
                string postfix;
                if (year == 1)
                    postfix = " год";
                else if (year > 1 && year < 5)
                    postfix = " года";
                else
                    postfix = " лет";
                return Model.Age.ToString() + postfix;
            }
            set
            {
                string[] s = value.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
                Model.Age = int.TryParse(s[0], out var stub) ? s[0] : null;
            }
        }

        [Required]
        public Gender Gender
        {
            get => (Gender)Model.Gender;
            set => Model.Gender = (int)value;
        }

        string IDataErrorInfo.Error => throw new NotImplementedException();

        string IDataErrorInfo.this[string propertyName] => Validate(propertyName);

        private string Validate(string propertyName)
        {
            var value = GetType().GetProperty(propertyName).GetValue(this, null);
            var results = new List<ValidationResult>();

            var context = new ValidationContext(this, null, null) { MemberName = propertyName };

            if (!Validator.TryValidateProperty(value, context, results))
                return results.First().ErrorMessage;

            return string.Empty;
        }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
