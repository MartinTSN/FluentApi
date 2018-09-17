using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentApi.EF;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for TeamsUserControl.xaml
    /// </summary>
    public partial class TeamsUserControl : UserControl
    {
        protected Model model;
        private Team selectedTeam;
        private Employee selectedEmployee;
        public TeamsUserControl()
        {
            InitializeComponent();
            model = new Model();
            ReloadDataGridTeams();
            ReloadDataGridEmployees();
        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedEmployee != null)
            {
                buttonAddToTeam.IsEnabled = true;
            }
            else
            {
                if (selectedTeam != null)
                {
                    dataGridEmployees.ItemsSource = selectedTeam.Employees;
                    buttonAddToTeam.IsEnabled = false;
                    buttonRemoveFromTeam.IsEnabled = true;
                }
            }
            if (selectedTeam != null)
            {
                try
                {
                    textBoxTeamName.Text = selectedTeam.Name;
                    textBoxDescription.Text = selectedTeam.Description;
                    datePickerStartDate.SelectedDate = selectedTeam.StartDate;
                    datePickerEndDate.SelectedDate = selectedTeam.ExpectedEndDate;
                    textBoxTeamSalary.Text = GetTeamSalary().ToString();
                    buttonEditTeam.IsEnabled = true;
                    buttonAddTeam.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                }

            }
        }

        private decimal GetTeamSalary()
        {
            decimal teamSalary = 0;
            foreach (Employee employee in selectedTeam.Employees)
            {
                teamSalary += employee.Salary;
            }
            return teamSalary;
        }

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
        }

        private void ReloadDataGridTeams()
        {
            try
            {
                dataGridTeams.ItemsSource = model.Teams.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet", e.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void ReloadDataGridEmployees()
        {
            try
            {
                dataGridEmployees.ItemsSource = model.Employees.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet", e.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Button_ShowAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            ReloadDataGridEmployees();
            dataGridTeams.SelectedItem = null;
            dataGridEmployees.SelectedItem = null;
            buttonAddToTeam.IsEnabled = false;
            buttonRemoveFromTeam.IsEnabled = false;
        }

        private void Button_AddToTeam_Click(object sender, RoutedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            selectedTeam = dataGridTeams.SelectedItem as Team;

            if (selectedEmployee != null && selectedTeam != null)
            {
                try
                {
                    foreach (Employee employee in selectedTeam.Employees)
                    {
                        if (selectedEmployee != employee)
                        {
                            selectedTeam.Employees.Add(selectedEmployee);
                            break;
                        }
                    }
                    if (selectedTeam.Employees.Count == 0)
                    {
                        selectedTeam.Employees.Add(selectedEmployee);
                    }
                    model.SaveChanges();
                    dataGridTeams.SelectedItem = selectedTeam;
                    dataGridEmployees.ItemsSource = selectedTeam.Employees;
                    dataGridTeams.SelectedItem = selectedTeam = null;
                    dataGridEmployees.SelectedItem = selectedEmployee = null;
                    buttonAddToTeam.IsEnabled = false;
                    buttonRemoveFromTeam.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_RemoveFromTeam_Click(object sender, RoutedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedEmployee != null && selectedTeam != null)
            {
                try
                {
                    foreach (Employee employee in selectedTeam.Employees)
                    {
                        if (selectedEmployee == employee)
                        {
                            selectedTeam.Employees.Remove(selectedEmployee);
                            break;
                        }
                    }
                    model.SaveChanges();
                    dataGridTeams.SelectedItem = selectedTeam;
                    dataGridEmployees.SelectedItem = selectedEmployee = null;
                    ReloadDataGridEmployees();
                    dataGridEmployees.ItemsSource = selectedTeam.Employees;
                    buttonRemoveFromTeam.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void DataGrid_Teams_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                dataGridTeams.SelectedItem = null;
                buttonAddTeam.IsEnabled = true;
                buttonEditTeam.IsEnabled = false;
                buttonAddToTeam.IsEnabled = false;
                buttonRemoveFromTeam.IsEnabled = false;
                textBoxTeamName.Text = String.Empty;
                textBoxDescription.Text = String.Empty;
                datePickerStartDate.SelectedDate = null;
                datePickerEndDate.SelectedDate = null;
                textBoxTeamName.Focus();
                ReloadDataGridTeams();
            }
        }

        private void Button_AddTeam_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsNameValid(textBoxTeamName.Text))
            {
                MessageBox.Show("Det indtastede navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsDescriptionValid(textBoxDescription.Text))
            {
                MessageBox.Show("Den indtastede beskrivelse er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsStartDateValid(datePickerStartDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede dato er ikke gyldigt. Skal være inden imorgen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsEndDateValid(datePickerEndDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede dato er ikke gyldigt. Skal være efter idag.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    Team newTeam = new Team();
                    newTeam.Name = textBoxTeamName.Text;
                    newTeam.Description = textBoxDescription.Text;
                    newTeam.StartDate = datePickerStartDate.SelectedDate.Value;
                    newTeam.ExpectedEndDate = datePickerEndDate.SelectedDate.Value;
                    model.Teams.Add(newTeam);
                    model.SaveChanges();
                    ReloadDataGridTeams();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at gemme det nye team. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_EditTeam_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTeam != null)
            {
                if (!Validator.IsNameValid(textBoxTeamName.Text))
                {
                    MessageBox.Show("Det indtastede navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsDescriptionValid(textBoxDescription.Text))
                {
                    MessageBox.Show("Den indtastede beskrivelse er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsStartDateValid(datePickerStartDate.SelectedDate.Value))
                {
                    MessageBox.Show("Den indtastede dato er ikke gyldigt. Skal være inden imorgen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsEndDateValid(datePickerEndDate.SelectedDate.Value))
                {
                    MessageBox.Show("Den indtastede dato er ikke gyldigt. Skal være efter idag.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
                    {
                        if (textBoxTeamName.Text != selectedTeam.Name)
                        {
                            selectedTeam.Name = textBoxTeamName.Text;
                        }
                        if (textBoxDescription.Text != selectedTeam.Description)
                        {
                            selectedTeam.Description = textBoxDescription.Text;
                        }
                        if (datePickerStartDate.SelectedDate != selectedTeam.StartDate)
                        {
                            selectedTeam.StartDate = datePickerStartDate.SelectedDate.Value;
                        }
                        if (datePickerEndDate.SelectedDate != selectedTeam.ExpectedEndDate)
                        {
                            selectedTeam.ExpectedEndDate = datePickerEndDate.SelectedDate.Value;
                        }
                        model.SaveChanges();
                        ReloadDataGridTeams();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere det valgte team. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        private void Button_ShowAvailableEmployees_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGridEmployees.ItemsSource = model.Employees.Where(employees => employees.TeamId == null).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}