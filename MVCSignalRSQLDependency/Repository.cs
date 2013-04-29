using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MVCSignalRSQLDependency.Models;
using Microsoft.AspNet.SignalR;

namespace MVCSignalRSQLDependency
{
    public interface IRepository
    {
        IEnumerable<Person> GetPersons();
    }

    public class Repository : IRepository
    {
        private readonly IHubContext _hubContext;

        public Repository(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public IEnumerable<Person> GetPersons()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"SELECT [ID],[FirstName],[LastName] FROM [dbo].[Person]", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += (x, y) => _hubContext.Clients.All.displayStatus(GetPersons());

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Cast<IDataRecord>()
                        .Select(x => new Person
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
}