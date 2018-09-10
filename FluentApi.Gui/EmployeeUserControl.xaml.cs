using FluentApi.EF;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

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
            if (selectedEmployee != null)
            {
                buttonUpdateEmployee.IsEnabled = true;
                buttonAddEmployee.IsEnabled = false;
                textBoxEmployeeName.Text = selectedEmployee.Name;
                if (selectedEmployee.ContactInfo != null)
                {
                    textBoxMail.Text = selectedEmployee.ContactInfo.Email;
                    textBoxPhoneNumber.Text = selectedEmployee.ContactInfo.Phone;
                    datePickerEmployeeStartDate.SelectedDate = selectedEmployee.EmploymentDate;
                }
                else
                {
                    textBoxMail.Text = String.Empty;
                    textBoxPhoneNumber.Text = String.Empty;
                    buttonUpdateContactInfo.IsEnabled = false;
                }
            }
        }

        private void ReloadDataGridEmployees()
            => dataGridEmployees.ItemsSource = model.Employees.ToList();

        private void Button_Create_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Employee newEmployee = new Employee();
            newEmployee.Name = textBoxEmployeeName.Text;
            model.Employees.Add(newEmployee);
            model.SaveChanges();
            ReloadDataGridEmployees();
        }

        private void Button_Update_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (textBoxEmployeeName.Text != selectedEmployee.Name)
                {
                    selectedEmployee.Name = textBoxEmployeeName.Text;
                }
                if (datePickerEmployeeStartDate.SelectedDate != selectedEmployee.EmploymentDate)
                {
                    selectedEmployee.EmploymentDate = Convert.ToDateTime(datePickerEmployeeStartDate.SelectedDate);
                }
                model.SaveChanges();
                ReloadDataGridEmployees();
            }
        }
        private void Button_Update_ContactInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (textBoxMail.Text != selectedEmployee.ContactInfo.Email)
                {
                    selectedEmployee.ContactInfo.Email = textBoxMail.Text;
                }
                if (textBoxPhoneNumber.Text != selectedEmployee.ContactInfo.Phone)
                {
                    selectedEmployee.ContactInfo.Phone = textBoxPhoneNumber.Text;
                }
                model.SaveChanges();
                ReloadDataGridEmployees();
            }
        }

        private void Button_Add_ContactInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ContactInfo newContactInfo = new ContactInfo();
            newContactInfo.Email = textBoxMail.Text;
            newContactInfo.Phone = textBoxPhoneNumber.Text;
            model.ContactInfos.Add(newContactInfo);
            selectedEmployee.ContactInfo = newContactInfo;
            model.SaveChanges();
            ReloadDataGridEmployees();
        }

        private void DataGrid_Employees_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (dataGridEmployees.SelectedItem != null)
            {
                if (e.Key == Key.Escape)
                {
                    dataGridEmployees.SelectedItem = selectedEmployee = null;
                    buttonAddEmployee.IsEnabled = true;
                    buttonUpdateEmployee.IsEnabled = false;
                    textBoxEmployeeName.Text = string.Empty;
                    datePickerEmployeeStartDate.SelectedDate = null;
                    textBoxEmployeeName.Focus();
                }
            }
        }

        private void TextBox_EmployeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxEmployeeName.Text == string.Empty)
            {
                buttonAddEmployee.IsEnabled = false;
                buttonUpdateEmployee.IsEnabled = false;
            }
            else if (selectedEmployee == null)
            {
                buttonAddEmployee.IsEnabled = true;
                buttonUpdateEmployee.IsEnabled = false;
            }
            else
            {
                buttonAddEmployee.IsEnabled = false;
                buttonUpdateEmployee.IsEnabled = true;
            }
        }

        private void TextBox_Mail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxMail.Text == string.Empty)
            {
                buttonAddContactInfo.IsEnabled = false;
                buttonUpdateContactInfo.IsEnabled = false;
            }
            else if (selectedEmployee.ContactInfo == null)
            {
                buttonAddContactInfo.IsEnabled = true;
                buttonUpdateContactInfo.IsEnabled = false;
            }
            else
            {
                buttonAddContactInfo.IsEnabled = false;
                buttonUpdateContactInfo.IsEnabled = true;
            }
        }
    }
}
