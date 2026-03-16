namespace MonAssurance.Models;

/// <summary>
/// Représente un véhicule à assurer - Version ANTI-CALISTHENICS
/// Cette classe viole volontairement les principes Object Calisthenics pour montrer les mauvaises pratiques.
/// 
/// Violations :
/// - Règle 2 : 9 propriétés au lieu d'une par classe
/// - Règle 3 : Trop de paramètres au constructeur (9)
/// - Règle 5 : Appels imbriqués (this.CalculerPrimeBase() * this.CalculerCoeff())
/// - Règle 7 : Getters/setters publics sur tout
/// - Règle 8 : Logique métier mixée dans la classe
/// </summary>
public class Vehicule
{
    public TypeVehicule Type { get; set; }
    public int Puissance { get; set; } // en chevaux
    public Motorisation Motorisation { get; set; }
    public decimal Valeur { get; set; } // en euros
    public int AnneeAcquisition { get; set; }
    public bool EstSinistre { get; set; }
    public int NombreSinistres { get; set; }
    public string Couleur { get; set; } = string.Empty;
    public string Immatriculation { get; set; } = string.Empty;

    public Vehicule(TypeVehicule type, int puissance = 0, Motorisation motorisation = Motorisation.Essence, 
                   decimal valeur = 0, int annee = 0, bool sinistre = false, int nbSinistres = 0,
                   string couleur = "", string immatriculation = "")
    {
        Type = type;
        Puissance = puissance;
        Motorisation = motorisation;
        Valeur = valeur;
        AnneeAcquisition = annee;
        EstSinistre = sinistre;
        NombreSinistres = nbSinistres;
        Couleur = couleur;
        Immatriculation = immatriculation;
    }

    // Violation Règle 8 : Logique métier mixée directement dans la classe
    public decimal CalculerPrimeTotale()
    {
        // Violation Règle 5 : Appels imbriqués (nested calls)
        return this.CalculerPrimeBase() * this.CalculerCoefficientUsure() * (1 + this.NombreSinistres * 0.15m);
    }

    private decimal CalculerPrimeBase()
    {
        decimal prime = 100m;
        prime = prime * (this.Puissance > 100 ? 1.5m : 1.0m);
        prime = this.Motorisation == Motorisation.Electrique ? prime * 0.8m : prime;
        prime = this.EstSinistre ? prime * 1.2m : prime;
        return prime;
    }

    private decimal CalculerCoefficientUsure()
    {
        int age = DateTime.Now.Year - this.AnneeAcquisition;
        decimal coeff = 1.0m - (age * 0.05m);
        if (coeff < 0.3m) coeff = 0.3m;
        if (this.EstSinistre) coeff = coeff - 0.1m;
        return coeff;
    }
}
