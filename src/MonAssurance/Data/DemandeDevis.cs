namespace MonAssurance.Data;

public class DemandeDevis
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreeLe { get; set; } = DateTime.UtcNow;

    public DateTime DateNaissance { get; set; }
    public int AnneesPermis { get; set; }
    public decimal CoefficientBonusMalus { get; set; }

    public string TypeVehicule { get; set; } = string.Empty;
    public int Puissance { get; set; }
    public string Motorisation { get; set; } = string.Empty;
    public decimal ValeurVehicule { get; set; }

    public bool EstAcceptee { get; set; }
    public string? MotifRefus { get; set; }
    public decimal? MontantFranchise { get; set; }
}
