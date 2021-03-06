using System.Security.Cryptography.X509Certificates;

namespace x509_storage;

public static class X509Extensions
{
    /// <summary>
    /// Obtiene un certificado
    /// </summary>
    /// <param name="path">Ruta de acceso al archivo del certificado</param>
    /// <param name="psw">Contraseña del certificado (si aplica)</param>
    /// <returns>Certificado X509Certificate2</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    public static X509Certificate2 Get(string path, string? psw = null)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' no puede ser nulo ni estar vacío.", nameof(path));
        }
        if (!File.Exists(path))
        {
            throw new NullReferenceException($"'{nameof(path)}' no se encontró el archivo especificado.");
        }

        try
        {
            X509Certificate2? certificate = new(path, psw, X509KeyStorageFlags.Exportable);

            return certificate is null ? throw new NullReferenceException() : certificate;
        }
        catch (Exception ex)
        {
            throw new Exception($"No se encontró un certificado válido: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene un certificado
    /// </summary>
    /// <param name="noSerie">No de serie del certificado (instalado en el equipo)</param>
    /// <returns>Certificado X509Certificate2</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    public static X509Certificate2 Get(string noSerie)
    {
        if (string.IsNullOrEmpty(noSerie))
        {
            throw new ArgumentException($"'{nameof(noSerie)}' no puede ser nulo ni estar vacío.", nameof(noSerie));
        }

        try
        {
            using X509Store store = new(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2? certificate = store.Certificates.FirstOrDefault(certificate => certificate.Issuer.Equals(noSerie.ToUpper().Trim()));
            store.Close();

            return certificate is null ? throw new NullReferenceException() : certificate;
        }
        catch (Exception ex)
        {
            throw new Exception($"No se encontró un certificado válido: {ex.Message}");
        }
    }
}
