using FluentApi.EF;

using System.Linq;
using System.Windows.Controls;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for EmployeeUserControl.xaml
    /// </summary>
    public partial class EmployeeUserControl : UserControl
    {
        protected Model model;
        private Employee selectedEmployee;

        public EmployeeUserControl()
        {
            InitializeComponent();
            model = new Model();
            dataGridEmployees.ItemsSource = model.Employees.ToList();
        }

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            textBoxEmployeeName.Text = selectedEmployee.Name;
            datePickerEmployeeStartDate.SelectedDate = selectedEmployee.EmploymentDate;
            if (selectedEmployee.ContactInfo != null)
            {
                textBoxMail.Text = selectedEmployee.ContactInfo.Email;
                textBoxPhoneNumber.Text = selectedEmployee.ContactInfo.Phone;
            }
            else
            {
                textBoxMail.Text = string.Empty;
                textBoxPhoneNumber.Text = string.Empty;
                buttonUpdateContactInfo.IsEnabled = false;
            }

        }

        private void Button_Create_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Employee newEmployee = new Employee();
            newEmployee.Name = textBoxEmployeeName.Text;
            model.Employees.Add(newEmployee);
            model.SaveChanges();
            dataGridEmployees.ItemsSource = model.Employees.ToList();

        }

        private void Button_Update_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Button_Update_ContactInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void Button_Add_ContactInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ContactInfo newContactInfo = new ContactInfo();
            newContactInfo.Email = textBoxMail.Text;
            newContactInfo.Phone = textBoxPhoneNumber.Text;
            model.ContactInfos.Add(newContactInfo);
            model.SaveChanges();
        }
    }
}
