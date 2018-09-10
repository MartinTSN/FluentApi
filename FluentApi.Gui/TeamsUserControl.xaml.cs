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
            //dataGridTeams.ItemsSource = model.Teams.ToList();
            dataGridEmployees.ItemsSource = model.Employees.ToList();

        }

        private void DataGrid_Teams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeam = dataGridTeams.SelectedItem as Team;
            dataGridEmployees.ItemsSource = selectedTeam.Employees;

            
        }

    }
}
