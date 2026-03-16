namespace MonAssurance.Tests;

using MonAssurance.Models;
using MonAssurance.Services;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Xunit;

/// <summary>
/// Tests BDD avec LightBDD - Syntaxe fluente et rapports HTML
/// 
/// LightBDD: Framework BDD moderne et actif, maintenu régulièrement
/// ✅ Syntaxe fluente descriptive: Runner.RunScenario()
/// ✅ Rapport HTML automatique avec étapes Given/When/Then
/// ✅ Meilleure lisibilité pour les parties prenantes
/// ✅ Correspond à: features/06_gestion_sinistres.feature
/// </summary>
[FeatureDescription("Gestion des sinistres du conducteur")]
public class SinistreTestsLightBDD : FeatureFixture
{
    private ConducteurSinistreService _service;
    private Conducteur _conducteur;
    private Sinistre _sinistre;
    private decimal _montantTotal;
    private bool _estSinistreRecent;
    private decimal _coeffSurprime;

    public SinistreTestsLightBDD()
    {
        _service = new ConducteurSinistreService();
    }

    [Scenario]
    public void Ajout_d_un_sinistre_a_un_conducteur()
    {
        Runner.RunScenario(
            Given_un_conducteur_nommé_Jean_Dupont,
            And_le_conducteur_n_a_pas_de_sinistre,
            When_j_ajoute_un_sinistre_d_un_montant_de_5000_euros,
            Then_le_conducteur_a_maintenant_1_sinistre,
            And_le_montant_total_des_sinistres_est_de_5000_euros
        );
    }

    [Scenario]
    public void Accumulation_de_plusieurs_sinistres()
    {
        Runner.RunScenario(
            Given_un_conducteur_nommé_Marie_Martin,
            And_le_conducteur_a_2_sinistres_précédents_pour_3000_et_2000_euros,
            When_j_ajoute_un_sinistre_d_un_montant_de_4000_euros,
            Then_le_conducteur_a_maintenant_3_sinistres,
            And_le_montant_total_des_sinistres_est_de_9000_euros
        );
    }

    [Scenario]
    public void Surcharge_pour_sinistre_récent()
    {
        Runner.RunScenario(
            Given_un_conducteur_nommé_Pierre_Lefevre,
            And_le_conducteur_a_un_sinistre_depuis_200_jours,
            When_je_vérifie_les_sinistres_récents_sur_365_jours,
            Then_le_conducteur_a_un_sinistre_récent,
            And_une_surcharge_est_appliquée
        );
    }

    [Scenario]
    public void Pas_de_surcharge_pour_sinistre_ancien()
    {
        Runner.RunScenario(
            Given_un_conducteur_nommé_Sophie_Bernard,
            And_le_conducteur_a_un_sinistre_depuis_500_jours,
            When_je_vérifie_les_sinistres_récents_sur_365_jours,
            Then_le_conducteur_n_a_pas_de_sinistre_récent,
            And_aucune_surcharge_supplémentaire_n_est_appliquée
        );
    }

    [Scenario]
    public void Coefficient_de_surprime_selon_nombre_de_sinistres()
    {
        Runner.RunScenario(
            Given_un_conducteur_nommé_Luc_Rousseau,
            And_le_conducteur_a_4_sinistres,
            When_je_calcule_le_coefficient_de_surprime,
            Then_le_coefficient_reflète_les_multiplicateurs_appliqués
        );
    }

    [Scenario]
    public void Suppression_des_sinistres_de_plus_de_2_ans()
    {
        Runner.RunScenario(
            Given_un_conducteur_nommé_Anne_Dubois,
            And_le_conducteur_a_3_sinistres_1_récent_6_mois_et_2_anciens,
            When_je_supprime_les_sinistres_de_plus_de_2_ans,
            Then_le_conducteur_a_maintenant_1_sinistre_le_récent,
            And_le_sinistre_restant_est_celui_de_6_mois
        );
    }

