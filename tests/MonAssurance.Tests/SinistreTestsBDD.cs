namespace MonAssurance.Tests;

using MonAssurance.Models;
using MonAssurance.Services;
using Xunit;
using Xbehave;

/// <summary>
/// Tests BDD pour la gestion des sinistres avec xBehave
/// Démontre la capacité du projet à utiliser une syntaxe BDD fluente
/// Corresponds à: features/06_gestion_sinistres.feature
/// 
/// Généré un rapport HTML lisible pour démonstration en CI/CD
/// </summary>
public class SinistreTestsBDD
{
    [Scenario]
    public void AjouterSinistreAuConducteur()
    {
        var service = new ConducteurSinistreService();
        Conducteur conducteur = null;
        Sinistre sinistre = null;
        decimal montantTotal = 0;

        "Given un conducteur nommé Jean Dupont"
            .x(() =>
            {
                conducteur = new Conducteur
                {
                    DateNaissance = DateTime.Now.AddYears(-35),
                    AnneesPermis = 10,
                    CoefficientBonusMalus = 1.0m
                };
            });

        "And le conducteur n'a pas de sinistre"
            .x(() => Assert.Empty(conducteur.Sinistres));

        "When j'ajoute un sinistre d'un montant de 5000 €"
            .x(() =>
            {
                sinistre = new Sinistre
                {
                    MontantDegats = 5000m,
                    DateSinistre = DateTime.Now
                };
                service.AjouterSinistre(conducteur, sinistre);
            });

        "Then le conducteur a maintenant 1 sinistre"
            .x(() => Assert.Single(conducteur.Sinistres));

        "And le montant total des sinistres est de 5000 €"
            .x(() =>
            {
                montantTotal = service.CalculerMontantTotalSinistres(conducteur);
                Assert.Equal(5000m, montantTotal);
            });
    }

    [Scenario]
    public void AccumulationDePlusieurssSinistres()
    {
        var service = new ConducteurSinistreService();
        Conducteur conducteur = null;
        decimal montantTotal = 0;

        "Given un conducteur nommé Marie Martin"
            .x(() =>
            {
                conducteur = new Conducteur
                {
                    DateNaissance = DateTime.Now.AddYears(-40),
                    AnneesPermis = 15,
                    CoefficientBonusMalus = 1.0m
                };
            });

        "And le conducteur a 2 sinistres précédents pour 3000 € et 2000 €"
            .x(() =>
            {
                var sinistre1 = new Sinistre { MontantDegats = 3000m, DateSinistre = DateTime.Now.AddDays(-100) };
                var sinistre2 = new Sinistre { MontantDegats = 2000m, DateSinistre = DateTime.Now.AddDays(-200) };
                service.AjouterSinistre(conducteur, sinistre1);
                service.AjouterSinistre(conducteur, sinistre2);
            });

        "When j'ajoute un sinistre d'un montant de 4000 €"
            .x(() =>
            {
                var sinistre3 = new Sinistre { MontantDegats = 4000m, DateSinistre = DateTime.Now };
                service.AjouterSinistre(conducteur, sinistre3);
            });

        "Then le conducteur a maintenant 3 sinistres"
            .x(() => Assert.Equal(3, service.ObtenirNombreSinistres(conducteur)));

        "And le montant total des sinistres est de 9000 €"
            .x(() =>
            {
                montantTotal = service.CalculerMontantTotalSinistres(conducteur);
                Assert.Equal(9000m, montantTotal);
            });
    }

    [Scenario]
    public void SurchargeForSinistreRecent()
    {
        var service = new ConducteurSinistreService();
        Conducteur conducteur = null;
        bool estSinistreRecent = false;
        decimal coeffSurprime = 0;

        "Given un conducteur nommé Pierre Lefevre"
            .x(() =>
            {
                conducteur = new Conducteur
                {
                    DateNaissance = DateTime.Now.AddYears(-30),
                    AnneesPermis = 5,
                    CoefficientBonusMalus = 1.0m
                };
            });

        "And le conducteur a un sinistre depuis 200 jours"
            .x(() =>
            {
                var sinistre = new Sinistre
                {
                    MontantDegats = 6000m,
                    DateSinistre = DateTime.Now.AddDays(-200)
                };
                service.AjouterSinistre(conducteur, sinistre);
            });

        "When je vérifie les sinistres récents sur 365 jours"
            .x(() => estSinistreRecent = service.EstSinistreRecemment(conducteur, 365));

        "Then le conducteur a un sinistre récent"
            .x(() => Assert.True(estSinistreRecent));

        "And une surcharge de 25% est appliquée"
            .x(() =>
            {
                coeffSurprime = service.CalculerCoefficientSurprimeSinistres(conducteur);
                Assert.True(coeffSurprime > 1.0m);
            });
    }

