using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CapaNegocio
{
    public class BusinessCustomer
    {
        public List<Customer> GetCustomers()
        {
            DataCustomer dataCustomers = new DataCustomer();
            return dataCustomers.GetCustomers();
        }

        public List<Customer> GetCustomersByName(string name)
        {
            List<Customer> allCustomers = new DataCustomer().GetCustomers();
            //DataCustomer dataCustomers = new DataCustomer();
            //List<Customer> allCustomers = dataCustomers.GetCustomers();
            //var customersByName = allCustomers.Where(x => x.Name == name).ToList();
            //return customersByName;
            return allCustomers
                .Where(c => string.IsNullOrEmpty(name) ||
                            c.Name != null && c.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        public void AddCustomer(Customer customer)
        {
            List<Customer> existing = GetCustomersByName(customer.Name);

            // Si ya existe activo, no se permite
            if (existing.Any(c => c.Name.Equals(customer.Name, StringComparison.OrdinalIgnoreCase) && c.Active))
            {
                throw new Exception("El cliente con ese nombre ya existe.");
            }

            // Si existe INACTIVO, lo reactivamos y actualizamos datos
            Customer inactive = existing.FirstOrDefault(c => c.Name.Equals(customer.Name, StringComparison.OrdinalIgnoreCase) && !c.Active);
            if (inactive != null)
            {
                inactive.Address = customer.Address;
                inactive.Phone = customer.Phone;
                inactive.Active = true;
                UpdateCustomer(inactive);
            }
            else
            {
                // Si no existe, lo creamos
                DataCustomer dataCustomers = new DataCustomer();
                dataCustomers.AddCustomer(customer);
            }
        }



        public void UpdateCustomer(Customer customer)
        {
            DataCustomer dataCustomers = new DataCustomer();
            dataCustomers.UpdateCustomer(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            DataCustomer dataCustomers = new DataCustomer();
            dataCustomers.DeleteCustomer(customer.CustomerId);
        }

    }
}
