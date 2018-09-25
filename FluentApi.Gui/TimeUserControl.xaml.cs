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
            dataGridTeams.ItemsSource = model.Teams.Where(team => team.Employees != null).ToList();
        }
    }
}
