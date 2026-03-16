namespace MonAssurance.Services;

using MonAssurance.Models;

/// <summary>
/// Service de gestion de l'usage du véhicule et du stationnement
/// </summary>
public class UsageVehiculeService
{
    private const decimal ReductionStationnementSecurise = 0.05m; // 5%
    private const decimal RemisePetitRouleur = 0.10m; // 10%
    private const int SeuilPetitRouleur = 8000; // km/an

    /// <summary>
    /// Calcule le coefficient géographique selon le stationnement
    /// </summary>
    public decimal CalculerCoefficientGeographique(Contrat contrat)
    {
        if (contrat.TypeStationnement == TypeStationnement.GarageFermePrive)
        {
            return contrat.TarifBase * (1 - ReductionStationnementSecurise);
        }

        return contrat.TarifBase;
    }

    /// <summary>
    /// Applique le facteur kilométrique selon le kilométrage annuel
    /// </summary>
    public decimal AppliquerFacteurKilometrique(decimal tarifBase, int kilometrageAnnuel)
    {
        if (kilometrageAnnuel < SeuilPetitRouleur)
        {
            return tarifBase * (1 - RemisePetitRouleur);
        }

        return tarifBase;
    }

    /// <summary>
    /// Valide l'usage du véhicule pour un contrat particulier
    /// </summary>
    public ResultatValidationUsage ValiderUsage(TypeVehicule typeVehicule, UsageVehicule usage)
    {
        // Les usages professionnels sont interdits pour les contrats particuliers
        if (usage == UsageVehicule.TransportPersonnesTitreOnereux)
        {
            return new ResultatValidationUsage(
                false, 
                "Usage professionnel non couvert par ce contrat"
            );
        }
        else if (usage == UsageVehicule.Livraison)
        {
            return new ResultatValidationUsage(
                false, 
                "Usage professionnel non couvert par ce contrat"
            );
        }
        else if (usage == UsageVehicule.UsageProfessionnel)
        {
            return new ResultatValidationUsage(
                false, 
                "Usage professionnel non couvert par ce contrat"
            );
        }
        else
        {
            return new ResultatValidationUsage(true);
        }
    }
}
