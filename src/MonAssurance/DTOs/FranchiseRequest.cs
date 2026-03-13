using MonAssurance.Models;

namespace MonAssurance.DTOs;

public record FranchiseRequest(
    DateTime DateNaissance,
    int AnneesPermis,
    decimal CoefficientBonusMalus,
    TypeVehicule TypeVehicule,
    int Puissance,
    Motorisation Motorisation,
    decimal ValeurVehicule
);
