using FluentApi.EF;
using System;
using System.Linq;
using System.Windows;
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
            try
            {
                ReloadDataGridEmployees();
            }
            catch (Exception e)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet", e.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                try
                {
                    buttonUpdateEmployee.IsEnabled = true;
                    buttonAddEmployee.IsEnabled = false;
                    textBoxEmployeeFirstName.Text = selectedEmployee.FirstName;
                    textBoxEmployeeLastName.Text = selectedEmployee.LastName;
                    datePickerEmployeeBirthday.SelectedDate = selectedEmployee.BirthDay;
                    datePickerEmployeeStartDate.SelectedDate = selectedEmployee.EmploymentDate;
                    textBoxEmployeeSalary.Text = selectedEmployee.Salary.ToString();
                    textBoxCPR.Text = selectedEmployee.CPRNumber.Substring(selectedEmployee.CPRNumber.Length - 4);
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
                catch (Exception)
                {
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at vælge personen. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void ReloadDataGridEmployees()
        {
            dataGridEmployees.ItemsSource = model.Employees.ToList();
        }

        private void Button_Create_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!Validator.IsNameValid(textBoxEmployeeFirstName.Text))
            {
                MessageBox.Show("Det indtastede navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsNameValid(textBoxEmployeeLastName.Text))
            {
                MessageBox.Show("Det indtastede navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsBirthDayValid(datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault()))
            {
                MessageBox.Show("Den indtastede dato er ikke gyldigt. Man må kun være 18-70 år gammel. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsEmploymentDateValid(datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault()))
            {
                MessageBox.Show("Den indtastede dato er ikke gyldigt. Må kun være efter år 1950 og inden imorgen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsBirthDayEmploymentDateValid(datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault(), datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault()))
            {
                MessageBox.Show("Den indtastede dato er ikke gyldigt. Skal være efter den valgte fødselsdag. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsCPRNumberValid(textBoxCPR.Text))
            {
                MessageBox.Show("Det indtastede Nummer er ikke gyldigt. Må kun indeholde tal. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsMoneyValid(Decimal.Parse(textBoxEmployeeSalary.Text)))
            {
                MessageBox.Show("Det indtastede værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    Employee newEmployee = new Employee();
                    newEmployee.FirstName = textBoxEmployeeFirstName.Text;
                    newEmployee.LastName = textBoxEmployeeLastName.Text;
                    newEmployee.BirthDay = datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault();
                    newEmployee.EmploymentDate = datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault();
                    newEmployee.CPRNumber = newEmployee.BirthDay.ToString("dd") + newEmployee.BirthDay.ToString("MM") + newEmployee.BirthDay.ToString("yy") + "-" + textBoxCPR;
                    newEmployee.Salary = Decimal.Parse(textBoxEmployeeSalary.Text);
                    model.Employees.Add(newEmployee);
                    model.SaveChanges();
                    ReloadDataGridEmployees();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at gemme den nye ansatte. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_Update_Employee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (!Validator.IsNameValid(textBoxEmployeeFirstName.Text))
                {
                    MessageBox.Show("Det indtastede navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsNameValid(textBoxEmployeeLastName.Text))
                {
                    MessageBox.Show("Det indtastede navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsBirthDayValid(datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault()))
                {
                    MessageBox.Show("Den indtastede dato er ikke gyldigt. Man må kun være 18-70 år gammel. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsEmploymentDateValid(datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault()))
                {
                    MessageBox.Show("Den indtastede dato er ikke gyldigt. Må kun være efter år 1950 og inden imorgen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsBirthDayEmploymentDateValid(datePickerEmployeeBirthday.SelectedDate.GetValueOrDefault(), datePickerEmployeeStartDate.SelectedDate.GetValueOrDefault()))
                {
                    MessageBox.Show("Den indtastede dato er ikke gyldigt. Skal være efter den valgte fødselsdag. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsCPRNumberValid(textBoxCPR.Text))
                {
                    MessageBox.Show("Det indtastede Nummer er ikke gyldigt. Må kun indeholde tal. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsMoneyValid(Decimal.Parse(textBoxEmployeeSalary.Text)))
                {
                    MessageBox.Show("Det indtastede værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
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
                        if (textBoxCPR.Text != selectedEmployee.CPRNumber.Substring(selectedEmployee.CPRNumber.Length - 4))
                        {
                            selectedEmployee.CPRNumber = selectedEmployee.BirthDay.ToString("dd") + selectedEmployee.BirthDay.ToString("MM") + selectedEmployee.BirthDay.ToString("yy") + "-" + textBoxCPR;
                        }
                        model.SaveChanges();
                        ReloadDataGridEmployees();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere den valgte ansat. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        private void Button_Update_ContactInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (!Validator.IsEmailValid(textBoxMail.Text))
                {
                    MessageBox.Show("Den indtastede mail er ikke gyldigt. Skal have @, en domæneslutning og noget i mellem de 2. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsPhoneValid(textBoxPhoneNumber.Text))
                {
                    MessageBox.Show("Det indtastede Nummer er ikke gyldigt. Må kun bestå af tal og skal være over 8 cifre lang. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
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
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere den Kontaktinformationen. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        private void Button_Add_ContactInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!Validator.IsEmailValid(textBoxMail.Text))
            {
                MessageBox.Show("Den indtastede mail er ikke gyldigt. Skal have @, en domæneslutning og noget i mellem de 2. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsPhoneValid(textBoxPhoneNumber.Text))
            {
                MessageBox.Show("Det indtastede Nummer er ikke gyldigt. Må kun bestå af tal og skal være over 8 cifre lang. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    ContactInfo newContactInfo = new ContactInfo();
                    newContactInfo.Email = textBoxMail.Text;
                    newContactInfo.Phone = textBoxPhoneNumber.Text;
                    model.ContactInfos.Add(newContactInfo);
                    selectedEmployee.ContactInfo = newContactInfo;
                    model.SaveChanges();
                    ReloadDataGridEmployees();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at gemme den nye Kontaktinformation. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
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
                    datePickerEmployeeBirthday.SelectedDate = null;
                    textBoxCPR.Text = String.Empty;
                    textBoxEmployeeSalary.Text = String.Empty;
                    buttonAddEmployee.IsEnabled = true;
                    textBoxEmployeeFirstName.Focus();
                }
            }
        }

        private void TextBox_EmployeeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedEmployee != null)
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
            if (selectedEmployee != null)
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
        }

        private void TextBox_PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedEmployee != null)
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

        private void TextBox_EmployeeSalary_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (textBoxEmployeeSalary.Text == String.Empty)
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

        private void TextBox_CPR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (textBoxCPR.Text == String.Empty)
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
}
