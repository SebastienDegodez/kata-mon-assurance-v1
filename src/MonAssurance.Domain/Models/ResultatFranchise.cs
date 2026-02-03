namespace MonAssurance.Domain.Models;

/// <summary>
/// Représente le résultat du calcul de franchise
/// </summary>
public class ResultatFranchise
{
    /// <summary>
    /// Montant de la franchise en euros
    /// </summary>
    public decimal Montant { get; set; }

    /// <summary>
    /// Détails du calcul (pour audit et traçabilité)
    /// </summary>
    public string Details { get; set; } = string.Empty;

    public ResultatFranchise(decimal montant, string details = "")
    {
        Montant = montant;
        Details = details;
    }
}
