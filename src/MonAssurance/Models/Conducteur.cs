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

    public List<Sinistre> Sinistres { get; set; } = new();

    /// <summary>
    /// Calcule l'âge du conducteur à une date donnée
    /// </summary>
    public int CalculerAge(DateTime dateReference)
    {
        var age = dateReference.Year - this.DateNaissance.Year;
        if (dateReference < this.DateNaissance.AddYears(age))
        {
            age--;
        }
        return age;
    }

    /// <summary>
    /// Calcul du coefficient bonus/malus basé sur ancienneté
    /// </summary>
    public decimal CalculerCoefficientBonus()
    {
        decimal coeff = this.CoefficientBonusMalus;
        
        if (this.AnneesPermis < 3)
            coeff = coeff * 1.25m; // Jeune conducteur : +25% malus
        else if (this.AnneesPermis >= 10)
            coeff = coeff * 0.90m; // Expérimenté : -10% bonus
            
        return coeff;
    }

    /// <summary>
    /// Consultation des sinistres du conducteur
    /// </summary>
    public List<Sinistre> ConsulterSinistres()
    {
        return this.Sinistres; // Expose directement l'état interne
    }
}
