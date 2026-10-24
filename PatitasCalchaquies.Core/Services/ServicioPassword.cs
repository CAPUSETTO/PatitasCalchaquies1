namespace PatitasCalchaquies.Core.Services;

public static class ServicioPassword
{
    public static string HashPassword(string password_)
    {
        return BCrypt.Net.BCrypt.HashPassword(password_);
    }

    public static bool VerificarPassword(string password, string? hash_)
    {
        if (string.IsNullOrEmpty(hash_)) return false;
        return BCrypt.Net.BCrypt.Verify(password, hash_);
    }
}