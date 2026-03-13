using Microsoft.AspNetCore.Mvc;
using MonAssurance.DTOs;
using MonAssurance.Models;
using MonAssurance.Services;

namespace MonAssurance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GestionRisqueController(GestionRisqueService service) : ControllerBase
{
    [HttpPost("franchise")]
    public IActionResult CalculerFranchise([FromBody] FranchiseRequest request)
    {
        var conducteur = new Conducteur
        {
            DateNaissance = request.DateNaissance,
            AnneesPermis = request.AnneesPermis,
            CoefficientBonusMalus = request.CoefficientBonusMalus
        };

        var vehicule = new Vehicule(request.TypeVehicule, request.Puissance, request.Motorisation, request.ValeurVehicule);

        var resultat = service.CalculerFranchise(conducteur, vehicule);

        return Ok(new { resultat.Montant, resultat.Details });
    }
}
