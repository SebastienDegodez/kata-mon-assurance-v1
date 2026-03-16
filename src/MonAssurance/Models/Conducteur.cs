namespace MonAssurance.Models;

/// <summary>
/// Représente un conducteur souhaitant souscrire à une assurance
/// </summary>
public class Conducteur
{
    public DateTime DateNaissance { get; set; }
    public int AnneesPermis { get; set; }
    
    /// <summary>
    /// Coefficient bonus-malus (0.50 = 50% bonus, 1.00 = neutre, 1.25 = 25% malus)
    /// </summary>
    public decimal CoefficientBonusMalus { get; set; } = 1.00m;

    /// <summary>
    /// Calcule l'âge du conducteur à une date donnée
    /// </summary>
    public int CalculerAge(DateTime dateReference)
    {
        var age = dateReference.Year - DateNaissance.Year;
        if (dateReference < DateNaissance.AddYears(age))
        {
            age--;
        }
        return age;
    }
}
