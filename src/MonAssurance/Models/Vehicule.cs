namespace MonAssurance.Models;

/// <summary>
/// Représente un véhicule à assurer
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

    public decimal CalculerPrimeTotale()
    {
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
