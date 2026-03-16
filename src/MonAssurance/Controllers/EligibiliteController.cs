using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonAssurance.Data;
using MonAssurance.DTOs;
using MonAssurance.Models;
using MonAssurance.Services;

namespace MonAssurance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibiliteController(EligibiliteService service, AssuranceDbContext db, ConducteurSinistreService sinistreService) : ControllerBase
{
    public sealed record VerifierEligibiliteResponse(Guid Id, bool EstAcceptee, string? MotifRefus);

    [HttpPost]
    [ProducesResponseType(typeof(VerifierEligibiliteResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<VerifierEligibiliteResponse>> VerifierEligibilite([FromBody] EligibiliteRequest request)
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

        var response = new VerifierEligibiliteResponse(demande.Id, resultat.EstAcceptee, resultat.MotifRefus);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> ListerDemandesDevis()
    {
        var demandes = await db.DemandesDevis.ToListAsync();
        return Ok(demandes);
    }

    // Endpoint pour montrer la logique métier mixée dans le Controller
    [HttpPost("conducteur/{id}/ajouter-sinistre")]
    public IActionResult AjouterSinistreConducteur(int id, [FromBody] Sinistre sinistre)
    {
        // Création d'un conducteur fictif pour la démo
        var conducteur = new Conducteur { AnneesPermis = 5, DateNaissance = DateTime.Now.AddYears(-30) };

        // Accès direct à la collection au lieu de passer par le service
        conducteur.Sinistres.Add(sinistre);

        // Calcul directement dans le Controller
        var totalSinistres = sinistreService.CalculerMontantTotalSinistres(conducteur);
        var nombreSinistres = sinistreService.ObtenirNombreSinistres(conducteur);
        
        // Vérification de logique métier ici aussi
        if (nombreSinistres > 3)
        {
            return BadRequest("Trop de sinistres : conducteur non asurable");
        }

        // Calcul de coefficient aussi dans le Controller
        var coeffSurprime = sinistreService.CalculerCoefficientSurprimeSinistres(conducteur);

        return Ok(new 
        { 
            message = "Sinistre ajouté",
            totalDegats = totalSinistres,
            nombreSinistres = nombreSinistres,
            coefficientSurprime = coeffSurprime
        });
    }

    // Une autre endpoint pour montrer plus de logique métier dans le Controller
    [HttpGet("conducteur/{id}/analyse-sinistres")]
    public IActionResult AnalyseSinistres(int id)
    {
        var conducteur = new Conducteur { AnneesPermis = 5, DateNaissance = DateTime.Now.AddYears(-30) };

        // Logique métier mélangée dans le Controller
        var estSinistreRecemment = sinistreService.EstSinistreRecemment(conducteur, 365);
        var montantTotal = sinistreService.CalculerMontantTotalSinistres(conducteur);
        
        // Calculs supplémentaires directement dans le Controller
        decimal surprime = 1.0m;
        if (estSinistreRecemment)
        {
            surprime = 1.25m;
        }

        // Appels en cascade à travers Service -> Controller
        var coeffStandard = sinistreService.CalculerCoefficientSurprimeSinistres(conducteur);
        
        // Logique complexe mélangée
        if (montantTotal > 10000)
        {
            surprime = surprime * 1.5m;
        }

        return Ok(new 
        { 
            estSinistreRecemment,
            montantTotal,
            surprimeCalculee = surprime,
            coefficientService = coeffStandard
        });
    }
}
