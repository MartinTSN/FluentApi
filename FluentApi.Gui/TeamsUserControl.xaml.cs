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

                    textBoxTeamName.Text = selectedTeam.Name;
                    textBoxDescription.Text = selectedTeam.Description;
                    datePickerStartDate.SelectedDate = selectedTeam.StartDate;
                    datePickerEndDate.SelectedDate = selectedTeam.ExpectedEndDate;
                    buttonEditTeam.IsEnabled = true;
                }
                else
                {
                    dataGridTeams.ItemsSource = null;
                }
                dataGridTeams.SelectedItem = null;
                ReloadDataGridTeams();
            }
        }

        private void DataGrid_Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
        }

        private void ReloadDataGridTeams()
        {
            dataGridTeams.ItemsSource = model.Teams.ToList();
        }

        private void ReloadDataGridEmployees()
        {
            dataGridEmployees.ItemsSource = model.Employees.ToList();
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
                foreach (Employee employee in selectedTeam.Employees)
                {
                    if (selectedEmployee != employee)
                    {
                        selectedTeam.Employees.Add(selectedEmployee);
                        break;
                    }
                }
                model.SaveChanges();
                dataGridTeams.SelectedItem = selectedTeam;
                dataGridEmployees.ItemsSource = selectedTeam.Employees;
                dataGridTeams.SelectedItem = selectedTeam = null;
                dataGridEmployees.SelectedItem = selectedEmployee = null;
                buttonAddToTeam.IsEnabled = false;
                buttonRemoveFromTeam.IsEnabled = true;

            }
        }

        private void Button_RemoveFromTeam_Click(object sender, RoutedEventArgs e)
        {
            selectedEmployee = dataGridEmployees.SelectedItem as Employee;
            selectedTeam = dataGridTeams.SelectedItem as Team;
            if (selectedEmployee != null && selectedTeam != null)
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
        }
    }
}
