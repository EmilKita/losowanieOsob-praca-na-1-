using System.Diagnostics;
using System.Xml.Linq;

namespace losowanieOsob
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void RemoveStudent(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string studentName)
            {
                foreach (var group in Groups.GroupsList)
                {
                    if (group.Students.Contains(studentName))
                    {
                        group.Students.Remove(studentName); 
                        UpdateXmlFile(); 
                        break;
                    }
                }
            }
        }
        private static void UpdateXmlFile()
        {
            string xmlFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "/test/test.xml");
            XElement GroupsXML = XElement.Load(xmlFilePath);

            GroupsXML.Elements("Group").Remove();

            foreach (var group in Groups.GroupsList)
                {
                    XElement groupElement = new XElement("Group",
                        new XElement("Name", group.GroupName),
                        new XElement("Students", group.Students.Select(s => new XElement("Student", s)))
                    );
                    GroupsXML.Add(groupElement);
                }
            GroupsXML.Save(xmlFilePath);
        }

        private void AddStudent_Clicked(object sender, EventArgs e)
        {
            var selectedGroup = Groups.GroupsList.FirstOrDefault(g => g.IsSelected);
            if (selectedGroup != null && !string.IsNullOrWhiteSpace(newStudentEditor.Text))
            {
                selectedGroup.Students.Add(newStudentEditor.Text);
                OnPropertyChanged(nameof(selectedGroup.Students)); 
                UpdateXmlFile(); 
            }
        }
    }

}
