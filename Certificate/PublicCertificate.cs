using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace x509_storage.Certificate;
/// <summary>
/// Expone la funcionalidad de un certificado público
/// </summary>
public class PublicCertificate : CertificateBase
{
    /// <summary>
    /// Crea un certificado público
    /// </summary>
    /// <param name="info">Información del certificadó que se cargó</param>
    /// <param name="certificate">Certificado adjunto</param>
    /// <exception cref="ArgumentNullException"></exception>
    public PublicCertificate(CertificateInfo info, X509Certificate2 certificate) : base(info, certificate, isPrivate: false)
    {
    }

    /// <summary>
    /// Verifica que una cadena sea compatible con una firma generada previamente
    /// </summary>
    /// <param name="data">Cadena de entrada</param>
    /// <param name="signature">Firma generada previamente</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    public bool VerifyData(string data, string signature)
    {
        try
        {
            RSA? publicKey = Certificate.GetRSAPublicKey();

            if (publicKey == null)
            {
                throw new NullReferenceException("No se encontró una llave pública válida");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = Convert.FromBase64String(signature);

            return publicKey.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }
}
