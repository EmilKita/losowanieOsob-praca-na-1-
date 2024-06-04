using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace losowanieOsob
{
    public class Groups : INotifyPropertyChanged
    {
        public string GroupName { get; set; }

        private ObservableCollection<string> _students;
        public ObservableCollection<string> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static string path = Path.Combine(FileSystem.Current.AppDataDirectory, "/test/test.xml");

        public static ObservableCollection<Groups> GroupsList { get; set; } = new ObservableCollection<Groups>();

        static Groups()
        {
            LoadGroupsFromXML();
        }

        public static void LoadGroupsFromXML()
        {
            XElement GroupsXML = XElement.Load(path);

            foreach (XElement groupElement in GroupsXML.Elements("Group"))
            {
                string groupName = groupElement.Element("Name").Value;

                var students = groupElement.Element("Students")
                        .Elements("Student")
                        .Select(s => s.Value)
                        .ToArray();

                GroupsList.Add(new Groups(groupName, students));
            }
        }

        public Groups(string groupName, string[] students)
        {
            GroupName = groupName;
            Students = new ObservableCollection<string>(students);
        }

        public static void AddGroup(string groupName, string[] students)
        {
            GroupsList.Add(new Groups(groupName, students));
        }
    }
}
