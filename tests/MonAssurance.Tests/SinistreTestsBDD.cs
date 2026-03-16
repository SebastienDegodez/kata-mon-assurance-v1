namespace MonAssurance.Tests;

using MonAssurance.Models;
using MonAssurance.Services;
using Xunit;

/// <summary>
/// Tests BDD - Approche minimaliste et pérenne
/// 
/// ✅ Zéro dépendance excessive (juste xUnit)
/// ✅ Syntaxe claire avec commentaires Given/When/Then
/// ✅ Attributs [Trait] pour tracer chaque test vers son scénario BDD
/// ✅ Pas à la merci d'un framework qui devient obsolète
/// ✅ Lisible et maintenable à long terme
/// 
/// Correspond à: features/06_gestion_sinistres.feature
/// </summary>
public class SinistreTestsBDD
{
    private readonly ConducteurSinistreService _service;

    public SinistreTestsBDD()
    {
        _service = new ConducteurSinistreService();
    }

    // ===== Scénario 1: Ajout d'un sinistre =====
    [Fact]
    [Trait("Feature", "06_gestion_sinistres")]
    [Trait("Scenario", "Ajout d'un sinistre à un conducteur")]
    public void Scenario_Ajout_sinistre_a_conducteur()
    {
        // Given: un conducteur nommé Jean Dupont
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-35),
            AnneesPermis = 10,
            CoefficientBonusMalus = 1.0m
        };

        // And: le conducteur n'a pas de sinistre
        Assert.Empty(conducteur.Sinistres);

        // When: j'ajoute un sinistre d'un montant de 5000€
        var sinistre = new Sinistre
        {
            MontantDegats = 5000m,
            DateSinistre = DateTime.Now
        };
        _service.AjouterSinistre(conducteur, sinistre);

        // Then: le conducteur a maintenant 1 sinistre
        Assert.Single(conducteur.Sinistres);

        // And: le montant total des sinistres est de 5000€
        var montantTotal = _service.CalculerMontantTotalSinistres(conducteur);
        Assert.Equal(5000m, montantTotal);
    }

    // ===== Scénario 2: Accumulation de plusieurs sinistres =====
    [Fact]
    [Trait("Feature", "06_gestion_sinistres")]
    [Trait("Scenario", "Accumulation de plusieurs sinistres")]
    public void Scenario_Accumulation_plusieurs_sinistres()
    {
        // Given: un conducteur nommé Marie Martin
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-40),
            AnneesPermis = 15,
            CoefficientBonusMalus = 1.0m
        };

        // And: le conducteur a 2 sinistres précédents pour 3000€ et 2000€
        var sinistre1 = new Sinistre { MontantDegats = 3000m, DateSinistre = DateTime.Now.AddDays(-100) };
        var sinistre2 = new Sinistre { MontantDegats = 2000m, DateSinistre = DateTime.Now.AddDays(-200) };
        _service.AjouterSinistre(conducteur, sinistre1);
        _service.AjouterSinistre(conducteur, sinistre2);

        // When: j'ajoute un sinistre d'un montant de 4000€
        var sinistre3 = new Sinistre { MontantDegats = 4000m, DateSinistre = DateTime.Now };
        _service.AjouterSinistre(conducteur, sinistre3);

        // Then: le conducteur a maintenant 3 sinistres
        Assert.Equal(3, _service.ObtenirNombreSinistres(conducteur));

        // And: le montant total des sinistres est de 9000€
        var montantTotal = _service.CalculerMontantTotalSinistres(conducteur);
        Assert.Equal(9000m, montantTotal);
    }

    // ===== Scénario 3: Surcharge pour sinistre récent =====
    [Fact]
    [Trait("Feature", "06_gestion_sinistres")]
    [Trait("Scenario", "Surcharge pour sinistre récent")]
    public void Scenario_Surcharge_sinistre_recent()
    {
        // Given: un conducteur nommé Pierre Lefevre
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-30),
            AnneesPermis = 5,
            CoefficientBonusMalus = 1.0m
        };

        // And: le conducteur a un sinistre depuis 200 jours
        var sinistre = new Sinistre
        {
            MontantDegats = 6000m,
            DateSinistre = DateTime.Now.AddDays(-200)
        };
        _service.AjouterSinistre(conducteur, sinistre);

        // When: je vérifie les sinistres récents sur 365 jours
        var estSinistreRecent = _service.EstSinistreRecemment(conducteur, 365);

        // Then: le conducteur a un sinistre récent
        Assert.True(estSinistreRecent);

        // And: une surcharge est appliquée
        var coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(conducteur);
        Assert.True(coeffSurprime > 1.0m);
    }

    // ===== Scénario 4: Pas de surcharge pour sinistre ancien =====
    [Fact]
    [Trait("Feature", "06_gestion_sinistres")]
    [Trait("Scenario", "Pas de surcharge pour sinistre ancien")]
    public void Scenario_Pas_surcharge_sinistre_ancien()
    {
        // Given: un conducteur nommé Sophie Bernard
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-35),
            AnneesPermis = 8,
            CoefficientBonusMalus = 1.0m
        };

        // And: le conducteur a un sinistre depuis 500 jours
        var sinistre = new Sinistre
        {
            MontantDegats = 5000m,
            DateSinistre = DateTime.Now.AddDays(-500)
        };
        _service.AjouterSinistre(conducteur, sinistre);

        // When: je vérifie les sinistres récents sur 365 jours
        var estSinistreRecent = _service.EstSinistreRecemment(conducteur, 365);

        // Then: le conducteur n'a pas de sinistre récent
        Assert.False(estSinistreRecent);

        // And: la surcharge reflète juste le sinistre (pas récent donc pas supplémentaire)
        var coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(conducteur);
        Assert.Equal(1.05m, coeffSurprime);
    }

    // ===== Scénario 5: Coefficient de surprime selon nombre de sinistres =====
    [Fact]
    [Trait("Feature", "06_gestion_sinistres")]
    [Trait("Scenario", "Coefficient de surprime selon le nombre de sinistres")]
    public void Scenario_Coefficient_surprime_nombre_sinistres()
    {
        // Given: un conducteur nommé Luc Rousseau
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-45),
            AnneesPermis = 20,
            CoefficientBonusMalus = 1.0m
        };

        // And: le conducteur a 4 sinistres
        for (int i = 1; i <= 4; i++)
        {
            var sinistre = new Sinistre
            {
                MontantDegats = 2000m * i,
                DateSinistre = DateTime.Now.AddDays(-100 * i)
            };
            _service.AjouterSinistre(conducteur, sinistre);
        }

        // When: je calcule le coefficient de surprime
        var coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(conducteur);

        // Then: le coefficient doit être positif et reflète les multiplicateurs appliqués
        // 4 sinistres: 2000, 4000, 6000, 8000
        // - 2000: 1.05 (non responsable)
        // - 4000: 1.05 (non responsable)
        // - 6000: 1.05 * 1.10 (montant > 5000)
        // - 8000: 1.05 * 1.10 (montant > 5000)
        // Total: 1.05 * 1.05 * 1.155 * 1.155 ≈ 1.47
        Assert.True(coeffSurprime > 1.0m, "Le coefficient doit être supérieur à 1.0");
        Assert.True(coeffSurprime < 2.0m, "Le coefficient doit être raisonnablement limité");
    }

    // ===== Scénario 6: Suppression des sinistres de plus de 2 ans =====
    [Fact]
    [Trait("Feature", "06_gestion_sinistres")]
    [Trait("Scenario", "Suppression des sinistres de plus de 2 ans")]
    public void Scenario_Suppression_sinistres_plus_2_ans()
    {
        // Given: un conducteur nommé Anne Dubois
        var conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-38),
            AnneesPermis = 12,
            CoefficientBonusMalus = 1.0m
        };

        // And: le conducteur a 3 sinistres: 1 récent (6 mois), 2 anciens (2.5 et 3 ans)
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

        // When: je supprime les sinistres de plus de 2 ans
        _service.SupprimerSinistresAnciens(conducteur, 2);

        // Then: le conducteur a maintenant 1 sinistre (le récent)
        Assert.Equal(1, _service.ObtenirNombreSinistres(conducteur));

        // And: le sinistre restant est celui de 6 mois
        var sinistresRestants = conducteur.Sinistres.ToList();
        Assert.Single(sinistresRestants);
        Assert.Contains(sinistre1, sinistresRestants);
        Assert.DoesNotContain(sinistre2, sinistresRestants);
        Assert.DoesNotContain(sinistre3, sinistresRestants);
    }
}


