using FluentApi.EF;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for DetailsUserControl.xaml
    /// </summary>
    public partial class DetailsUserControl : UserControl
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

        public DetailsUserControl()
        {
            InitializeComponent();
            model = new Model();
            try
            {
                ReloadDataGridProject();
                textBoxAllProjectsSalary.Text = GetAllPayments().ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void DataGrid_Projects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProject = dataGridProjects.SelectedItem as Project;
            if (selectedProject != null)
            {
                textBoxProjectTeamSalary.Text = String.Empty;
                textBoxProjectSalary.Text = GetProjectPayments().ToString();
                dataGridTeams.ItemsSource = selectedProject.Teams;
            }
        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedTeam != null)
            {
                textBoxProjectTeamSalary.Text = selectedTeam.Budget.ToString();
            }
        }

        //                                              Methods

        /// <summary>
        /// Refils the DataGridProject with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Project is null.</exception>
        private void ReloadDataGridProject()
        {
            dataGridProjects.ItemsSource = model.Projects.ToList();
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

        /// <summary>
        /// Gets the total amount it costs to have every project running.
        /// </summary>
        /// <returns>The total cost of every project.</returns>
        private decimal GetAllPayments()
        {
            decimal allPayments = 0;
            foreach (Project project in model.Projects.ToList())
            {
                decimal projectPayments = 0;
                foreach (Team team in project.Teams)
                {
                    projectPayments += team.Budget;
                }
                allPayments += projectPayments;
            }
            return allPayments;
        }
    }
}
