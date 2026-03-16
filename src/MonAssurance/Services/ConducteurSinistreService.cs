namespace MonAssurance.Services;

using MonAssurance.Models;

/// <summary>
/// Service pour gérer les sinistres d'un conducteur
/// </summary>
public class ConducteurSinistreService
{
    /// <summary>
    /// Ajoute un sinistre à un conducteur
    /// </summary>
    /// <param name="conducteur">Le conducteur</param>
    /// <param name="sinistre">Le sinistre à ajouter</param>
    public void AjouterSinistre(Conducteur conducteur, Sinistre sinistre)
    {
        conducteur.Sinistres.Add(sinistre);
    }

    /// <summary>
    /// Supprime les sinistres d'un conducteur qui datent de plus de X ans
    /// </summary>
    /// <param name="conducteur">Le conducteur</param>
    /// <param name="nombreAns">Le nombre d'années à partir duquel les sinistres sont supprimés</param>
    public void SupprimerSinistresAnciens(Conducteur conducteur, int nombreAns)
    {
        var dateReference = DateTime.Now.AddYears(-nombreAns);
        var sinistresASupprimer = conducteur.Sinistres.Where(s => s.DateSinistre < dateReference).ToList();
        foreach (var sinistre in sinistresASupprimer)
        {
            conducteur.Sinistres.Remove(sinistre);
        }
    }

    /// <summary>
    /// Vérifie si un conducteur a eu un sinistre récemment (dans les X derniers jours)
    /// </summary>
    /// <param name="conducteur">Le conducteur à vérifier</param>
    /// <param name="joursRecents">Le nombre de jours considérés comme "récents"</param>
    /// <returns>True si le conducteur a un sinistre récent, false sinon</returns>
    public bool EstSinistreRecemment(Conducteur conducteur, int joursRecents)
    {
        var dateReference = DateTime.Now.AddDays(-joursRecents);
        return conducteur.Sinistres.Any(s => s.DateSinistre >= dateReference);
    }

    /// <summary>
    /// Calcule le montant total des sinistres d'un conducteur
    /// </summary>
    /// <param name="conducteur">Le conducteur</param>
    /// <returns>Le montant total des sinistres</returns>
    public decimal CalculerMontantTotalSinistres(Conducteur conducteur)
    {
        return conducteur.Sinistres.Sum(s => s.MontantDegats);
    }

    /// <summary>
    /// Obtient le nombre de sinistres d'un conducteur
    /// </summary>
    /// <param name="conducteur">Le conducteur</param>
    /// <returns>Le nombre de sinistres</returns>
    public int ObtenirNombreSinistres(Conducteur conducteur)
    {
        return conducteur.Sinistres.Count;
    }

    /// <summary>
    /// Calcule le coefficient de surprime basé sur les sinistres du conducteur
    /// </summary>
    /// <param name="conducteur">Le conducteur</param>
    /// <returns>Le coefficient de surprime (1.0 = pas de variation)</returns>
    public decimal CalculerCoefficientSurprimeSinistres(Conducteur conducteur)
    {
        if (conducteur.Sinistres.Count == 0)
            return 1.0m;

        // Chaque sinistre ajoute un coefficient de base 1.05
        decimal coeff = 1.0m;
        
        foreach (var sinistre in conducteur.Sinistres)
        {
            coeff *= 1.05m;  // Base: +5% par sinistre
            
            // Majoratio  n supplémentaire si montant > 5000
            if (sinistre.MontantDegats > 5000m)
            {
                coeff *= 1.10m;  // +10% en plus pour gros dégâts
            }
        }

        return coeff;
    }
}

