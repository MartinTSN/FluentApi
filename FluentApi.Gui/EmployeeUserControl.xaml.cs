﻿using FluentApi.EF;
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
                textBoxEmployeeFirstName.Text = selectedEmployee.FirstName;
                textBoxEmployeeLastName.Text = selectedEmployee.LastName;
                datePickerEmployeeBirthday.SelectedDate = selectedEmployee.BirthDay;
                datePickerEmployeeStartDate.SelectedDate = selectedEmployee.EmploymentDate;
                textBoxEmployeeSalary.Text = selectedEmployee.Salary.ToString();
                if (selectedEmployee.ContactInfo != null)
                {
                    textBoxMail.Text = selectedEmployee.ContactInfo.Email;
                    textBoxPhoneNumber.Text = selectedEmployee.ContactInfo.Phone;
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
        {
            dataGridEmployees.ItemsSource = model.Employees.ToList();
        }

        private void Button_Create_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Random random = new Random();
            int odd = 1 + 2 * random.Next(1000,9999);
            Employee newEmployee = new Employee();
            newEmployee.FirstName = textBoxEmployeeFirstName.Text;
            newEmployee.LastName = textBoxEmployeeLastName.Text;
            newEmployee.BirthDay = datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault();
            newEmployee.EmploymentDate = datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault();
            newEmployee.CPRNumber = newEmployee.BirthDay.ToString("dd") + newEmployee.BirthDay.ToString("MM") + newEmployee.BirthDay.ToString("yy") + "-" + odd;
            newEmployee.Salary = Decimal.Parse(textBoxEmployeeSalary.Text);
            model.Employees.Add(newEmployee);
            model.SaveChanges();
            ReloadDataGridEmployees();
        }

        private void Button_Update_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (textBoxEmployeeFirstName.Text != selectedEmployee.FirstName)
                {
                    selectedEmployee.FirstName = textBoxEmployeeFirstName.Text;
                }
                if (textBoxEmployeeLastName.Text != selectedEmployee.LastName)
                {
                    selectedEmployee.LastName = textBoxEmployeeLastName.Text;
                }
                if (datePickerEmployeeBirthday.SelectedDate != selectedEmployee.BirthDay)
                {
                    selectedEmployee.BirthDay = datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault();
                }
                if (datePickerEmployeeStartDate.SelectedDate != selectedEmployee.EmploymentDate)
                {
                    selectedEmployee.EmploymentDate = datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault();
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
                    textBoxEmployeeFirstName.Text = String.Empty;
                    textBoxEmployeeLastName.Text = String.Empty;
                    datePickerEmployeeStartDate.SelectedDate = null;
                    textBoxEmployeeFirstName.Focus();
                }
            }
        }

        private void TextBox_EmployeeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxEmployeeFirstName.Text == String.Empty)
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
        private void TextBox_EmployeeLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxEmployeeLastName.Text == String.Empty)
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
            if (textBoxMail.Text == String.Empty)
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

        private void TextBox_PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxPhoneNumber.Text == String.Empty)
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
