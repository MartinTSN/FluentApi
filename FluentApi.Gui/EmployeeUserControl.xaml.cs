using FluentApi.EF;

using System.Linq;
using System.Windows.Controls;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for EmployeeUserControl.xaml
    /// </summary>
    public partial class EmployeeUserControl : UserControl
    {
        protected Model model;

        public EmployeeUserControl()
        {
            InitializeComponent();
            model = new Model();
            dataGridEmployees.ItemsSource = model.Employees.ToList();
        }
    }
}
