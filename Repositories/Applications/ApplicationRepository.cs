using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.EntityFrameworkCore;

namespace MyBudgetManagement.Repositories.Applications;

public class ApplicationRepository : IApplicationRepository
{

    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetStoredProcedure()
    {
        List<string> storedProcedures = new List<string>();
        using (var conn = new SqlConnection(_context.Database.GetConnectionString()))
        {
            try
            {
                conn.Open();
                var sql = "Select name from sys.procedures";
                var command = new SqlCommand(sql, conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        storedProcedures.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return string.Join(";", storedProcedures);
    }

   
}