using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MonAssurance.Models;

namespace MonAssurance.Tests.Integration;

public class EligibiliteControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostEligibilite_MajeurVoiture_RetourneAccepte()
    {
        var request = new
        {
            DateNaissance = DateTime.Today.AddYears(-25),
            AnneesPermis = 5,
            CoefficientBonusMalus = 1.0m,
            TypeVehicule = TypeVehicule.Voiture,
            Puissance = 90,
            Motorisation = Motorisation.Essence,
            ValeurVehicule = 15000m
        };

        var response = await _client.PostAsJsonAsync("/api/eligibilite", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<EligibiliteResponse>();
        Assert.NotNull(body);
        Assert.True(body.EstAcceptee);
        Assert.Null(body.MotifRefus);
    }

    [Fact]
    public async Task PostEligibilite_MineurVoiture_RetourneRefus()
    {
        var request = new
        {
            DateNaissance = DateTime.Today.AddYears(-16),
            AnneesPermis = 0,
            CoefficientBonusMalus = 1.0m,
            TypeVehicule = TypeVehicule.Voiture,
            Puissance = 90,
            Motorisation = Motorisation.Essence,
            ValeurVehicule = 10000m
        };

        var response = await _client.PostAsJsonAsync("/api/eligibilite", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<EligibiliteResponse>();
        Assert.NotNull(body);
        Assert.False(body.EstAcceptee);
        Assert.NotNull(body.MotifRefus);
    }

    [Fact]
    public async Task GetEligibilite_RetourneListe()
    {
        var response = await _client.GetAsync("/api/eligibilite");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private record EligibiliteResponse(Guid Id, bool EstAcceptee, string? MotifRefus);
}
