namespace MonAssurance.Services;

using MonAssurance.Models;

/// <summary>
/// Service de gestion du risque et calcul de franchise
/// </summary>
public class GestionRisqueService
{
    private const decimal FranchiseStandard = 500m;
    private const decimal TauxFranchiseMalus = 0.10m; // 10% de la valeur du véhicule
    private const decimal FranchiseMobiliteDouce = 50m;
    private const decimal SeuilMalus = 1.00m;

    /// <summary>
    /// Calcule la franchise en cas d'accident
    /// </summary>
    /// <param name="conducteur">Le conducteur</param>
    /// <param name="vehicule">Le véhicule</param>
    /// <returns>Résultat contenant le montant de la franchise et les détails du calcul</returns>
    public ResultatFranchise CalculerFranchise(Conducteur conducteur, Vehicule vehicule)
    {
        // Cas 1: Mobilité douce (Vélo électrique, Trottinette électrique)
        if (vehicule.Type == TypeVehicule.VeloElectrique || vehicule.Type == TypeVehicule.TrottinetteElectrique)
        {
            return new ResultatFranchise(
                FranchiseMobiliteDouce,
                $"Franchise réduite pour mobilité douce (type: {vehicule.Type})"
            );
        }

        // Cas 2: Conducteur malussé (coefficient > 1.00)
        if (conducteur.CoefficientBonusMalus > SeuilMalus)
        {
            var franchisePunitive = vehicule.Valeur * TauxFranchiseMalus;
            return new ResultatFranchise(
                franchisePunitive,
                $"Franchise punitive pour conducteur malussé (coefficient: {conducteur.CoefficientBonusMalus}), calculée à {TauxFranchiseMalus * 100}% de la valeur du véhicule"
            );
        }

        // Cas 3: Franchise standard (conducteur avec bonus ou neutre)
        return new ResultatFranchise(
            FranchiseStandard,
            $"Franchise standard pour conducteur avec coefficient bonus-malus de {conducteur.CoefficientBonusMalus}"
        );
    }
}
