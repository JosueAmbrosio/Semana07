using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CapaEntidades;
using CapaNegocio;

namespace Semana07
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnListar_Click(object sender, RoutedEventArgs e)
        {
            BusinessCustomer businessCustomers = new BusinessCustomer();
            List<Customer> customers = businessCustomers.GetCustomers();
            dgCustomers.ItemsSource = customers;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();

            BusinessCustomer business = new BusinessCustomer();

            List<Customer> result = business.GetCustomersByName(string.IsNullOrEmpty(name) ? null : name);

            dgCustomers.ItemsSource = result;
        }

        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewName.Text))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Customer newCustomer = new Customer
            {
                Name = txtNewName.Text.Trim(),
                Address = txtNewAddress.Text.Trim(),
                Phone = txtNewPhone.Text.Trim(),
                Active = true
            };

            try
            {
                BusinessCustomer business = new BusinessCustomer();
                business.AddCustomer(newCustomer);

                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                txtNewName.Clear();
                txtNewAddress.Clear();
                txtNewPhone.Clear();

                dgCustomers.ItemsSource = business.GetCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}