using DbManager.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"^SELECT\s+([a-zA-Z0-9]+(?:,[a-zA-Z0-9]+)*)\s+FROM\s+([a-zA-Z0-9]+)(?:\s+WHERE\s+([a-zA-Z0-9]+)(=|<|>)'(-?[0-9]+(?:\.[0-9]+)?|[a-zA-Z]+(?:\s[a-zA-Z]+)*)')?$";

            const string dropTablePattern = @"^DROP\s+TABLE\s+([a-zA-Z0-9]+)$";


            //LEIRE --> #16
            const string insertPattern = @"INSERT\s+INTO\s+([\w_]+)\s+VALUES\s+\(('[^',]*'(?:,'(?:[^',]*)')*)\)$";


            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed

            const string createTablePattern = @"CREATE\s+TABLE\s+([a-zA-Z][a-zA-Z0-9]*)\s+\(\s*(.*?)\s*\)";
            //const string createTablePattern = @"CREATE\s+TABLE\s+([a-zA-Z][a-zA-Z0-9]*)\s+\(\s*(.*?)\s*\)";
            //const string createTablePattern = @"CREATE\s+TABLE\s+([a-zA-Z][a-zA-Z0-9]*)\s*\((.*)\)";

            //LEIRE --> #26
            const string updateTablePattern = @"UPDATE\s+(\w+)\s+SET\s+(.+?)(?:\s+WHERE\s+(\w+)\s*([=<>])\s*('[^']*'))?$";

            const string deletePattern = @"^DELETE\s+FROM\s+([a-zA-Z0-9]+)\s+WHERE\s+([a-zA-Z0-9]+)(>|<|=)'([^']*)'$";

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null;

            const string dropSecurityProfilePattern = null;

            const string grantPattern = null;

            const string revokePattern = null;

            const string addUserPattern = @"ADD\s+USER\s+\(([a-zA-Z][a-zA-Z0-9]*),([^,]+),([a-zA-Z][a-zA-Z0-9]*)\)$"; 

            const string deleteUserPattern = null;


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            //droptable
            Match dropMatch = Regex.Match(miniSQLQuery, dropTablePattern);
            if (dropMatch.Success)
            {
                string tableName = dropMatch.Groups[1].Value;
                return new DropTable(tableName);
            }


            //select
            Match matchSelect = Regex.Match(miniSQLQuery, selectPattern);

            if (matchSelect.Success)
            {
                string column = matchSelect.Groups[1].Value;
                //Separar las columas
                List<string> columns = CommaSeparatedNames(column);

                string table = matchSelect.Groups[2].Value;

                Condition cond = null;

                //si hay where
                if (matchSelect.Groups[3].Success) 
                {
                    string colum = matchSelect.Groups[3].Value;
                    string op = matchSelect.Groups[4].Value;
                    string valor = matchSelect.Groups[5].Value;

                    cond = new Condition(colum, op, valor);
                }
                if (cond != null)
                {
                    return new Select(table, columns, cond);
                }
                else
                {
                    return new Select(table, columns, null);
                }
            }


            //Insert
            Match matchInsert = Regex.Match(miniSQLQuery, insertPattern);
            if (matchInsert.Success)
            {
                string table = matchInsert.Groups[1].Value;
                string valores = matchInsert.Groups[2].Value;

                string[] valoresArray = valores.Split(',');
                List<string> values = new List<string>();

                foreach (string valor in valoresArray)
                {
                    // Quitar las comillas simples
                    string valorSinComillas = valor.Trim('\'');
                    values.Add(valorSinComillas);
                }

                return new Insert(table, values);
            }


            const string Int_Type = "INT";
            const string Text_Type = "TEXT";
            const string Double_Type = "DOUBLE";

            //create table
            Match createMatch = Regex.Match(miniSQLQuery, createTablePattern);

            if (createMatch.Success)
            {
                string nombreTabla = createMatch.Groups[1].Value;
                string columnasText = createMatch.Groups[2].Value;

                List<ColumnDefinition> crearColumnas = new List<ColumnDefinition>();

                if (!string.IsNullOrWhiteSpace(columnasText))
                {
                    // Dividir por comas
                    string[] columnaSep = columnasText.Split(',');

                    foreach (string parte in columnaSep)
                    {
                        string parteTrim = parte.Trim();
                        // Dividir por espacio
                        string[] columna = parteTrim.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (columna.Length == 2)
                        {
                            string name = columna[0].Trim();
                            string tipo = columna[1].Trim().ToUpper();

                            ColumnDefinition.DataType columnType;
                            if (tipo == Int_Type)
                            {
                                columnType = ColumnDefinition.DataType.Int;
                            }
                            else if (tipo == Text_Type)
                            {
                                columnType = ColumnDefinition.DataType.String;
                            }
                            else if (tipo == Double_Type)
                            {
                                columnType = ColumnDefinition.DataType.Double;
                            }
                            else
                            {
                                return null;
                            }

                            crearColumnas.Add(new ColumnDefinition(columnType, name));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return new CreateTable(nombreTabla, crearColumnas);
            }

            //addUser
            Match matchAddUser = Regex.Match(miniSQLQuery, addUserPattern);
            if (matchAddUser.Success)
            {
                string username = matchAddUser.Groups[1].Value;
                string password = matchAddUser.Groups[2].Value;
                string securityProfile = matchAddUser.Groups[3].Value;

                if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(securityProfile))
                {
                    return null;
                }
                return new AddUser(username, password, securityProfile);
            }


            //Update
            Match matchUpdate = Regex.Match(miniSQLQuery, updateTablePattern);
            if (matchUpdate.Success)
            {
                string table = matchUpdate.Groups[1].Value;
                string valores = matchUpdate.Groups[2].Value;
                string whereColumn = null;
                string whereOp = null;
                string whereValue = null;

                if (matchUpdate.Groups[3].Success)
                {
                    whereColumn = matchUpdate.Groups[3].Value;
                    whereOp = matchUpdate.Groups[4].Value;
                    whereValue = matchUpdate.Groups[5].Value;
                }

                if (miniSQLQuery.ToUpper().Contains("WHERE") && (whereOp == null || whereValue == null))
                {
                    return null;
                }

                string setPartSinComillas = Regex.Replace(valores, @"'[^']*'", "#");
                if (setPartSinComillas.Contains(" ,") || setPartSinComillas.Contains(", "))
                {
                    return null;
                }

                List<SetValue> setValues = new List<SetValue>();
                string[] assignments = Regex.Split(valores, @",(?=(?:[^']*'[^']*')*[^']*$)");

                foreach (string assignment in assignments)
                {
                    if (!assignment.Contains("="))
                    {
                        return null;
                    }

                    string[] parts = assignment.Split('=');
                    if (parts.Length != 2)
                    {
                        return null;
                    }

                    string column = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (!value.StartsWith("'") || !value.EndsWith("'"))
                    {
                        return null;
                    }

                    value = value.Substring(1, value.Length - 2);
                    setValues.Add(new SetValue(column, value));
                }

                if (setValues.Count == 0)
                {
                    return null;
                }

                // Parsear WHERE
                Condition condition = null;
                if (whereColumn != null)
                {
                    // Solo aceptar valores entre comillas simples
                    if (!whereValue.StartsWith("'") || !whereValue.EndsWith("'"))
                    {
                        return null;
                    }

                    whereValue = whereValue.Substring(1, whereValue.Length - 2);
                    condition = new Condition(whereColumn, whereOp, whereValue);
                }

                return new Update(table, setValues, condition);
            }

            //Delete
            Match matchDelete = Regex.Match(miniSQLQuery, deletePattern);
            if (matchDelete.Success)
            {
                string table = matchDelete.Groups[1].Value;
                string whereColumn = matchDelete.Groups[2].Value;
                string whereOperator = matchDelete.Groups[3].Value;
                string whereValue = matchDelete.Groups[4].Value;

                Condition condition = new Condition(whereColumn, whereOperator, whereValue);
                return new Delete(table, condition);
            }



            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            return null;

        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for (int i = 0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }

    }
}
