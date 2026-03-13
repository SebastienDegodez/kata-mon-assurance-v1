using Microsoft.EntityFrameworkCore;

namespace MonAssurance.Data;

public class AssuranceDbContext(DbContextOptions<AssuranceDbContext> options) : DbContext(options)
{
    public DbSet<DemandeDevis> DemandesDevis => Set<DemandeDevis>();
}
