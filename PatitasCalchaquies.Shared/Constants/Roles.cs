namespace PatitasCalchaquies.Shared.Constants;

/// <summary>
/// Role names as stored in the Roles table and emitted as role claims in the JWT.
/// </summary>
public static class Roles
{
    public const string Administrador = "Administrador";
    public const string Veterinario = "Veterinario";
    public const string Voluntario = "Voluntario";
}

/// <summary>
/// Authorization policies that mirror the use case diagram.
/// Administrador and Veterinario specialize Voluntario, so both satisfy the Voluntario policy.
/// </summary>
public static class Politicas
{
    /// <summary>Base actor: any user with one of the three system roles.</summary>
    public const string Voluntario = "Voluntario";

    /// <summary>Manage users and roles, catalogs, and sites and sectors.</summary>
    public const string Administrador = "Administrador";

    /// <summary>Register veterinary care, vaccinations, sterilizations and treatments.</summary>
    public const string Veterinario = "Veterinario";
}
