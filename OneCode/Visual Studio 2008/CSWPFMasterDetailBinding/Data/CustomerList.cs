﻿/************************************* Module Header **************************************\
* Module Name:  CustomerList.cs
* Project:      CSWPFMasterDetailBinding
* Copyright (c) Microsoft Corporation.
* 
* This example demonstrates how to do master/detail data binding in WPF.
* 
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 10/29/2009 3:00 PM Zhi-Xin Created
 * 
\******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CSWPFMasterDetailBinding.Data
{
    class CustomerList
    {
        private ObservableCollection<Customer> _customers;

        public CustomerList()
        {
            _customers = new ObservableCollection<Customer>();

            // Insert customer and corresponding order information into
            Customer c = new Customer() { ID = 1, Name = "Customer1" };
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 1, 1), ShipCity = "Shanghai" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 2, 1), ShipCity = "Beijing" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 11, 10), ShipCity = "Guangzhou" });
            _customers.Add(c);

            c = new Customer() { ID = 2, Name = "Customer2" };
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 1, 1), ShipCity = "New York" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 2, 1), ShipCity = "Seattle" });
            _customers.Add(c);

            c = new Customer() { ID = 3, Name = "Customer3" };
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 1, 1), ShipCity = "Xiamen" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 2, 1), ShipCity = "Shenzhen" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 11, 10), ShipCity = "Tianjin" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 11, 10), ShipCity = "Wuhan" });
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 11, 10), ShipCity = "Jinan" });
            _customers.Add(c);

            c = new Customer() { ID = 4, Name = "Customer4" };
            c.Orders.Add(new Order() { ID = 1, Date = new DateTime(2009, 1, 1), ShipCity = "Lanzhou" });
            _customers.Add(c);
        }

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
        }
    }
}
