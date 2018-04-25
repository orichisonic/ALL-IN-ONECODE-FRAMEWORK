﻿/****************************** Module Header ******************************\
* Module Name:    DBProcess.cs
* Project:        CSExcelLikeGridView
* Copyright (c) Microsoft Corporation
*
* This module is managing the Connection,adapter as well as datatable
* instances.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
\*****************************************************************************/

using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CSExcelLikeGridView
{
    /// <summary>
    /// This is a class that manages the whole db's connection, create a 
    /// memory-based datatable, maintaince the state and do a batch save 
    /// things together in different functions.
    /// </summary>
    public sealed class DBProcess
    {
        private static SqlConnection conn = null;
        private static SqlDataAdapter adapter = null;
        private DataTable dt = null;

        /// <summary>
        /// This static constructor will read out the whole connect string
        /// from defined web.config. The connection and adapter are both
        /// pointing to the same db, so only create once.
        /// </summary>
        static DBProcess()
        {
            string constr = ConfigurationManager.ConnectionStrings["MyConn"]
                                                .ConnectionString;
            conn = new SqlConnection(constr);
            string command = "select * from tb_personInfo";
            adapter = new SqlDataAdapter(command, conn);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            builder.GetDeleteCommand(true);
            builder.GetInsertCommand(true);
            builder.GetUpdateCommand(true);
        }

        /// <summary>
        /// This function will create a datatable and a "Table.dat" file.
        /// This is a serialized type file to store all table rows' states
        /// together in order to avoid memory-based lost problem.
        /// </summary>
        public  DataTable GetDataTable(bool create)
        {
            if (create)
            {
                using (FileStream fs = new FileStream("Table.dat", FileMode.Create))
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                    dt.Columns[0].AutoIncrement = true;
                    dt.Columns[0].AutoIncrementStep = 1;
                    dt.Columns[0].AutoIncrementSeed = dt.Rows.Count;
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, dt);
                }
            }
            else
            {
                using (FileStream fs = new FileStream("Table.dat",FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    dt = (DataTable)bf.Deserialize(fs);
                }
            }
            return dt;
        }

        /// <summary>
        /// Write a new memory-based datatable into the file.
        /// </summary>
        public void WriteDataTable(DataTable dt)
        {
            using (FileStream fs = new FileStream("Table.dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, dt);
            }
        }
        /// <summary>
        /// Update the DataTable and delete the serialized file.
        /// </summary>
        public void BatchSave(DataTable dt)
        {
            adapter.Update(dt);
            File.Delete("Table.dat");
        }
    }
}