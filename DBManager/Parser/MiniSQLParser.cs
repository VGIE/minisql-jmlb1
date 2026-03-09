using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = null;

            //LEIRE --> #16
            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s*\(([^)]+)\)";

            const string dropTablePattern = null;
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = null;

            //LEIRE
            const string updateTablePattern = null;
            
            const string deletePattern = null;
            

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null;
            
            const string dropSecurityProfilePattern = null;
            
            const string grantPattern = null;
            
            const string revokePattern = null;
            
            const string addUserPattern = null;
            
            const string deleteUserPattern = null;


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            //Insert
            Match matchInsert = Regex.Match(miniSQLQuery, insertPattern);
            if (matchInsert.Success)
            {
                string table = matchInsert.Groups[1].Value;
                string valores = matchInsert.Groups[2].Value;
                //"'Juan', 28"

                string newPattern = @"'[^']+'|\d+\.\d+|\d+";
                MatchCollection matchCollectionComillas = Regex.Matches(valores, newPattern);

                List<string> values = new List<string>();

                foreach (Match match in matchCollectionComillas)
                {
                    string valor = match.Value;
                    valor = valor.Trim('\'');
                    values.Add(valor);
                }

                return new Insert(table, values);
            }


            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            return null;
           
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}
