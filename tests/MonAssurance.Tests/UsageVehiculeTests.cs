namespace MonAssurance.Tests;

using MonAssurance.Models;
using MonAssurance.Services;

public class UsageVehiculeTests
{
    [Fact]
    public void ReductionPourStationnementSecurise()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var contrat = new Contrat(500m)
        {
            TypeStationnement = TypeStationnement.GarageFermePrive
        };

        // Act
        var tarifAjuste = service.CalculerCoefficientGeographique(contrat);

        // Assert
        Assert.Equal(475m, tarifAjuste);
    }

    [Fact]
    public void PasDeReductionPourStationnementRue()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var contrat = new Contrat(500m)
        {
            TypeStationnement = TypeStationnement.Rue
        };

        // Act
        var tarifAjuste = service.CalculerCoefficientGeographique(contrat);

        // Assert
        Assert.Equal(500m, tarifAjuste);
    }

    [Fact]
    public void AvantagePetitRouleur()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var tarifBase = 600m;
        var kilometrage = 5000;

        // Act
        var tarifAjuste = service.AppliquerFacteurKilometrique(tarifBase, kilometrage);

        // Assert
        Assert.Equal(540m, tarifAjuste);
    }

    [Fact]
    public void PasDeRemisePourGrosRouleur()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var tarifBase = 600m;
        var kilometrage = 15000;

        // Act
        var tarifAjuste = service.AppliquerFacteurKilometrique(tarifBase, kilometrage);

        // Assert
        Assert.Equal(600m, tarifAjuste);
    }

    [Fact]
    public void RefusUsageCommercialVTC()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var typeVehicule = TypeVehicule.Voiture;
        var usage = UsageVehicule.TransportPersonnesTitreOnereux;

        // Act
        var resultat = service.ValiderUsage(typeVehicule, usage);

        // Assert
        Assert.False(resultat.EstValide);
        Assert.Equal("Usage professionnel non couvert par ce contrat", resultat.Motif);
    }

    [Fact]
    public void RefusUsageLivraison()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var typeVehicule = TypeVehicule.Voiture;
        var usage = UsageVehicule.Livraison;

        // Act
        var resultat = service.ValiderUsage(typeVehicule, usage);

        // Assert
        Assert.False(resultat.EstValide);
        Assert.Equal("Usage professionnel non couvert par ce contrat", resultat.Motif);
    }

    [Fact]
    public void AcceptationUsagePriveUniquement()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var typeVehicule = TypeVehicule.Voiture;
        var usage = UsageVehicule.UsagePriveUniquement;

        // Act
        var resultat = service.ValiderUsage(typeVehicule, usage);

        // Assert
        Assert.True(resultat.EstValide);
        Assert.Empty(resultat.Motif);
    }

    [Fact]
    public void AcceptationTrajetsDomicileTravail()
    {
        // Arrange
        var service = new UsageVehiculeService();
        var typeVehicule = TypeVehicule.Voiture;
        var usage = UsageVehicule.TrajetsDomicileTravail;

        // Act
        var resultat = service.ValiderUsage(typeVehicule, usage);

        // Assert
        Assert.True(resultat.EstValide);
        Assert.Empty(resultat.Motif);
    }
}
