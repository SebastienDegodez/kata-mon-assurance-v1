using MonAssurance.Models;

namespace MonAssurance.DTOs;

public record UsageValidationRequest(
    TypeVehicule TypeVehicule,
    UsageVehicule Usage
);
