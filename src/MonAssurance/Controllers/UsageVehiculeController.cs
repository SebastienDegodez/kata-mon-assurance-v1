using Microsoft.AspNetCore.Mvc;
using MonAssurance.DTOs;
using MonAssurance.Models;
using MonAssurance.Services;
using Microsoft.AspNetCore.Http;

namespace MonAssurance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsageVehiculeController(UsageVehiculeService service) : ControllerBase
{
    [HttpPost("coefficient")]
    [ProducesResponseType(typeof(CoefficientResponse), StatusCodes.Status200OK)]
    public ActionResult<CoefficientResponse> CalculerCoefficient([FromBody] ContratRequest request)
    {
        var contrat = new Contrat(request.TarifBase)
        {
            TypeStationnement = request.TypeStationnement,
            KilometrageAnnuel = request.KilometrageAnnuel,
            Usage = request.Usage,
            TypeVehicule = request.TypeVehicule
        };

        var tarifAjuste = service.CalculerCoefficientGeographique(contrat);
        var response = new CoefficientResponse(tarifAjuste);

        return Ok(response);
    }

    [HttpPost("kilometrage")]
    [ProducesResponseType(typeof(KilometrageResponse), StatusCodes.Status200OK)]
    public ActionResult<KilometrageResponse> AppliquerFacteurKilometrique([FromBody] ContratRequest request)
    {
        var tarifAjuste = service.AppliquerFacteurKilometrique(request.TarifBase, request.KilometrageAnnuel);
        var response = new KilometrageResponse(tarifAjuste);

        return Ok(response);
    }

    [HttpPost("valider-usage")]
    [ProducesResponseType(typeof(UsageValidationResponse), StatusCodes.Status200OK)]
    public ActionResult<UsageValidationResponse> ValiderUsage([FromBody] UsageValidationRequest request)
    {
        var resultat = service.ValiderUsage(request.TypeVehicule, request.Usage);
        var response = new UsageValidationResponse(resultat.EstValide, resultat.Motif);

        return Ok(response);
    }

    public sealed record CoefficientResponse(decimal TarifAjuste);

    public sealed record KilometrageResponse(decimal TarifAjuste);

    public sealed record UsageValidationResponse(bool EstValide, string? Motif);
}
