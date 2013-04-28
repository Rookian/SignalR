using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MVCSignalRSQLDependency.Controllers;
using MVCSignalRSQLDependency.Hubs;

namespace MVCSignalRSQLDependency
{
    public class Repository
    {
        public IEnumerable<Person> Get()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [ID],[FirstName],[LastName] FROM [dbo].[Person]", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += (sender, e) => ChatHub.Show();

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                        return reader.Cast<IDataRecord>()
                            .Select(x => new Person()
                            {
                                ID = x.GetInt64(0),
                                FirstName = x.GetString(1),
                                LastName = x.GetString(2),
                            }).ToList();
                }
            }
        } 
    }
}