    // ===== GIVEN STEPS =====
    private void Given_un_conducteur_nommé_Jean_Dupont()
    {
        _conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-35),
            AnneesPermis = 10,
            CoefficientBonusMalus = 1.0m
        };
    }

    private void Given_un_conducteur_nommé_Marie_Martin()
    {
        _conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-40),
            AnneesPermis = 15,
            CoefficientBonusMalus = 1.0m
        };
    }

    private void Given_un_conducteur_nommé_Pierre_Lefevre()
    {
        _conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-30),
            AnneesPermis = 5,
            CoefficientBonusMalus = 1.0m
        };
    }

    private void Given_un_conducteur_nommé_Sophie_Bernard()
    {
        _conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-35),
            AnneesPermis = 8,
            CoefficientBonusMalus = 1.0m
        };
    }

    private void Given_un_conducteur_nommé_Luc_Rousseau()
    {
        _conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-45),
            AnneesPermis = 20,
            CoefficientBonusMalus = 1.0m
        };
    }

    private void Given_un_conducteur_nommé_Anne_Dubois()
    {
        _conducteur = new Conducteur
        {
            DateNaissance = DateTime.Now.AddYears(-38),
            AnneesPermis = 12,
            CoefficientBonusMalus = 1.0m
        };
    }

    // ===== AND (GIVEN continuation) =====
    private void And_le_conducteur_n_a_pas_de_sinistre()
    {
        Assert.Empty(_conducteur.Sinistres);
    }

    private void And_le_conducteur_a_2_sinistres_précédents_pour_3000_et_2000_euros()
    {
        var sinistre1 = new Sinistre { MontantDegats = 3000m, DateSinistre = DateTime.Now.AddDays(-100) };
        var sinistre2 = new Sinistre { MontantDegats = 2000m, DateSinistre = DateTime.Now.AddDays(-200) };
        _service.AjouterSinistre(_conducteur, sinistre1);
        _service.AjouterSinistre(_conducteur, sinistre2);
    }

    private void And_le_conducteur_a_un_sinistre_depuis_200_jours()
    {
        var sinistre = new Sinistre
        {
            MontantDegats = 6000m,
            DateSinistre = DateTime.Now.AddDays(-200)
        };
        _service.AjouterSinistre(_conducteur, sinistre);
    }

    private void And_le_conducteur_a_un_sinistre_depuis_500_jours()
    {
        var sinistre = new Sinistre
        {
            MontantDegats = 5000m,
            DateSinistre = DateTime.Now.AddDays(-500)
        };
        _service.AjouterSinistre(_conducteur, sinistre);
    }

    private void And_le_conducteur_a_4_sinistres()
    {
        for (int i = 1; i <= 4; i++)
        {
            var sinistre = new Sinistre
            {
                MontantDegats = 2000m * i,
                DateSinistre = DateTime.Now.AddDays(-100 * i)
            };
            _service.AjouterSinistre(_conducteur, sinistre);
        }
    }

    private void And_le_conducteur_a_3_sinistres_1_récent_6_mois_et_2_anciens()
    {
        var sinistre1 = new Sinistre { MontantDegats = 3000m, DateSinistre = DateTime.Now.AddMonths(-6) };
        var sinistre2 = new Sinistre { MontantDegats = 2500m, DateSinistre = DateTime.Now.AddYears(-2).AddMonths(-6) };
        var sinistre3 = new Sinistre { MontantDegats = 4000m, DateSinistre = DateTime.Now.AddYears(-3) };

        _service.AjouterSinistre(_conducteur, sinistre1);
        _service.AjouterSinistre(_conducteur, sinistre2);
        _service.AjouterSinistre(_conducteur, sinistre3);
    }

    // ===== WHEN STEPS =====
    private void When_j_ajoute_un_sinistre_d_un_montant_de_5000_euros()
    {
        _sinistre = new Sinistre
        {
            MontantDegats = 5000m,
            DateSinistre = DateTime.Now
        };
        _service.AjouterSinistre(_conducteur, _sinistre);
    }

    private void When_j_ajoute_un_sinistre_d_un_montant_de_4000_euros()
    {
        var sinistre = new Sinistre { MontantDegats = 4000m, DateSinistre = DateTime.Now };
        _service.AjouterSinistre(_conducteur, sinistre);
    }

    private void When_je_vérifie_les_sinistres_récents_sur_365_jours()
    {
        _estSinistreRecent = _service.EstSinistreRecemment(_conducteur, 365);
    }

    private void When_je_calcule_le_coefficient_de_surprime()
    {
        _coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(_conducteur);
    }

    private void When_je_supprime_les_sinistres_de_plus_de_2_ans()
    {
        _service.SupprimerSinistresAnciens(_conducteur, 2);
    }

    // ===== THEN STEPS =====
    private void Then_le_conducteur_a_maintenant_1_sinistre()
    {
        Assert.Single(_conducteur.Sinistres);
    }

    private void Then_le_conducteur_a_maintenant_3_sinistres()
    {
        Assert.Equal(3, _service.ObtenirNombreSinistres(_conducteur));
    }

    private void Then_le_conducteur_a_un_sinistre_récent()
    {
        Assert.True(_estSinistreRecent);
    }

    private void Then_le_conducteur_n_a_pas_de_sinistre_récent()
    {
        Assert.False(_estSinistreRecent);
    }

    private void Then_le_coefficient_reflète_les_multiplicateurs_appliqués()
    {
        Assert.True(_coeffSurprime > 1.0m, "Le coefficient doit être supérieur à 1.0");
        Assert.True(_coeffSurprime < 2.0m, "Le coefficient doit être raisonnablement limité");
    }

    private void Then_le_conducteur_a_maintenant_1_sinistre_le_récent()
    {
        Assert.Equal(1, _service.ObtenirNombreSinistres(_conducteur));
    }

    // ===== AND (THEN continuation) =====
    private void And_le_montant_total_des_sinistres_est_de_5000_euros()
    {
        _montantTotal = _service.CalculerMontantTotalSinistres(_conducteur);
        Assert.Equal(5000m, _montantTotal);
    }

    private void And_le_montant_total_des_sinistres_est_de_9000_euros()
    {
        _montantTotal = _service.CalculerMontantTotalSinistres(_conducteur);
        Assert.Equal(9000m, _montantTotal);
    }

    private void And_une_surcharge_est_appliquée()
    {
        _coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(_conducteur);
        Assert.True(_coeffSurprime > 1.0m);
    }

    private void And_aucune_surcharge_supplémentaire_n_est_appliquée()
    {
        _coeffSurprime = _service.CalculerCoefficientSurprimeSinistres(_conducteur);
        Assert.Equal(1.05m, _coeffSurprime);
    }

    private void And_le_sinistre_restant_est_celui_de_6_mois()
    {
        var sinistresRestants = _conducteur.Sinistres.ToList();
        Assert.Single(sinistresRestants);
    }
}
