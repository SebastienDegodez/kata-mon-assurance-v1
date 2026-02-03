namespace MonAssurance.Domain.Services;

using MonAssurance.Domain.Models;

/// <summary>
/// Service de vérification de l'éligibilité d'un conducteur à la souscription d'assurance
/// </summary>
public class EligibiliteService
{
    private const int AgeMinimumVoitureOuGrosseMoto = 18;
    private const int AgeMinimumTrottinetteElectrique = 16;
    private const int PuissanceMaximumSansExperience = 100;
    private const int AnneesPermisRequisesMotosPuissantes = 5;

    /// <summary>
    /// Vérifie l'éligibilité d'un conducteur pour un véhicule donné
    /// </summary>
    /// <param name="conducteur">Le conducteur souhaitant souscrire</param>
    /// <param name="vehicule">Le véhicule à assurer</param>
    /// <param name="dateReference">La date de référence pour le calcul (par défaut: aujourd'hui)</param>
    /// <returns>Le résultat de l'éligibilité</returns>
    public ResultatEligibilite VerifierEligibilite(
        Conducteur conducteur, 
        Vehicule vehicule, 
        DateTime? dateReference = null)
    {
        var date = dateReference ?? DateTime.Now;
        var age = conducteur.CalculerAge(date);

        // Règle : Il faut être majeur pour assurer une voiture ou une grosse moto
        if (vehicule.Type == TypeVehicule.Voiture || vehicule.Type == TypeVehicule.Moto)
        {
            if (age < AgeMinimumVoitureOuGrosseMoto)
            {
                return ResultatEligibilite.Refuse("Conducteur trop jeune pour ce véhicule");
            }
        }

        // Règle : On peut assurer des petits véhicules électriques dès 16 ans
        if (vehicule.Type == TypeVehicule.TrottinetteElectrique)
        {
            if (age < AgeMinimumTrottinetteElectrique)
            {
                return ResultatEligibilite.Refuse("Conducteur trop jeune pour ce véhicule");
            }
            // Si l'âge est suffisant, accepté
            return ResultatEligibilite.Accepte();
        }

        // Règle : Les motos extrêmement puissantes (> 100ch) nécessitent 5 ans de permis
        if (vehicule.Type == TypeVehicule.Moto && vehicule.Puissance > PuissanceMaximumSansExperience)
        {
            if (conducteur.AnneesPermis < AnneesPermisRequisesMotosPuissantes)
            {
                return ResultatEligibilite.Refuse("Expérience insuffisante pour la puissance");
            }
        }

        // Toutes les règles sont respectées
        return ResultatEligibilite.Accepte();
    }
}
