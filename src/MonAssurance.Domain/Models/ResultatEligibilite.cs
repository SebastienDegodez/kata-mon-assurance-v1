namespace MonAssurance.Domain.Models;

/// <summary>
/// Représente le résultat d'une vérification d'éligibilité
/// </summary>
public class ResultatEligibilite
{
    public bool EstAcceptee { get; set; }
    public string? MotifRefus { get; set; }

    public static ResultatEligibilite Accepte()
    {
        return new ResultatEligibilite { EstAcceptee = true };
    }

    public static ResultatEligibilite Refuse(string motif)
    {
        return new ResultatEligibilite 
        { 
            EstAcceptee = false, 
            MotifRefus = motif 
        };
    }
}
