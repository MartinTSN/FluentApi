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
            ReloadDataGridProject();
            ReloadDataGridTeams();
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
                textBoxProjectName.Text = selectedProject.Name;
                textBoxDescription.Text = selectedProject.Description;
                datePickerStartDate.SelectedDate = selectedProject.StartDate;
                datePickerEndDate.SelectedDate = selectedProject.EndDate;
                textBoxBudgetLimit.Text = selectedProject.BudgetLimit.ToString();
                buttonEditProject.IsEnabled = true;
                buttonAddProject.IsEnabled = false;
                textBoxProjectBuget.Text = GetProjectPayments().ToString();
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

        private decimal GetProjectPayments()
        {
            decimal projectPayments = 0;
            decimal teamSalaries = 0;
            foreach (Team team in selectedProject.Teams)
            {
                foreach (Employee employee in team.Employees)
                {
                    teamSalaries += employee.Salary;
                }
            }
            projectPayments += teamSalaries;
            return projectPayments;
        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedTeam != null)
            {
                textBoxTeamSalary.Text = GetTeamSalary().ToString();
            }
        }

        private void Button_AddProject_Click(object sender, RoutedEventArgs e)
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

        private void Button_EditProject_Click(object sender, RoutedEventArgs e)

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

        private void Button_AddToProject_Click(object sender, RoutedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            selectedProject = dataGridProjects.SelectedItem as Project;

            if (GetTeamSalary() > (selectedProject.BudgetLimit - GetProjectPayments()))
            {
                MessageBox.Show("Du kan ikke tilføje et team som gør at bugettet går over grænsen");
            }
            else
            {
                if (selectedTeam != null && selectedProject != null)
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

                model.SaveChanges();
                dataGridProjects.SelectedItem = selectedProject;
                dataGridTeams.ItemsSource = selectedProject.Teams;
                textBoxProjectBuget.Text = GetProjectPayments().ToString();
                dataGridProjects.SelectedItem = selectedProject = null;
                dataGridTeams.SelectedItem = selectedTeam = null;
                buttonAddToProject.IsEnabled = false;
                buttonRemoveFromProject.IsEnabled = true;
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
                model.SaveChanges();
                dataGridProjects.SelectedItem = selectedProject;
                dataGridTeams.SelectedItem = selectedTeam = null;
                ReloadDataGridTeams();
                dataGridTeams.ItemsSource = selectedProject.Teams;
                buttonRemoveFromProject.IsEnabled = false;
                textBoxProjectBuget.Text = GetProjectPayments().ToString();
            }
        }

        private void Button_ShowAllTeams_Click(object sender, RoutedEventArgs e)
        {
            ReloadDataGridTeams();
            dataGridProjects.SelectedItem = null;
            dataGridTeams.SelectedItem = null;
            buttonAddToProject.IsEnabled = false;
            buttonRemoveFromProject.IsEnabled = false;
        }

        private void Button_ShowAvailableTeams_Click(object sender, RoutedEventArgs e)
        {
            dataGridTeams.ItemsSource = model.Teams.Where(teams => teams.ProjectId == null).ToList();
        }

        private void DataGrid_Projects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                dataGridProjects.SelectedItem = null;
                buttonAddProject.IsEnabled = true;
                buttonEditProject.IsEnabled = false;
                buttonAddToProject.IsEnabled = false;
                buttonRemoveFromProject.IsEnabled = false;
                textBoxProjectName.Text = String.Empty;
                textBoxDescription.Text = String.Empty;
                datePickerStartDate.SelectedDate = null;
                datePickerEndDate.SelectedDate = null;
                textBoxBudgetLimit.Text = String.Empty;
                textBoxProjectBuget.Text = String.Empty;
                textBoxTeamSalary.Text = String.Empty;
                textBoxProjectName.Focus();
                ReloadDataGridTeams();
            }
        }
    }
}
