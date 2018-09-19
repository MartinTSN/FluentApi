using FluentApi.EF;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for TeamsUserControl.xaml
    /// </summary>
    public partial class TeamsUserControl : UserControl
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
        public TeamsUserControl()
        {
            InitializeComponent();
            model = new Model();
            try
            {
                ReloadDataGridTeams();
                ReloadDataGridEmployees();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //                              DataGrid Events

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            if (selectedTeam != null)
            {
                buttonAddToTeam.IsEnabled = true;
            }
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
                    textBoxTeamSalary.Text = SetTeamSalary().ToString();
                    buttonEditTeam.IsEnabled = true;
                    buttonAddTeam.IsEnabled = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

            }
        }

        private void DataGrid_Teams_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridTeams.SelectedItem != null)
            {
                if (e.Key == Key.Escape)
                {
                    try
                    {
                        ReloadDataGridTeams();
                        ReloadDataGridEmployees();
                        dataGridTeams.SelectedItem = null;
                        dataGridEmployees.SelectedItem = null;
                        textBoxTeamName.Focus();
                        //                                          TextBoxes
                        textBoxTeamName.Text = String.Empty;
                        textBoxDescription.Text = String.Empty;
                        textBoxTeamSalary.Text = String.Empty;
                        //                                           Datepicker
                        datePickerStartDate.SelectedDate = null;
                        datePickerEndDate.SelectedDate = null;
                        //                                          Buttons
                        buttonAddTeam.IsEnabled = true;
                        buttonEditTeam.IsEnabled = false;
                        buttonAddToTeam.IsEnabled = false;
                        buttonRemoveFromTeam.IsEnabled = false;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        //                              Button Events

        private void Button_AddTeam_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsNameValid(textBoxTeamName.Text))
            {
                MessageBox.Show("Det indtastede Team-navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsDescriptionValid(textBoxDescription.Text))
            {
                MessageBox.Show("Den indtastede Beskrivelse er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsStartDateValid(datePickerStartDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede Start-dato er ikke gyldigt. Skal være inden imorgen. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsEndDateValid(datePickerEndDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede Slut-dato er ikke gyldigt. Skal være efter idag. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at gemme det nye team. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_EditTeam_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTeam != null)
            {
                if (!Validator.IsNameValid(textBoxTeamName.Text))
                {
                    MessageBox.Show("Det indtastede Team-navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsDescriptionValid(textBoxDescription.Text))
                {
                    MessageBox.Show("Den indtastede beskrivelse er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsStartDateValid(datePickerStartDate.SelectedDate.Value))
                {
                    MessageBox.Show("Den indtastede Start-dato er ikke gyldigt. Skal være inden imorgen. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!Validator.IsEndDateValid(datePickerEndDate.SelectedDate.Value))
                {
                    MessageBox.Show("Den indtastede Slut-dato er ikke gyldigt. Skal være efter idag. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere det valgte team. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
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
                    textBoxTeamSalary.Text = SetTeamSalary().ToString();
                    dataGridEmployees.SelectedItem = selectedEmployee = null;
                    buttonAddToTeam.IsEnabled = false;
                    buttonRemoveFromTeam.IsEnabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl under forsøget på at tilføje den valgte ansat til teamet. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
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
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl under forsøget i at fjerne den valgte ansat fra holdet. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_ShowAllEmployees_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReloadDataGridEmployees();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl under forsøget i at vise alle ansatte. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            dataGridTeams.SelectedItem = null;
            dataGridEmployees.SelectedItem = null;
            buttonAddToTeam.IsEnabled = false;
            buttonRemoveFromTeam.IsEnabled = false;
        }

        private void Button_ShowAvailableEmployees_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGridEmployees.ItemsSource = model.Employees.Where(employees => employees.TeamId == null).ToList();
                selectedEmployee = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl under forsøget på at vise alle ledige ansatte. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //                                  Methods

        /// <summary>
        /// Refils the DataGridTeams with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Teams is null.</exception>
        private void ReloadDataGridTeams()
        {
            dataGridTeams.ItemsSource = model.Teams.ToList();
        }

        /// <summary>
        /// Refils the DataGridEmployees with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Employees is null.</exception>
        private void ReloadDataGridEmployees()
        {
            dataGridEmployees.ItemsSource = model.Employees.ToList();
        }

        /// <summary>
        /// Sets the TeamSalary to every employee's salary put together.
        /// </summary>
        /// <returns>The selected team's budget.</returns>
        private decimal SetTeamSalary()
        {
            selectedTeam.Budget = 0;
            foreach (Employee employee in selectedTeam.Employees)
            {
                selectedTeam.Budget += employee.Salary;
            }
            model.SaveChanges();
            return selectedTeam.Budget;
        }
    }
}