namespace MonAssurance.Domain.Models;

/// <summary>
/// Représente un conducteur souhaitant souscrire à une assurance
/// </summary>
public class Conducteur
{
    public DateTime DateNaissance { get; set; }
    public int AnneesPermis { get; set; }

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
