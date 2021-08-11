using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AuctionSystem.ORM.Oracle
{
    public static class Extensions
    {
        /// <summary>
        /// Wrapper for method AddWithValue
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public static void AddWithValue(this OracleParameterCollection cmd, string parameterName, object value)
        {
            cmd.Add(parameterName, value);
        }
    }
}

