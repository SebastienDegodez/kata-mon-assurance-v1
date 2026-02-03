namespace MonAssurance.Domain.Models;

/// <summary>
/// Représente un véhicule à assurer
/// </summary>
public class Vehicule
{
    public TypeVehicule Type { get; set; }
    public int Puissance { get; set; } // en chevaux
    public Motorisation Motorisation { get; set; }
    public decimal Valeur { get; set; } // en euros

    public Vehicule(TypeVehicule type, int puissance = 0, Motorisation motorisation = Motorisation.Essence, decimal valeur = 0)
    {
        Type = type;
        Puissance = puissance;
        Motorisation = motorisation;
        Valeur = valeur;
    }
}
