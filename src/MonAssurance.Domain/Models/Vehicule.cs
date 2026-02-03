namespace MonAssurance.Domain.Models;

/// <summary>
/// Représente un véhicule à assurer
/// </summary>
public class Vehicule
{
    public TypeVehicule Type { get; set; }
    public int Puissance { get; set; } // en chevaux

    public Vehicule(TypeVehicule type, int puissance = 0)
    {
        Type = type;
        Puissance = puissance;
    }
}
