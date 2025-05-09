using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

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
            DataCustomer dataCustomers = new DataCustomer();
            dataCustomers.AddCustomer(customer);
        }

    }
}
