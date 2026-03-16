namespace MonAssurance.Models;

/// <summary>
/// Représente un sinistre avec détails.
/// Données mutables directement modifiables (violation Règle 7 des calisthenics).
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
