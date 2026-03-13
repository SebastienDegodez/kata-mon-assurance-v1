using Microsoft.AspNetCore.Mvc;
using MonAssurance.DTOs;
using MonAssurance.Models;
using MonAssurance.Services;

namespace MonAssurance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsageVehiculeController(UsageVehiculeService service) : ControllerBase
{
    [HttpPost("coefficient")]
    public IActionResult CalculerCoefficient([FromBody] ContratRequest request)
    {
        var contrat = new Contrat(request.TarifBase)
        {
            TypeStationnement = request.TypeStationnement,
            KilometrageAnnuel = request.KilometrageAnnuel,
            Usage = request.Usage,
            TypeVehicule = request.TypeVehicule
        };

        var tarifAjuste = service.CalculerCoefficientGeographique(contrat);
        return Ok(new { TarifAjuste = tarifAjuste });
    }

    [HttpPost("kilometrage")]
    public IActionResult AppliquerFacteurKilometrique([FromBody] ContratRequest request)
    {
        var tarifAjuste = service.AppliquerFacteurKilometrique(request.TarifBase, request.KilometrageAnnuel);
        return Ok(new { TarifAjuste = tarifAjuste });
    }

    [HttpPost("valider-usage")]
    public IActionResult ValiderUsage([FromBody] UsageValidationRequest request)
    {
        var resultat = service.ValiderUsage(request.TypeVehicule, request.Usage);
        return Ok(new { resultat.EstValide, resultat.Motif });
    }
}
