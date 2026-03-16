namespace MonAssurance.Models;

/// <summary>
/// Représente un véhicule à assurer - Version ANTI-CALISTHENICS
/// Cette classe viole volontairement les principes Object Calisthenics pour montrer les mauvaises pratiques.
/// 
/// Violations :
/// - Règle 2 : 10+ propriétés au lieu d'une par classe
/// - Règle 3 : Trop de paramètres au constructeur (9)
/// - Règle 4 : Collection exposée directement sans wrapper (List<Sinistre> publique)
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
    
    // VIOLATION RÈGLE 4 : Collection directement exposée sans encapsulation
    // N'importe qui peut ajouter/supprimer/modifier les sinistres
    public List<Sinistre> Sinistres { get; set; } = new();

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
        Sinistres = new List<Sinistre>();
    }

    // Violation Règle 8 : Logique métier mixée directement dans la classe
    public decimal CalculerPrimeTotale()
    {
        // Violation Règle 5 : Appels imbriqués (nested calls)
        return this.CalculerPrimeBase() * this.CalculerCoefficientUsure() * this.CalculerCoefficientSinistres();
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

    private decimal CalculerCoefficientSinistres()
    {
        if (this.Sinistres.Count == 0)
            return 1.0m;

        decimal coeff = 1.0m;
        // Logique métier complexe et dispersée
        foreach (var sinistre in this.Sinistres)
        {
            if (sinistre.ResponsableEntierement)
                coeff = coeff * 1.15m;
            else
                coeff = coeff * 1.05m;

            // Surcharge supplémentaire pour sinistres graves
            if (sinistre.MontantDegats > 5000)
                coeff = coeff * 1.10m;
        }

        return coeff;
    }

    /// <summary>
    /// Consultation des sinistres - exemple de méthode exposant les données brutes
    /// </summary>
    public List<Sinistre> ConsulterSinistres()
    {
        return this.Sinistres; // Retourne la référence directe (violation Règle 4)
    }

    /// <summary>
    /// Ajouter un sinistre directement (mutation pas contrôlée)
    /// </summary>
    public void AjouterSinistre(Sinistre sinistre)
    {
        this.Sinistres.Add(sinistre);
        this.NombreSinistres = this.Sinistres.Count;
        this.EstSinistre = this.Sinistres.Count > 0;
    }

    /// <summary>
    /// Obtenir le détail d'un sinistre
    /// </summary>
    public string ObtenirDetailSinistre(int idSinistre)
    {
        var sinistre = this.Sinistres.FirstOrDefault(s => s.Id == idSinistre);
        if (sinistre == null)
            return "Sinistre non trouvé";

        return $"Sinistre #{sinistre.Id}\n" +
               $"Date: {sinistre.DateSinistre:dd/MM/yyyy}\n" +
               $"Type: {sinistre.TypeSinistre}\n" +
               $"Description: {sinistre.Description}\n" +
               $"Montant dégâts: {sinistre.MontantDegats:C}\n" +
               $"Responsable: {(sinistre.ResponsableEntierement ? "Entièrement" : "Partiellement")}\n" +
               $"Référence: {sinistre.ReferenceAssureur}";
    }

    /// <summary>
    /// Montant total des sinistres
    /// </summary>
    public decimal MontantTotalSinistres()
    {
        return this.Sinistres.Sum(s => s.MontantDegats);
    }
}
