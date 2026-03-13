namespace MonAssurance.Tests;

using MonAssurance.Models;
using MonAssurance.Services;
using Xunit;

/// <summary>
/// Tests de la fonctionnalité d'éligibilité à la souscription d'assurance
/// Correspond au fichier: features/01_eligibilite.feature
/// </summary>
public class EligibiliteTests
{
    private readonly EligibiliteService _service;

    public EligibiliteTests()
    {
        _service = new EligibiliteService();
    }

    /// <summary>
    /// Scénario: Refus de souscription voiture pour un mineur
    /// </summary>
    [Fact]
    public void RefusSouscriptionVoiturePourMineur()
    {
        // Etant donné un conducteur né le "10/10/2010"
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(2010, 10, 10),
            AnneesPermis = 0
        };

        // Et nous sommes le "01/01/2026"
        var dateReference = new DateTime(2026, 1, 1);

        // Et le véhicule est une "Voiture"
        var vehicule = new Vehicule(TypeVehicule.Voiture);

        // Quand je demande une éligibilité
        var resultat = _service.VerifierEligibilite(conducteur, vehicule, dateReference);

        // Alors la demande est refusée avec le motif "Conducteur trop jeune pour ce véhicule"
        Assert.False(resultat.EstAcceptee);
        Assert.Equal("Conducteur trop jeune pour ce véhicule", resultat.MotifRefus);
    }

    /// <summary>
    /// Scénario: Acceptation souscription trottinette pour un adolescent de 16 ans
    /// </summary>
    [Fact]
    public void AcceptationSouscriptionTrottinetteAdolescent16Ans()
    {
        // Etant donné un conducteur âgé de 16 ans
        var dateReference = new DateTime(2026, 1, 1);
        var conducteur = new Conducteur
        {
            DateNaissance = dateReference.AddYears(-16),
            AnneesPermis = 0
        };

        // Et le véhicule est une "Trottinette électrique"
        var vehicule = new Vehicule(TypeVehicule.TrottinetteElectrique);

        // Quand je demande une éligibilité
        var resultat = _service.VerifierEligibilite(conducteur, vehicule, dateReference);

        // Alors la demande est acceptée
        Assert.True(resultat.EstAcceptee);
        Assert.Null(resultat.MotifRefus);
    }

    /// <summary>
    /// Scénario: Refus moto puissante pour jeune permis
    /// </summary>
    [Fact]
    public void RefusMotoPuissantePourJeunePermis()
    {
        // Etant donné un conducteur avec 2 ans de permis
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1995, 1, 1),
            AnneesPermis = 2
        };

        // Et le véhicule est une "Moto" de 120 chevaux
        var vehicule = new Vehicule(TypeVehicule.Moto, puissance: 120);

        // Quand je demande une éligibilité
        var resultat = _service.VerifierEligibilite(conducteur, vehicule);

        // Alors la demande est refusée avec le motif "Expérience insuffisante pour la puissance"
        Assert.False(resultat.EstAcceptee);
        Assert.Equal("Expérience insuffisante pour la puissance", resultat.MotifRefus);
    }

    /// <summary>
    /// Test complémentaire: Acceptation voiture pour majeur
    /// </summary>
    [Fact]
    public void AcceptationSouscriptionVoiturePourMajeur()
    {
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(2000, 1, 1),
            AnneesPermis = 2
        };

        var vehicule = new Vehicule(TypeVehicule.Voiture);
        var dateReference = new DateTime(2026, 1, 1);

        var resultat = _service.VerifierEligibilite(conducteur, vehicule, dateReference);

        Assert.True(resultat.EstAcceptee);
        Assert.Null(resultat.MotifRefus);
    }

    /// <summary>
    /// Test complémentaire: Acceptation moto puissante avec expérience suffisante
    /// </summary>
    [Fact]
    public void AcceptationMotoPuissanteAvecExperienceSuffisante()
    {
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1990, 1, 1),
            AnneesPermis = 10
        };

        var vehicule = new Vehicule(TypeVehicule.Moto, puissance: 150);

        var resultat = _service.VerifierEligibilite(conducteur, vehicule);

        Assert.True(resultat.EstAcceptee);
        Assert.Null(resultat.MotifRefus);
    }
}
