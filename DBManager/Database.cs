using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class Database
    {
        private List<Table> Tables = new List<Table>();
        private string m_username;
        private string m_password;

        public string LastErrorMessage { get; private set; }

        public Manager SecurityManager { get; private set; }

        //This constructor should only be used from Load (without needing to set a password for the user). It cannot be used from any other class
        private Database()
        {
        }

        public Database(string adminUsername, string adminPassword)
        {
            //DEADLINE 1.B: Initalize the member variables
            m_username = adminUsername;

            m_password = adminPassword;

            
        }

        public bool AddTable(Table table)
        {
            //DEADLINE 1.B: Add a new table to the database

            //mirar que la table que nos dan no sea nula
            if(table == null)
            {
                return false;
            }
            //comprobar que no existe ya en la lista
            for (int i = 0; i < Tables.Count; i++)
            {
                if (Tables[i].Name == table.Name)
                {
                    //ya existe
                    return false;
                }
            }
            //añado a lista la table que me dan
            Tables.Add(table);
            return true;
            
        }

        public Table TableByName(string tableName)
        {
            //DEADLINE 1.B: Find and return the table with the given name

            //recorrer la lista buscando que coincidan los nombres
            for(int i = 0; i < Tables.Count; i++)
            {
                if (Tables[i].Name == tableName)
                {
                    return Tables[i];
                }
            }

            
            return null;
            
        }

        public bool CreateTable(string tableName, List<ColumnDefinition> ColumnDefinition)
        {
            //DEADLINE 1.B: Create and new table with the given name and columns. If there is already a table with that name,
            //return false and set LastErrorMessage with the appropriate error (Check Constants.cs)
            //Do the same if no column is provided
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)

            //mirar si ya existe una tabla con ese nombre
            for (int i = 0; i < Tables.Count; i++)
            {
                if (Tables[i].Name == tableName)
                {
                    LastErrorMessage = Constants.TableAlreadyExistsError;
                    return false;
                    
                }
            }

            //comprobar que nos de la columna
      
            if(ColumnDefinition == null)
            {
                LastErrorMessage = Constants.DatabaseCreatedWithoutColumnsError;
                return false;
            
            }
            //comprobar que no este vacía
            else if(ColumnDefinition.Count == 0)
            {
                LastErrorMessage = Constants.DatabaseCreatedWithoutColumnsError;
                return false;
                
            }


            Table nuevaTabla = new Table(tableName, ColumnDefinition);
            Tables.Add(nuevaTabla);
            LastErrorMessage = Constants.CreateTableSuccess;
            return true;
            
        }

        public bool DropTable(string tableName)
        {
            //DEADLINE 1.B: Delete the table with the given name. If the table doesn't exist, return false and set LastErrorMessage
            //If everything goes ok, return true and set LastErrorMessage with the appropriate success message (Check Constants.cs)

            for(int i =0; i< Tables.Count; i++)
            {
                if(Tables[i].Name == tableName)
                {
                    Tables.Remove(Tables[i]);
                    LastErrorMessage = Constants.DropTableSuccess;
                    return true;
                }
                
            }
            
            LastErrorMessage = Constants.TableDoesNotExistError;
            return false;
        }

        public bool Insert(string tableName, List<string> values)
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            for (int i = 0; i < Tables.Count; i++)
            {
                if (Tables[i].Name == tableName)
                {
                    //comprobar que el número de valores es igual que el numero de columnas
                   if(values.Count == Tables[i].NumColumns())
                    {
                        Tables[i].Insert(values);
                        LastErrorMessage = Constants.InsertSuccess;
                        return true;
                    }
                    LastErrorMessage = Constants.ColumnCountsDontMatch;
                    return false;

                }
            }

            //no hay ninguna tabla con ese nombre
            LastErrorMessage = Constants.TableDoesNotExistError;
            return false;
            
            
        }

        public Table Select(string tableName, List<string> columns, Condition condition)
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table
            Table table = TableByName(tableName);
            if(table != null)
            {
                foreach (string column in columns)
                {
                    if (table.ColumnByName(column) == null)
                    {
                        LastErrorMessage = Constants.ColumnDoesNotExistError;
                        return null;
                    }
                }
                return table.Select(columns, condition);
            }
            else
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return null;
            }
        }

        public bool DeleteWhere(string tableName, Condition columnCondition)
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true
            
            return false;
            
        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition)
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            Table table = TableByName(tableName);
            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            //Column in the condition exist?
            if (columnCondition != null)
            {
                if (table.ColumnIndexByName(columnCondition.ColumnName) == -1)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return false;
                }
            }

            table.Update(columnNames, columnCondition);
            return true;
        }






 
        public bool Save(string databaseName)
        {
            //DEADLINE 1.C: Save this database to disk with the given name
            //If everything goes ok, return true, false otherwise.
            //DEADLINE 5: Save the SecurityManager so that it can be loaded with the database in Load()
            try
            {
                string filename = databaseName + ".db";
                using (StreamWriter w = new StreamWriter(filename))
                {
                    w.WriteLine(m_username);
                    w.WriteLine(m_password);
                    w.WriteLine(Tables.Count);

                    //guardo cada tabla
                    foreach(Table table in Tables)
                    {
                        //info tabla
                        w.WriteLine(table.Name);
                        w.WriteLine(table.NumColumns());
                        w.WriteLine(table.NumRows());

                        //columnas
                        for(int i = 0; i < table.NumColumns(); i++)
                        {
                            ColumnDefinition column = table.GetColumn(i);
                            w.WriteLine(column.ToString());
                        }

                        //filas
                        for (int j = 0; j < table.NumRows() ; j++)
                        {
                            Row r = table.GetRow(j);
                            string row = "";

                            for(int x = 0; x<r.Values.Count; x++)
                            {
                                string v = r.Values[x];
                                //separadores
                                if (v.Contains(",") || v.Contains("\""))
                                {
                                    v = "\"" + v.Replace("\"", "\"\"") + "\"";
                                }

                                row += v;

                                //comas entre valores menos el ultimo
                                if (x < r.Values.Count - 1)
                                {
                                    row += ",";
                                }
                            }
                            w.WriteLine(row);
                        }
                    }
                }
                return true;
            }
            catch 
            {
                return false;
            }

        }

        public static Database Load(string databaseName, string username, string password)
        {
            return null;
        }

        public string ExecuteMiniSQLQuery(string query)
        {
            //Parse the query
            MiniSqlQuery miniSQLQuery = MiniSQLParser.Parse(query);

            //If the parser returns null, there must be a syntax error (or the parser is failing)
            if (miniSQLQuery == null)
                return Constants.SyntaxError;

            //Once the query is parsed, we run it on this database
            return miniSQLQuery.Execute(this);
        }


        public bool IsUserAdmin()
        {
            return SecurityManager.IsUserAdmin();
        }





        //All these methods are ONLY FOR TESTING. Use them to simplify creating unit tests:
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminPassword";
        public static Database CreateTestDatabase()
        {
            Database database = new Database(AdminUsername, AdminPassword);

            database.Tables.Add(Table.CreateTestTable());

            return database;
        }

        public void AddTuplesForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);
            foreach (List<string> row in rows)
            {
                table.Insert(row);
            }
        }

        public void CheckForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);

            table.CheckForTesting(rows);
        }
    }
}





