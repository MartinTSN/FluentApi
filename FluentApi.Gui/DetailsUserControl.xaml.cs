using System;
using System.Collections.Generic;
using System.Linq;
using FluentApi.EF;
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

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for DetailsUserControl.xaml
    /// </summary>
    public partial class DetailsUserControl : UserControl
    {
        protected Model model;
        private Project selectedProject;
        private Team selectedTeam;
        private Employee selectedEmployee;

        public DetailsUserControl()
        {
            InitializeComponent();
            model = new Model();
            try
            {
                ReloadDataGridProjects();
            }
            catch (Exception e)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet", e.Message, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        public void ReloadDataGridProjects()
        {
            dataGridProjects.ItemsSource = model.Projects.ToList();
        }

        public void ReloadDataGridTeams()
        {
            dataGridTeams.ItemsSource = model.Teams.ToList();
        }

        public void ReloadDataGridEmployees()
        {
            dataGridEmployees.ItemsSource = model.Employees.ToList();
        }

        private void DataGrid_Projects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProject = dataGridProjects.SelectedItem as Project;
            dataGridTeams.ItemsSource = selectedProject.Teams;
            textBoxProjectBudget.Text = selectedProject.BudgetLimit.ToString();
        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            dataGridEmployees.ItemsSource = selectedTeam.Employees;
            textBoxTeamBudget.Text = 
        }

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
        }
    }
}