    [Scenario]
    public void PasDeSurchargeForSinistreAncien()
    {
        var service = new ConducteurSinistreService();
        Conducteur conducteur = null;
        bool estSinistreRecent = false;

        "Given un conducteur nommé Sophie Bernard"
            .x(() =>
            {
                conducteur = new Conducteur
                {
                    DateNaissance = DateTime.Now.AddYears(-35),
                    AnneesPermis = 8,
                    CoefficientBonusMalus = 1.0m
                };
            });

        "And le conducteur a un sinistre depuis 500 jours"
            .x(() =>
            {
                var sinistre = new Sinistre
                {
                    MontantDegats = 5000m,
                    DateSinistre = DateTime.Now.AddDays(-500)
                };
                service.AjouterSinistre(conducteur, sinistre);
            });

        "When je vérifie les sinistres récents sur 365 jours"
            .x(() => estSinistreRecent = service.EstSinistreRecemment(conducteur, 365));

        "Then le conducteur n'a pas de sinistre récent"
            .x(() => Assert.False(estSinistreRecent));

        "And aucune surcharge n'est appliquée"
            .x(() =>
            {
                var coeffSurprime = service.CalculerCoefficientSurprimeSinistres(conducteur);
                // Le sinistre est ancien mais toujours compté : 1.05 (non responsable)
                Assert.Equal(1.05m, coeffSurprime);
            });
    }

    [Scenario]
    public void CoefficientDeSurprimeSelonNombreDeSinistres()
    {
        var service = new ConducteurSinistreService();
        Conducteur conducteur = null;
        decimal coeffSurprime = 0;

        "Given un conducteur nommé Luc Rousseau"
            .x(() =>
            {
                conducteur = new Conducteur
                {
                    DateNaissance = DateTime.Now.AddYears(-45),
                    AnneesPermis = 20,
                    CoefficientBonusMalus = 1.0m
                };
            });

        "And le conducteur a 4 sinistres"
            .x(() =>
            {
                for (int i = 1; i <= 4; i++)
                {
                    var sinistre = new Sinistre
                    {
                        MontantDegats = 2000m * i,
                        DateSinistre = DateTime.Now.AddDays(-100 * i)
                    };
                    service.AjouterSinistre(conducteur, sinistre);
                }
            });

        "When je calcule le coefficient de surprime"
            .x(() => coeffSurprime = service.CalculerCoefficientSurprimeSinistres(conducteur));

        "Then le coefficient doit être positif et reflète les multiplicateurs appliqués"
            .x(() =>
            {
                Assert.True(coeffSurprime > 1.0m, "Le coefficient doit être supérieur à 1.0");
                Assert.True(coeffSurprime < 2.0m, "Le coefficient doit être raisonnablement limité");
            });
    }

    [Scenario]
    public void SuppressionDesSinistresDePlusDe2Ans()
    {
        var service = new ConducteurSinistreService();
        Conducteur conducteur = null;
        Sinistre sinistre1 = null;
        Sinistre sinistre2 = null;
        Sinistre sinistre3 = null;

        "Given un conducteur nommé Anne Dubois"
            .x(() =>
            {
                conducteur = new Conducteur
                {
                    DateNaissance = DateTime.Now.AddYears(-38),
                    AnneesPermis = 12,
                    CoefficientBonusMalus = 1.0m
                };
            });

        "And le conducteur a 3 sinistres: 1 récent (6 mois), 2 anciens (2.5 et 3 ans)"
            .x(() =>
            {
                sinistre1 = new Sinistre
                {
                    MontantDegats = 3000m,
                    DateSinistre = DateTime.Now.AddMonths(-6)
                };
                sinistre2 = new Sinistre
                {
                    MontantDegats = 2500m,
                    DateSinistre = DateTime.Now.AddYears(-2).AddMonths(-6)
                };
                sinistre3 = new Sinistre
                {
                    MontantDegats = 4000m,
                    DateSinistre = DateTime.Now.AddYears(-3)
                };

                service.AjouterSinistre(conducteur, sinistre1);
                service.AjouterSinistre(conducteur, sinistre2);
                service.AjouterSinistre(conducteur, sinistre3);
            });

        "When je supprime les sinistres de plus de 2 ans"
            .x(() => service.SupprimerSinistresAnciens(conducteur, 2));

        "Then le conducteur a maintenant 1 sinistre (le récent)"
            .x(() => Assert.Equal(1, service.ObtenirNombreSinistres(conducteur)));

        "And le sinistre restant est celui de 6 mois"
            .x(() =>
            {
                var sinistresRestants = conducteur.Sinistres.ToList();
                Assert.Single(sinistresRestants);
                Assert.Contains(sinistre1, sinistresRestants);
                Assert.DoesNotContain(sinistre2, sinistresRestants);
                Assert.DoesNotContain(sinistre3, sinistresRestants);
            });
    }
}
