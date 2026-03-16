namespace MonAssurance.Models;

/// <summary>
/// Représente un sinistre avec détails.
/// Violation convention: définie dans le même fichier que Conducteur au lieu d'un fichier séparé.
/// Données mutables directement modifiables.
/// </summary>
public class Sinistre
{
    public int Id { get; set; }
    public DateTime DateSinistre { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal MontantDegats { get; set; }
    public string TypeSinistre { get; set; } = string.Empty; // "Collision", "Vol", "Vandalisme", etc.
    public bool ResponsableEntierement { get; set; }
    public string ReferenceAssureur { get; set; } = string.Empty;

    public Sinistre()
    {
    }

    public Sinistre(int id, DateTime dateSinistre, string description, decimal montant, 
                   string type, bool responsable, string reference)
    {
        Id = id;
        DateSinistre = dateSinistre;
        Description = description;
        MontantDegats = montant;
        TypeSinistre = type;
        ResponsableEntierement = responsable;
        ReferenceAssureur = reference;
    }

    public override string ToString()
    {
        return $"#{Id} - {DateSinistre:dd/MM/yyyy} - {TypeSinistre} ({MontantDegats:C})";
    }
}

/// <summary>
/// Représente un conducteur souhaitant souscrire à une assurance - Version ANTI-CALISTHENICS
/// 
/// Violations :
/// - Règle 2 : 4 propriétés au lieu d'une par classe (incluant sinistres)
/// - Règle 4 : Collection List<Sinistre> exposée directement sans wrapper
/// - Règle 7 : Getters/setters publics sur tout
/// - Règle 8 : Logique métier (CalculerAge, CalculerCoefficientBonus) mixée dans la classe
/// </summary>
public class Conducteur
{
    public DateTime DateNaissance { get; set; }
    public int AnneesPermis { get; set; }
    
    /// <summary>
    /// Coefficient bonus-malus (0.50 = 50% bonus, 1.00 = neutre, 1.25 = 25% malus)
    /// </summary>
    public decimal CoefficientBonusMalus { get; set; } = 1.00m;

    // VIOLATION RÈGLE 4 : Collection directement exposée sans encapsulation
    // N'importe qui peut ajouter/supprimer/modifier les sinistres du conducteur
    public List<Sinistre> Sinistres { get; set; } = new();

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

    /// <summary>
    /// Consultation des sinistres du conducteur
    /// Violation Règle 4 : retourne la référence directe à la liste
    /// </summary>
    public List<Sinistre> ConsulterSinistres()
    {
        return this.Sinistres; // Expose directement l'état interne
    }

    /// <summary>
    /// Ajouter un sinistre au conducteur
    /// </summary>
    public void AjouterSinistre(Sinistre sinistre)
    {
        this.Sinistres.Add(sinistre);
    }

    /// <summary>
    /// Obtenir le nombre total de sinistres
    /// </summary>
    public int NombreTotalSinistres => this.Sinistres.Count;

    /// <summary>
    /// Montant total des sinistres du conducteur
    /// </summary>
    public decimal MontantTotalSinistres()
    {
        return this.Sinistres.Sum(s => s.MontantDegats);
    }
}
