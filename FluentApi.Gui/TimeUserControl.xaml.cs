using FluentApi.EF;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for TimeUserControl.xaml
    /// </summary>
    public partial class TimeUserControl : UserControl
    {
        protected Model model;
        /// <summary>
        /// The currently selected team.
        /// </summary>
        private Team selectedTeam;
        /// <summary>
        /// The currently selected employee.
        /// </summary>
        private Employee selectedEmployee;

        public TimeUserControl()
        {
            InitializeComponent();
            model = new Model();

            try
            {
                ReloadDataGridTeams();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedTeam != null)
            {
                dataGridEmployees.ItemsSource = selectedTeam.Employees.Where(employee => employee.IsHourlyPaid == true);
            }
        }

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                textBoxEmployeeFirstName.Text = selectedEmployee.FirstName;
                textBoxEmployeeLastName.Text = selectedEmployee.LastName;
                textBoxEmployeeHoursWorked.Text = selectedEmployee.HoursWorked.ToString();
                textBoxEmployeeSalary.Text = selectedEmployee.Salary.ToString();
            }
        }

        //                                      Buttons

        private void Button_EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                if (!Validator.IsMoneyValid(Decimal.Parse(textBoxEmployeeSalary.Text)))
                {
                    MessageBox.Show("Det indtastede penge værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsTimeValid(Decimal.Parse(textBoxEmployeeHoursWorked.Text)))
                {
                    MessageBox.Show("Den indtastede mængde timer er ikke gyldigt. skal være positiv. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
                    {
                        if (Decimal.Parse(textBoxEmployeeSalary.Text) != selectedEmployee.Salary)
                        {
                            selectedEmployee.Salary = Decimal.Parse(textBoxEmployeeSalary.Text);
                        }
                        if (Decimal.Parse(textBoxEmployeeHoursWorked.Text) != selectedEmployee.HoursWorked)
                        {
                            selectedEmployee.HoursWorked = Decimal.Parse(textBoxEmployeeHoursWorked.Text);
                        }
                        model.SaveChanges();
                        dataGridEmployees.ItemsSource = selectedTeam.Employees.Where(employee => employee.IsHourlyPaid == true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere den valgte ansat. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        private void ButtonShowAll_Teams_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReloadDataGridTeams();
                dataGridEmployees.SelectedItem = null;
                dataGridTeams.SelectedItem = null;
                textBoxEmployeeFirstName.Text = String.Empty;
                textBoxEmployeeLastName.Text = String.Empty;
                textBoxEmployeeHoursWorked.Text = String.Empty;
                textBoxEmployeeSalary.Text = String.Empty;
                dataGridEmployees.ItemsSource = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl under forsøget i at vise alle teams. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //                                                  Methods

        /// <summary>
        /// Refils the DataGridEmployees with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Employees is null.</exception>
        private void ReloadDataGridEmployees()
        {
            dataGridEmployees.ItemsSource = model.Employees.Where(employee => employee.IsHourlyPaid == true).Where(employee => employee.TeamId != null).ToList();
        }

        /// <summary>
        /// Refils the DataGridTeams with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Teams is null.</exception>
        private void ReloadDataGridTeams()
        {
            dataGridTeams.ItemsSource = model.Teams.Where(team => team.Employees.Count != 0).Where(team => team.Employees.Where(employee => employee.IsHourlyPaid == true).ToList().Count > 0).ToList();
        }
    }
}
