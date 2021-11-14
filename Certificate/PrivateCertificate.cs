using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace x509_storage.Certificate;
/// <summary>
/// Expone la funcionalidad de un certificado privado
/// </summary>
public class PrivateCertificate : CertificateBase
{
    /// <summary>
    /// Crea un certificado privado
    /// </summary>
    /// <param name="info">Información del certificadó que se cargó</param>
    /// <param name="certificate">Certificado adjunto</param>
    /// <exception cref="ArgumentNullException"></exception>
    public PrivateCertificate(CertificateInfo info, X509Certificate2 certificate) : base(info, certificate, isPrivate: true)
    {
        Public = new PublicCertificate(info, certificate);
    }

    /// <summary>
    /// Certificado público adjunto
    /// </summary>
    public PublicCertificate Public { get; }

    /// <summary>
    /// Firma una cadena
    /// </summary>
    /// <param name="data">Cadena que se desea firmar</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    public string SignData(string data)
    {
        try
        {
            RSA? privateKey = Certificate.GetRSAPrivateKey();

            if (privateKey == null)
            {
                throw new NullReferenceException("No se encontró una llave privada válida");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = privateKey.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signatureBytes);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }
}
