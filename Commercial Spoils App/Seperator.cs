using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace Commercial_Spoils_App
{
    class Seperator
    {
        
        public static DataTable DataFromTextFile(string location, char delimeter = '|')
        {            
            DataTable result;
            string[] LineArray = File.ReadAllLines(location);            
            result = FormDataTable(LineArray, delimeter);
            return result;
        }

        private static DataTable FormDataTable(string[] LineArray, char delimeter)
        {                   
            DataTable dt = new DataTable();
            AddColumnToTable(LineArray, delimeter, ref dt);
            AddRowToTabel(LineArray, delimeter, ref dt);
            return dt;
        }

        private static void AddRowToTabel(string[] valueCollection, char delimeter, ref DataTable dt)
        {
                for (int i = 1; i < valueCollection.Length; i++)
                {
                    string[] values = valueCollection[i].Split(delimeter);
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < values.Length; j++)
                    {
                        dr[j] = values[j];
                    }
                    dt.Rows.Add(dr);
                }
        }

        private static void AddColumnToTable(string[] columnCollection, char delimeter, ref DataTable dt)
        {
                string[] columns = columnCollection[0].Split(delimeter);
                foreach (string columnName in columns)
                {
                    DataColumn dc = new DataColumn(columnName, typeof(string));
                    dt.Columns.Add(dc);
                }
        }

    }
}
