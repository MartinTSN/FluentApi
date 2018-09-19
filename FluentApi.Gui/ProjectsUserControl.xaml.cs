using FluentApi.EF;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for ProjectsUserControl.xaml
    /// </summary>
    public partial class ProjectsUserControl : UserControl
    {
        protected Model model;
        /// <summary>
        /// The currently selected project.
        /// </summary>
        private Project selectedProject;
        /// <summary>
        /// The currently selected team.
        /// </summary>
        private Team selectedTeam;

        public ProjectsUserControl()
        {
            InitializeComponent();
            model = new Model();
            try
            {
                ReloadDataGridProject();
                ReloadDataGridTeams();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //                              DataGrid Events

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedProject != null)
            {
                buttonAddToProject.IsEnabled = true;
            }
            if (selectedTeam != null)
            {
                try
                {
                    textBoxTeamSalary.Text = selectedTeam.Budget.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }
        private void DataGrid_Projects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProject = dataGridProjects.SelectedItem as Project;

            if (selectedTeam != null)
            {
                buttonAddToProject.IsEnabled = true;
            }
            else
            {
                if (selectedProject != null)
                {
                    dataGridTeams.ItemsSource = selectedProject.Teams;
                    buttonAddToProject.IsEnabled = false;
                    buttonRemoveFromProject.IsEnabled = true;
                }
            }
            if (selectedProject != null)
            {
                try
                {
                    textBoxProjectName.Text = selectedProject.Name;
                    textBoxDescription.Text = selectedProject.Description;
                    datePickerStartDate.SelectedDate = selectedProject.StartDate;
                    datePickerEndDate.SelectedDate = selectedProject.EndDate;
                    textBoxBudgetLimit.Text = selectedProject.BudgetLimit.ToString();
                    buttonEditProject.IsEnabled = true;
                    buttonAddProject.IsEnabled = false;

                    textBoxProjectBudget.Text = GetProjectPayments().ToString();

                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void DataGrid_Projects_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridProjects.SelectedItem != null)
            {
                if (e.Key == Key.Escape)
                {
                    try
                    {
                        ReloadDataGridTeams();
                        dataGridProjects.SelectedItem = null;
                        dataGridTeams.SelectedItem = null;
                        textBoxProjectName.Focus();
                        //                                                      TextBoxes
                        textBoxProjectName.Text = String.Empty;
                        textBoxDescription.Text = String.Empty;
                        textBoxBudgetLimit.Text = String.Empty;
                        textBoxProjectBudget.Text = String.Empty;
                        textBoxTeamSalary.Text = String.Empty;
                        //                                                      Datepickers
                        datePickerStartDate.SelectedDate = null;
                        datePickerEndDate.SelectedDate = null;
                        //                                                      Buttons
                        buttonAddProject.IsEnabled = true;
                        buttonEditProject.IsEnabled = false;
                        buttonAddToProject.IsEnabled = false;
                        buttonRemoveFromProject.IsEnabled = false;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete en uventet fejl. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }
        
        //                          Button Events

        private void Button_AddProject_Click(object sender, RoutedEventArgs e)
        {
            bool couldParse = Decimal.TryParse(textBoxBudgetLimit.Text, out decimal d);
            if (!Validator.IsNameValid(textBoxProjectName.Text))
            {
                MessageBox.Show("Det indtastede Projekt-navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            else if (!Validator.IsMoneyValid(d))
            {
                MessageBox.Show("Den indtastede budget-værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    Project newProject = new Project();
                    newProject.Name = textBoxProjectName.Text;
                    newProject.Description = textBoxDescription.Text;
                    newProject.StartDate = datePickerStartDate.SelectedDate.Value;
                    newProject.EndDate = datePickerEndDate.SelectedDate.Value;
                    newProject.BudgetLimit = Decimal.Parse(textBoxBudgetLimit.Text);
                    model.Projects.Add(newProject);
                    model.SaveChanges();
                    ReloadDataGridProject();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at gemme det nye projekt. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_EditProject_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsNameValid(textBoxProjectName.Text))
            {
                MessageBox.Show("Det indtastede Projekt-navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            else if (!Validator.IsMoneyValid(Decimal.Parse(textBoxBudgetLimit.Text)))
            {
                MessageBox.Show("Det indtastede penge værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    if (textBoxProjectName.Text != selectedProject.Name)
                    {
                        selectedProject.Name = textBoxProjectName.Text;
                    }
                    if (textBoxDescription.Text != selectedProject.Description)
                    {
                        selectedProject.Description = textBoxDescription.Text;
                    }
                    if (datePickerStartDate.SelectedDate != selectedProject.StartDate)
                    {
                        selectedProject.StartDate = datePickerStartDate.SelectedDate.Value;
                    }
                    if (datePickerEndDate.SelectedDate != selectedProject.EndDate)
                    {
                        selectedProject.EndDate = datePickerEndDate.SelectedDate.Value;
                    }
                    if (textBoxBudgetLimit.Text != selectedProject.BudgetLimit.ToString() && Decimal.Parse(textBoxBudgetLimit.Text) > GetProjectPayments())
                    {
                        selectedProject.BudgetLimit = Decimal.Parse(textBoxBudgetLimit.Text);
                    }
                    model.SaveChanges();
                    ReloadDataGridProject();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere det valgte projekt. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_AddToProject_Click(object sender, RoutedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            selectedProject = dataGridProjects.SelectedItem as Project;

            if (selectedTeam.Budget > (selectedProject.BudgetLimit - GetProjectPayments()))
            {
                MessageBox.Show("Du kan ikke tilføje et team som gør at bugettet går over grænsen. Prøv igen.", "Vælg en anden.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                if (selectedTeam != null && selectedProject != null)
                {
                    try
                    {
                        foreach (Team team in selectedProject.Teams)
                        {
                            if (selectedTeam != team)
                            {
                                selectedProject.Teams.Add(selectedTeam);
                                break;
                            }
                        }
                        if (selectedProject.Teams.Count == 0)
                        {
                            selectedProject.Teams.Add(selectedTeam);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der skete en uventet fejl. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }

                try
                {
                    model.SaveChanges();
                    dataGridProjects.SelectedItem = selectedProject;
                    dataGridTeams.ItemsSource = selectedProject.Teams;
                    textBoxProjectBudget.Text = GetProjectPayments().ToString();
                    dataGridProjects.SelectedItem = selectedProject = null;
                    dataGridTeams.SelectedItem = selectedTeam = null;
                    buttonAddToProject.IsEnabled = false;
                    buttonRemoveFromProject.IsEnabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl under forsøget i at tilføje det valgte team til projektet. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_RemoveFromProject_Click(object sender, RoutedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            selectedProject = dataGridProjects.SelectedItem as Project;

            if (selectedTeam != null && selectedProject != null)
            {
                foreach (Team team in selectedProject.Teams)
                {
                    if (selectedTeam == team)
                    {
                        selectedProject.Teams.Remove(selectedTeam);
                        break;
                    }
                }
                try
                {
                    model.SaveChanges();
                    dataGridProjects.SelectedItem = selectedProject;
                    dataGridTeams.SelectedItem = selectedTeam = null;
                    ReloadDataGridTeams();
                    dataGridTeams.ItemsSource = selectedProject.Teams;
                    buttonRemoveFromProject.IsEnabled = false;
                    textBoxProjectBudget.Text = GetProjectPayments().ToString();
                    textBoxTeamSalary.Text = selectedTeam.Budget.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Der skete en uventet fejl under forsøget i at fjerne det valgte team fra projektet. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_ShowAllTeams_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReloadDataGridTeams();
                dataGridProjects.SelectedItem = null;
                dataGridTeams.SelectedItem = null;
                buttonAddToProject.IsEnabled = false;
                buttonRemoveFromProject.IsEnabled = false;
                textBoxTeamSalary.Text = String.Empty;
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl under forsøget i at vise alle teams. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Button_ShowAvailableTeams_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGridTeams.ItemsSource = model.Teams.Where(teams => teams.ProjectId == null).ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl Under forsøget i at vise alle ledige teams. Prøv igen.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        
        //                                      Methods

        /// <summary>
        /// Refils the DataGridProject with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Project is null.</exception>
        private void ReloadDataGridProject()
        {
            dataGridProjects.ItemsSource = model.Projects.ToList();
        }

        /// <summary>
        /// Refils the DataGridTeams with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Teams is null.</exception>
        private void ReloadDataGridTeams()
        {
            dataGridTeams.ItemsSource = model.Teams.ToList();
        }

        /// <summary>
        /// Gets the Total amount it costs to have every team in the project running.
        /// </summary>
        /// <returns>The total costs of the projects teams.</returns>
        private decimal GetProjectPayments()
        {
            decimal projectPayments = 0;
            if (selectedProject != null)
            {
                foreach (Team team in selectedProject.Teams)
                {
                    projectPayments += team.Budget;
                }
            }
            return projectPayments;
        }
    }
}
