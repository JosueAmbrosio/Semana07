using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CapaEntidades;
using CapaNegocio;

namespace Semana07
{
    public partial class MainWindow : Window
    {
        BusinessCustomer business = new BusinessCustomer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnListar_Click(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = business.GetCustomers();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            dgCustomers.ItemsSource = business.GetCustomersByName(string.IsNullOrEmpty(name) ? null : name);
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Customer newCustomer = ShowCustomerDialog();
            if (newCustomer != null)
            {
                try
                {
                    business.AddCustomer(newCustomer);
                    MessageBox.Show("Customer created successfully.");
                    dgCustomers.ItemsSource = business.GetCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selected)
            {
                Customer updatedCustomer = ShowCustomerDialog(selected);
                if (updatedCustomer != null)
                {
                    try
                    {
                        business.UpdateCustomer(updatedCustomer);
                        MessageBox.Show("Customer updated successfully.");
                        dgCustomers.ItemsSource = business.GetCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to update.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selected)
            {
                if (MessageBox.Show($"Are you sure you want to delete '{selected.Name}'?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        business.DeleteCustomer(selected);
                        MessageBox.Show("Customer deleted logically.");
                        dgCustomers.ItemsSource = business.GetCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }

        private Customer ShowCustomerDialog(Customer customer = null)
        {
            Window dialog = new Window
            {
                Title = customer == null ? "Add Customer" : "Update Customer",
                Height = 250,
                Width = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this
            };

            Grid grid = new Grid
            {
                Margin = new Thickness(10)
            };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 2; i++) grid.ColumnDefinitions.Add(new ColumnDefinition());

            // Name
            grid.Children.Add(new Label { Content = "Name:", VerticalAlignment = VerticalAlignment.Center });
            TextBox txtName = new TextBox { Text = customer?.Name ?? "" };
            Grid.SetRow(txtName, 0); Grid.SetColumn(txtName, 1);
            grid.Children.Add(txtName);

            // Address
            grid.Children.Add(new Label { Content = "Address:", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[^1], 1); // ^1 = último
            TextBox txtAddress = new TextBox { Text = customer?.Address ?? "" };
            Grid.SetRow(txtAddress, 1); Grid.SetColumn(txtAddress, 1);
            grid.Children.Add(txtAddress);

            // Phone
            grid.Children.Add(new Label { Content = "Phone:", VerticalAlignment = VerticalAlignment.Center });
            Grid.SetRow(grid.Children[^1], 2);
            TextBox txtPhone = new TextBox { Text = customer?.Phone ?? "" };
            Grid.SetRow(txtPhone, 2); Grid.SetColumn(txtPhone, 1);
            grid.Children.Add(txtPhone);

            // Buttons
            StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            Button btnOK = new Button { Content = "Save", Width = 80, Margin = new Thickness(5) };
            Button btnCancel = new Button { Content = "Cancel", Width = 80, Margin = new Thickness(5) };
            panel.Children.Add(btnOK); panel.Children.Add(btnCancel);
            Grid.SetRow(panel, 3); Grid.SetColumnSpan(panel, 2);
            grid.Children.Add(panel);

            dialog.Content = grid;

            bool? result = null;
            btnOK.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Name is required.");
                    return;
                }
                result = true;
                dialog.Close();
            };
            btnCancel.Click += (_, _) => { result = false; dialog.Close(); };

            dialog.ShowDialog();

            if (result == true)
            {
                return new Customer
                {
                    CustomerId = customer?.CustomerId ?? 0,
                    Name = txtName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Active = true
                };
            }

            return null;
        }
    }
}
