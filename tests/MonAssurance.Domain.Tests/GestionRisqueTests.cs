namespace MonAssurance.Domain.Tests;

using MonAssurance.Domain.Models;
using MonAssurance.Domain.Services;
using Xunit;

public class GestionRisqueTests
{
    private readonly GestionRisqueService _service;

    public GestionRisqueTests()
    {
        _service = new GestionRisqueService();
    }

    [Fact]
    public void FranchiseStandard_BonConducteur_Retourne500Euros()
    {
        // Arrange - Conducteur avec un bonus de 0.50 (50% de bonus)
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1980, 1, 1),
            AnneesPermis = 10,
            CoefficientBonusMalus = 0.50m
        };

        // Voiture d'une valeur de 20000 €
        var vehicule = new Vehicule(TypeVehicule.Voiture, puissance: 100, valeur: 20000m);

        // Act - Calcul de la franchise accident
        var resultat = _service.CalculerFranchise(conducteur, vehicule);

        // Assert - La franchise est de 500 € (fixe)
        Assert.Equal(500m, resultat.Montant);
        Assert.Contains("standard", resultat.Details.ToLower());
    }

    [Fact]
    public void FranchisePunitive_ConducteurMalusse_Retourne10PourcentValeurVehicule()
    {
        // Arrange - Conducteur avec un malus de 1.25 (25% de malus)
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1990, 1, 1),
            AnneesPermis = 2,
            CoefficientBonusMalus = 1.25m
        };

        // Voiture d'une valeur de 20000 €
        var vehicule = new Vehicule(TypeVehicule.Voiture, puissance: 120, valeur: 20000m);

        // Act - Calcul de la franchise accident
        var resultat = _service.CalculerFranchise(conducteur, vehicule);

        // Assert - La franchise est calculée à 10% de la valeur du véhicule
        // Le montant de la franchise est de 2000 €
        Assert.Equal(2000m, resultat.Montant);
        Assert.Contains("punitive", resultat.Details.ToLower());
        Assert.Contains("malus", resultat.Details.ToLower());
    }

    [Fact]
    public void FranchiseReduite_VeloElectrique_PlafonneeA50Euros()
    {
        // Arrange - Véhicule de type "Vélo électrique"
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1985, 1, 1),
            AnneesPermis = 5,
            CoefficientBonusMalus = 1.00m
        };

        // Valeur d'achat de 2000 €
        var vehicule = new Vehicule(TypeVehicule.VeloElectrique, valeur: 2000m);

        // Act - Calcul de la franchise accident
        var resultat = _service.CalculerFranchise(conducteur, vehicule);

        // Assert - La franchise est plafonnée à 50 €
        Assert.Equal(50m, resultat.Montant);
        Assert.Contains("mobilité douce", resultat.Details.ToLower());
    }

    [Fact]
    public void FranchiseReduite_TrottinetteElectrique_PlafonneeA50Euros()
    {
        // Arrange
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1995, 1, 1),
            AnneesPermis = 3,
            CoefficientBonusMalus = 0.80m
        };

        var vehicule = new Vehicule(TypeVehicule.TrottinetteElectrique, valeur: 800m);

        // Act
        var resultat = _service.CalculerFranchise(conducteur, vehicule);

        // Assert
        Assert.Equal(50m, resultat.Montant);
        Assert.Contains("mobilité douce", resultat.Details.ToLower());
    }

    [Fact]
    public void FranchiseStandard_ConducteurNeutre_Retourne500Euros()
    {
        // Arrange - Conducteur neutre (coefficient = 1.00)
        var conducteur = new Conducteur
        {
            DateNaissance = new DateTime(1975, 1, 1),
            AnneesPermis = 15,
            CoefficientBonusMalus = 1.00m
        };

        var vehicule = new Vehicule(TypeVehicule.Voiture, puissance: 90, valeur: 15000m);

        // Act
        var resultat = _service.CalculerFranchise(conducteur, vehicule);

        // Assert
        Assert.Equal(500m, resultat.Montant);
    }
}
