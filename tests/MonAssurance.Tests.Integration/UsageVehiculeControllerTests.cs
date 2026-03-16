using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MonAssurance.Models;

namespace MonAssurance.Tests.Integration;

public class UsageVehiculeControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostValiderUsage_UsagePrivePourVoiture_EstValide()
    {
        var request = new { TypeVehicule = TypeVehicule.Voiture, Usage = UsageVehicule.UsagePriveUniquement };

        var response = await _client.PostAsJsonAsync("/api/usagevehicule/valider-usage", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UsageResponse>();
        Assert.NotNull(body);
        Assert.True(body.EstValide);
    }

    [Fact]
    public async Task PostValiderUsage_Livraison_EstInvalide()
    {
        var request = new { TypeVehicule = TypeVehicule.Voiture, Usage = UsageVehicule.Livraison };

        var response = await _client.PostAsJsonAsync("/api/usagevehicule/valider-usage", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<UsageResponse>();
        Assert.NotNull(body);
        Assert.False(body.EstValide);
    }

    [Fact]
    public async Task PostCoefficient_GaragePrive_AppliqueReduction()
    {
        var request = new
        {
            TarifBase = 1000m,
            TypeStationnement = TypeStationnement.GarageFermePrive,
            KilometrageAnnuel = 10000,
            Usage = UsageVehicule.UsagePriveUniquement,
            TypeVehicule = TypeVehicule.Voiture
        };

        var response = await _client.PostAsJsonAsync("/api/usagevehicule/coefficient", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<TarifResponse>();
        Assert.NotNull(body);
        Assert.Equal(950m, body.TarifAjuste);
    }

    [Fact]
    public async Task PostKilometrage_PetitRouleur_AppliqueRemise()
    {
        var request = new
        {
            TarifBase = 1000m,
            TypeStationnement = TypeStationnement.Rue,
            KilometrageAnnuel = 5000,
            Usage = UsageVehicule.UsagePriveUniquement,
            TypeVehicule = TypeVehicule.Voiture
        };

        var response = await _client.PostAsJsonAsync("/api/usagevehicule/kilometrage", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<TarifResponse>();
        Assert.NotNull(body);
        Assert.Equal(900m, body.TarifAjuste);
    }

    private record UsageResponse(bool EstValide, string Motif);
    private record TarifResponse(decimal TarifAjuste);
}
