using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MonAssurance.Models;

namespace MonAssurance.Tests.Integration;

public class GestionRisqueControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostFranchise_BonConducteur_Retourne500Euros()
    {
        var request = new
        {
            DateNaissance = DateTime.Today.AddYears(-30),
            AnneesPermis = 10,
            CoefficientBonusMalus = 0.8m,
            TypeVehicule = TypeVehicule.Voiture,
            Puissance = 90,
            Motorisation = Motorisation.Essence,
            ValeurVehicule = 15000m
        };

        var response = await _client.PostAsJsonAsync("/api/gestionrisque/franchise", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<FranchiseResponse>();
        Assert.NotNull(body);
        Assert.Equal(500m, body.Montant);
    }

    [Fact]
    public async Task PostFranchise_ConducteurMalus_Retourne10PourcentValeur()
    {
        var request = new
        {
            DateNaissance = DateTime.Today.AddYears(-30),
            AnneesPermis = 5,
            CoefficientBonusMalus = 1.25m,
            TypeVehicule = TypeVehicule.Voiture,
            Puissance = 90,
            Motorisation = Motorisation.Essence,
            ValeurVehicule = 20000m
        };

        var response = await _client.PostAsJsonAsync("/api/gestionrisque/franchise", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<FranchiseResponse>();
        Assert.NotNull(body);
        Assert.Equal(2000m, body.Montant);
    }

    [Fact]
    public async Task PostFranchise_VeloElectrique_Retourne50Euros()
    {
        var request = new
        {
            DateNaissance = DateTime.Today.AddYears(-25),
            AnneesPermis = 3,
            CoefficientBonusMalus = 1.0m,
            TypeVehicule = TypeVehicule.VeloElectrique,
            Puissance = 0,
            Motorisation = Motorisation.Electrique,
            ValeurVehicule = 2000m
        };

        var response = await _client.PostAsJsonAsync("/api/gestionrisque/franchise", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<FranchiseResponse>();
        Assert.NotNull(body);
        Assert.Equal(50m, body.Montant);
    }

    private record FranchiseResponse(decimal Montant, string Details);
}
