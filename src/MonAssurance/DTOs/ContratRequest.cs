using MonAssurance.Models;

namespace MonAssurance.DTOs;

public record ContratRequest(
    decimal TarifBase,
    TypeStationnement TypeStationnement,
    int KilometrageAnnuel,
    UsageVehicule Usage,
    TypeVehicule TypeVehicule
);
