namespace MonAssurance.Models;

/// <summary>
/// Représente un conducteur souhaitant souscrire à une assurance - Version ANTI-CALISTHENICS
/// 
/// Violations :
/// - Règle 2 : 3 propriétés au lieu d'une par classe
/// - Règle 7 : Getters/setters publics sur tout
/// - Règle 8 : Logique métier (CalculerAge) mixée dans la classe
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
    /// Violation Règle 8 : Logique métier mixée dans la classe
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
    /// Violation Règle 8 : Logique métier mixée directement dans la classe
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
}
