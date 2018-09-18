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
    /// Interaction logic for ProjectsUserControl.xaml
    /// </summary>
    public partial class ProjectsUserControl : UserControl
    {
        protected Model model;
        private Project selectedProject;
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
            catch (Exception e)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet", e.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void ReloadDataGridProject()
        {
            dataGridProjects.ItemsSource = model.Projects.ToList();
        }

        private void ReloadDataGridTeams()
        {
            dataGridTeams.ItemsSource = model.Teams.ToList();
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

                    textBoxProjectBuget.Text = GetProjectPayments().ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

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
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_AddProject_Click(object sender, RoutedEventArgs e)
        {
            bool couldParse = Decimal.TryParse(textBoxBudgetLimit.Text, out decimal d);
            if (!Validator.IsNameValid(textBoxProjectName.Text))
            {
                MessageBox.Show("Det indtastede Projekt-navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsDescriptionValid(textBoxDescription.Text))
            {
                MessageBox.Show("Den indtastede beskrivelse er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsStartDateValid(datePickerStartDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede Start-dato er ikke gyldigt. Skal være inden imorgen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsEndDateValid(datePickerEndDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede Slut-dato er ikke gyldigt. Skal være efter idag.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsMoneyValid(d))
            {
                MessageBox.Show("Den indtastede budget-værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at gemme det nye projekt. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_EditProject_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsNameValid(textBoxProjectName.Text))
            {
                MessageBox.Show("Det indtastede Projekt-navn er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsDescriptionValid(textBoxDescription.Text))
            {
                MessageBox.Show("Den indtastede beskrivelse er ikke gyldigt. Må kun indeholde bogstaver og mellemrum. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsStartDateValid(datePickerStartDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede Start-dato er ikke gyldigt. Skal være inden imorgen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsEndDateValid(datePickerEndDate.SelectedDate.Value))
            {
                MessageBox.Show("Den indtastede Slut-dato er ikke gyldigt. Skal være efter idag.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!Validator.IsMoneyValid(Decimal.Parse(textBoxBudgetLimit.Text)))
            {
                MessageBox.Show("Det indtastede penge værdi er ikke gyldigt. Må kun indeholde tal og skal være positiv. Prøv igen.", "Indtastningsfejl", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Der skete desværre en uventet fejl under forsøget på at opdatere det valgte projekt. Prøv igen", "Uventet fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void Button_AddToProject_Click(object sender, RoutedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            selectedProject = dataGridProjects.SelectedItem as Project;

            if (selectedTeam.Budget > (selectedProject.BudgetLimit - GetProjectPayments()))
            {
                MessageBox.Show("Du kan ikke tilføje et team som gør at bugettet går over grænsen", "Fejl", MessageBoxButton.OK, MessageBoxImage.Stop);
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }

                try
                {
                    model.SaveChanges();
                    dataGridProjects.SelectedItem = selectedProject;
                    dataGridTeams.ItemsSource = selectedProject.Teams;
                    textBoxProjectBuget.Text = GetProjectPayments().ToString();
                    dataGridProjects.SelectedItem = selectedProject = null;
                    dataGridTeams.SelectedItem = selectedTeam = null;
                    buttonAddToProject.IsEnabled = false;
                    buttonRemoveFromProject.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
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
                    textBoxProjectBuget.Text = GetProjectPayments().ToString();
                    textBoxTeamSalary.Text = selectedTeam.Budget.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
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
            catch (Exception ex)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Button_ShowAvailableTeams_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGridTeams.ItemsSource = model.Teams.Where(teams => teams.ProjectId == null).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void DataGrid_Projects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                dataGridProjects.SelectedItem = null;
                dataGridTeams.SelectedItem = null;
                textBoxProjectName.Focus();
                //                                                      TextBoxes
                textBoxProjectName.Text = String.Empty;
                textBoxDescription.Text = String.Empty;
                textBoxBudgetLimit.Text = String.Empty;
                textBoxProjectBuget.Text = String.Empty;
                textBoxTeamSalary.Text = String.Empty;
                //                                                      Datepickers
                datePickerStartDate.SelectedDate = null;
                datePickerEndDate.SelectedDate = null;
                //                                                      Buttons
                buttonAddProject.IsEnabled = true;
                buttonEditProject.IsEnabled = false;
                buttonAddToProject.IsEnabled = false;
                buttonRemoveFromProject.IsEnabled = false;

                try
                {
                    ReloadDataGridTeams();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der skete en uventet fejl. Prøv igen.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }
    }
}
