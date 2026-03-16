using Microsoft.AspNetCore.Mvc;
using MonAssurance.DTOs;
using MonAssurance.Models;
using MonAssurance.Services;
using Microsoft.AspNetCore.Http;

namespace MonAssurance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GestionRisqueController(GestionRisqueService service) : ControllerBase
{
    public record FranchiseResponseDto(decimal Montant, string Details);

    [HttpPost("franchise")]
    [ProducesResponseType(typeof(FranchiseResponseDto), StatusCodes.Status200OK)]
    public ActionResult<FranchiseResponseDto> CalculerFranchise([FromBody] FranchiseRequest request)
    {
        var conducteur = new Conducteur
        {
            DateNaissance = request.DateNaissance,
            AnneesPermis = request.AnneesPermis,
            CoefficientBonusMalus = request.CoefficientBonusMalus
        };

        var vehicule = new Vehicule(request.TypeVehicule, request.Puissance, request.Motorisation, request.ValeurVehicule);

        var resultat = service.CalculerFranchise(conducteur, vehicule);

        var response = new FranchiseResponseDto(resultat.Montant, resultat.Details);

        return Ok(response);
    }
}
