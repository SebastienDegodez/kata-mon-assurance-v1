namespace MonAssurance.Models;

/// <summary>
/// Résultat de validation d'usage véhicule
/// </summary>
public class ResultatValidationUsage
{
    public bool EstValide { get; set; }
    public string Motif { get; set; } = string.Empty;

    public ResultatValidationUsage(bool estValide, string motif = "")
    {
        EstValide = estValide;
        Motif = motif;
    }
}
