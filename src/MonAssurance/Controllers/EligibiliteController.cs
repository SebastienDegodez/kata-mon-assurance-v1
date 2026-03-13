using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonAssurance.Data;
using MonAssurance.DTOs;
using MonAssurance.Models;
using MonAssurance.Services;

namespace MonAssurance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibiliteController(EligibiliteService service, AssuranceDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> VerifierEligibilite([FromBody] EligibiliteRequest request)
    {
        var conducteur = new Conducteur
        {
            DateNaissance = request.DateNaissance,
            AnneesPermis = request.AnneesPermis,
            CoefficientBonusMalus = request.CoefficientBonusMalus
        };

        var vehicule = new Vehicule(request.TypeVehicule, request.Puissance, request.Motorisation, request.ValeurVehicule);

        var resultat = service.VerifierEligibilite(conducteur, vehicule);

        var demande = new DemandeDevis
        {
            DateNaissance = request.DateNaissance,
            AnneesPermis = request.AnneesPermis,
            CoefficientBonusMalus = request.CoefficientBonusMalus,
            TypeVehicule = request.TypeVehicule.ToString(),
            Puissance = request.Puissance,
            Motorisation = request.Motorisation.ToString(),
            ValeurVehicule = request.ValeurVehicule,
            EstAcceptee = resultat.EstAcceptee,
            MotifRefus = resultat.MotifRefus
        };

        db.DemandesDevis.Add(demande);
        await db.SaveChangesAsync();

        return Ok(new { demande.Id, resultat.EstAcceptee, resultat.MotifRefus });
    }

    [HttpGet]
    public async Task<IActionResult> ListerDemandesDevis()
    {
        var demandes = await db.DemandesDevis.ToListAsync();
        return Ok(demandes);
    }
}
