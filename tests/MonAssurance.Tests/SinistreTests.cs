namespace MonAssurance.Tests;

using MonAssurance.Models;
using MonAssurance.Services;
using Xunit;

/// <summary>
/// Tests de la fonctionnalité de gestion des sinistres
/// Correspond au fichier: features/06_gestion_sinistres.feature
/// </summary>
public class SinistreTests
{
    private readonly ConducteurSinistreService _service;

    public SinistreTests()
    {
        _service = new ConducteurSinistreService();
    }

    /// <summary>
    /// Scénario: Ajout d'un sinistre à un conducteur
    /// </summary>
    [Fact]
    public void AjoutSinistreAuConducteur()
    {
        // Etant donné un conducteur nommé "Jean Dupont"
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-35),
            AnneesPermis = 10,
            CoefficientBonusMalus = 1.0m
        };

        // Et le conducteur n'a pas de sinistre
        Assert.Empty(conducteur.Sinistres);

        // Quand j'ajoute un sinistre d'un montant de 5000 €
        var sinistre = new Sinistre
        {
            MontantDegats = 5000m,
            DateSinistre = DateTime.Now
        };
        _service.AjouterSinistre(conducteur, sinistre);

        // Alors le conducteur a maintenant 1 sinistre
        Assert.Single(conducteur.Sinistres);

        // Et le montant total des sinistres est de 5000 €
        var montantTotal = _service.CalculerMontantTotalSinistres(conducteur);
        Assert.Equal(5000m, montantTotal);
    }

    /// <summary>
    /// Scénario: Accumulation de plusieurs sinistres
    /// </summary>
    [Fact]
    public void AccumulationPlusieurssSinistres()
    {
        // Etant donné un conducteur nommé "Marie Martin"
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-40),
            AnneesPermis = 15,
            CoefficientBonusMalus = 1.0m
        };

        // Et le conducteur a 2 sinistres précédents pour 3000 € et 2000 €
        var sinistre1 = new Sinistre { MontantDegats = 3000m, DateSinistre = DateTime.Now.AddDays(-100) };
        var sinistre2 = new Sinistre { MontantDegats = 2000m, DateSinistre = DateTime.Now.AddDays(-200) };
        _service.AjouterSinistre(conducteur, sinistre1);
        _service.AjouterSinistre(conducteur, sinistre2);

        // Quand j'ajoute un sinistre d'un montant de 4000 €
        var sinistre3 = new Sinistre { MontantDegats = 4000m, DateSinistre = DateTime.Now };
        _service.AjouterSinistre(conducteur, sinistre3);

        // Alors le conducteur a maintenant 3 sinistres
        Assert.Equal(3, _service.ObtenirNombreSinistres(conducteur));

        // Et le montant total des sinistres est de 9000 €
        var montantTotal = _service.CalculerMontantTotalSinistres(conducteur);
        Assert.Equal(9000m, montantTotal);
    }

    /// <summary>
    /// Scénario: Surcharge pour sinistre récent
    /// </summary>
    [Fact]
    public void SurchargeForSinistreRecent()
    {
        // Etant donné un conducteur nommé "Pierre Lefevre"
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-30),
            AnneesPermis = 5,
            CoefficientBonusMalus = 1.0m
        };

        // Et le conducteur a un sinistre depuis 200 jours
        var sinistre = new Sinistre
        {
            MontantDegats = 6000m,
            DateSinistre = DateTime.Now.AddDays(-200)
        };
        _service.AjouterSinistre(conducteur, sinistre);

        // Quand je vérifie les sinistres récents sur 365 jours
        var estSinistreRecent = _service.EstSinistreRecemment(conducteur, 365);

        // Alors le conducteur a un sinistre récent
        Assert.True(estSinistreRecent);

        // Et une surcharge de 25% est appliquée
        var coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(conducteur);
        Assert.True(coeffSurprime > 1.0m); // Le coefficient doit être supérieur à 1
    }

    /// <summary>
    /// Scénario: Pas de surcharge pour sinistre ancien
    /// </summary>
    [Fact]
    public void PasDeSurchargeForSinistreAncien()
    {
        // Etant donné un conducteur nommé "Sophie Bernard"
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-35),
            AnneesPermis = 8,
            CoefficientBonusMalus = 1.0m
        };

        // Et le conducteur a un sinistre depuis 500 jours
        var sinistre = new Sinistre
        {
            MontantDegats = 5000m,
            DateSinistre = DateTime.Now.AddDays(-500)
        };
        _service.AjouterSinistre(conducteur, sinistre);

        // Quand je vérifie les sinistres récents sur 365 jours
        var estSinistreRecent = _service.EstSinistreRecemment(conducteur, 365);

        // Alors le conducteur n'a pas de sinistre récent
        Assert.False(estSinistreRecent);
    }

    /// <summary>
    /// Scénario: Coefficient de surprime selon le nombre de sinistres
    /// </summary>
    [Fact]
    public void CoefficientSurprimeSelonNombreSinistres()
    {
        // Etant donné un conducteur nommé "Luc Rousseau"
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-45),
            AnneesPermis = 20,
            CoefficientBonusMalus = 1.0m
        };

        // Et le conducteur a 4 sinistres
        for (int i = 1; i <= 4; i++)
        {
            var sinistre = new Sinistre
            {
                MontantDegats = 2000m * i,
                DateSinistre = DateTime.Now.AddDays(-100 * i)
            };
            _service.AjouterSinistre(conducteur, sinistre);
        }

        // Quand je calcule le coefficient de surprime
        var coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(conducteur);

        // Alors le coefficient doit être positif et reflète les multiplicateurs appliqués
        // 4 sinistres: 2000, 4000, 6000, 8000
        // - 2000: 1.05 (non responsable)
        // - 4000: 1.05 (non responsable)
        // - 6000: 1.05 * 1.10 (montant > 5000)
        // - 8000: 1.05 * 1.10 (montant > 5000)
        // Total: 1.05 * 1.05 * 1.155 * 1.155 ≈ 1.47
        Assert.True(coeffSurprime > 1.0m, "Le coefficient doit être supérieur à 1.0");
        Assert.True(coeffSurprime < 2.0m, "Le coefficient doit être raisonnablement limité");
    }

    /// <summary>
    /// Scénario: Suppression des sinistres de plus de 2 ans
    /// </summary>
    [Fact]
    public void SuppressionSinistresPlus2Ans()
    {
        // Etant donné un conducteur nommé "Anne Dubois"
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-38),
            AnneesPermis = 12,
            CoefficientBonusMalus = 1.0m
        };

        // Et le conducteur a 3 sinistres: 1 récent (6 mois), 2 anciens (2.5 et 3 ans)
        var sinistre1 = new Sinistre
        {
            MontantDegats = 3000m,
            DateSinistre = DateTime.Now.AddMonths(-6)  // Récent: 6 mois
        };
        var sinistre2 = new Sinistre
        {
            MontantDegats = 2500m,
            DateSinistre = DateTime.Now.AddYears(-2).AddMonths(-6)  // Ancien: 2.5 ans
        };
        var sinistre3 = new Sinistre
        {
            MontantDegats = 4000m,
            DateSinistre = DateTime.Now.AddYears(-3)  // Ancien: 3 ans
        };

        _service.AjouterSinistre(conducteur, sinistre1);
        _service.AjouterSinistre(conducteur, sinistre2);
        _service.AjouterSinistre(conducteur, sinistre3);

        // Quand je supprime les sinistres de plus de 2 ans
        _service.SupprimerSinistresAnciens(conducteur, 2);

        // Alors le conducteur a maintenant 1 sinistre (le récent)
        Assert.Equal(1, _service.ObtenirNombreSinistres(conducteur));

        // Et le sinistre restant est celui de 6 mois
        var sinistresRestants = conducteur.Sinistres.ToList();
        Assert.Single(sinistresRestants);
        Assert.Contains(sinistre1, sinistresRestants);
        Assert.DoesNotContain(sinistre2, sinistresRestants);
        Assert.DoesNotContain(sinistre3, sinistresRestants);
    }
}
