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

        private Project selectedProject;

        public DetailsUserControl()
        {
            InitializeComponent();
            model = new Model();
            try
            {
                ReloadDataGridProject();
            }
            catch (Exception)
            {
                MessageBox.Show("Der skete en uventet fejl. Prøv igen eller genstart programmet.", "Uventet fejl.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        /// <summary>
        /// Refils the DataGridProject with data.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the model.Project is null.</exception>
        private void ReloadDataGridProject()
        {
            dataGridProjects.ItemsSource = model.Projects.ToList();
        }
    }
}
