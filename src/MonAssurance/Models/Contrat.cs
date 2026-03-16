namespace MonAssurance.Models;

/// <summary>
/// Représente un contrat d'assurance - Version ANTI-CALISTHENICS
/// 
/// Violations :
/// - Règle 2 : 5 propriétés au lieu d'une par classe
/// - Règle 3 : Constructeur à responsabilités mixées
/// - Règle 7 : Getters/setters publics sur tout
/// - Règle 8 : Validation et règles métier directement dans la classe
/// </summary>
public class Contrat
{
    public decimal TarifBase { get; set; }
    public TypeStationnement TypeStationnement { get; set; }
    public int KilometrageAnnuel { get; set; }
    public UsageVehicule Usage { get; set; }
    public TypeVehicule TypeVehicule { get; set; }

    public Contrat(decimal tarifBase = 0)
    {
        TarifBase = tarifBase;
        TypeStationnement = TypeStationnement.Rue;
        Usage = UsageVehicule.TrajetsDomicileTravail;
    }

    /// <summary>
    /// Valide le contrat selon plusieurs critères disparates
    /// Violation Règle 8 : Logique métier mixée dans la classe
    /// </summary>
    public bool ValiderContrat(int ageDriver)
    {
        // Critères mélangés sans séparation des responsabilités
        if (ageDriver < 18)
            return false;

        if (this.KilometrageAnnuel > 50000 && this.TypeStationnement == TypeStationnement.Rue)
            return false;

        if (this.Usage == UsageVehicule.Livraison && this.TypeVehicule == TypeVehicule.Voiture)
            return false;

        return true;
    }

    /// <summary>
    /// Calcule des réductions multiples sans séparation concern
    /// Violation Règle 5 et 8
    /// </summary>
    public decimal CalculerReductionCumulee(Vehicule vehicule, Conducteur conducteur)
    {
        decimal reduction = 1.0m;

        // Chaos : mélange de validations métier ici
        if (this.KilometrageAnnuel < 5000)
            reduction = reduction * 0.85m;

        if (vehicule.Motorisation == Motorisation.Electrique)
            reduction = reduction * 0.90m;

        if (conducteur.CoefficientBonusMalus < 1.0m)
            reduction = reduction * (1.0m - (1.0m - conducteur.CoefficientBonusMalus));

        return reduction;
    }
}
