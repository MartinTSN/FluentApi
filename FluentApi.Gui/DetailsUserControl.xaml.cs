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

        }
    }
}
