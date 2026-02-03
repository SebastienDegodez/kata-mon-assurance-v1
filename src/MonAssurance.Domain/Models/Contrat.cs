namespace MonAssurance.Domain.Models;

/// <summary>
/// Représente un contrat d'assurance
/// </summary>
public class Contrat
{
    public decimal TarifBase { get; set; }
    public TypeStationnement TypeStationnement { get; set; }
    public int KilometrageAnnuel { get; set; }
    public UsageVehicule Usage { get; set; }
    public TypeVehicule TypeVehicule { get; set; }

    public Contrat(decimal tarifBase)
    {
        TarifBase = tarifBase;
        TypeStationnement = TypeStationnement.Rue;
        Usage = UsageVehicule.TrajetsDomicileTravail;
    }
}
