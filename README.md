# x509-storage
## Libreria de administración de certificados x509
> Agregar el espacio de nombres
```
using x509_storage;
using x509_storage.Certificate;
```
## Especificación de información de los certificados
*Los certificados se pueden instanciar usando dos vías, la primera es a través de la boveda de almacenamiento del equipo, y la segunda, es a través del archivo **.cert** o **.pfx** del certificado.*
### Si su caso es aquel donde el certificado está instalado en el equipo, entonces requiere el número de serie del certificado en cuestion.
> Nota: La herramienta determinará en tiempo de ejecución la naturaleza del certificado (público o privado)
```
CertificateInfo certInstalled = new()
{
    Name = "localCert",
    IsInstalled = true,
    CS = "15c2ad31f56b5c8f4a3e0bfecf3730c8"
};
```
### Si su caso es aquel donde el certificado es conocido a través de un archivo **.cert** o **.pfx**, entonces requiere la ruta de acceso del archivo, y de manera opcional, la constraseña de este (si es un certificado privado)
> Nota: Si especifica una constraseña para acceder a un certificado público, esta no será tomada en cuenta.
```
CertificateInfo certPublicFile = new()
{
    Name = "localCertPublic",
    IsInstalled = false,
    CF = "C:/certificates/certificate.cert"
};

CertificateInfo certPrivateFile = new()
{
    Name = "localCertPrivate",
    IsInstalled = false,
    CF = "C:/certificates/certificate.pfx",
    CP = "Contraseña"
};
```
## Instanciamiento y almacenamiento de nuevos certificados en la aplicación
1. Llamar al singletón de la clase `X509Storage`
```
X509Storage x509Storage = X509Storage.Storage;
```
2. Cargar la información de los certificados antes especificados
```
x509Storage.Load(certInstalled, certPublicFile, certPrivateFile);
```
## Usar los certificados caargados en la aplicación
> Ubicar el certificado por su nombre unico
```
CertificateBase localCertificate = x509Storage.Certificates["localCert"];
```
> Si el certificado tiene una llave privada, quiere decir que el certificado es privado, para lo cual deberá castearlo de tal modo, en caso contrario, será publico.

> Nota: Puede saltarse esta validación y hacer directamente el casteo del certificado si está seguro de la naturaleza de este.
```
if (localCertificate.Certificate.HasPrivateKey)
{
    var privateCert = localCertificate as PrivateCertificate;
}
else
{
    var publicCert = localCertificate as PublicCertificate;
}
```
### Certificado Público (`PublicCertificate`)
> Con un certificado publico, puede verificar la validez de una cadena firmada. Utilizando una cadena `A`, el algoritmo firmará dicha cadena, y comparará el resultado con la firma original proporcionada.
```
var publicCertificateFile = (PublicCertificate)x509Storage.Certificates["localCertPublic"];
bool isValidSignature = publicCertificateFile.VerifyData("CADENA DE DÓNDE SE GENERARÁ UNA FIRMA", "CADENA DE FIRMA ORIGINAL");
```
### Certificado Privado (`PrivateCertificate`)
> Con un certificado privado, puede firmar una cadena la cual puede utilizar para distintos procesos de seguridad.

> Nota: Un certificado privado, tiene un componente `PublicCertificate` con el cual puede ejecutar funcionalidad de un certificado público.
```
var privateCertificateFile = (PrivateCertificate)x509Storage.Certificates["localCertPrivate"];
string signature = privateCertificateFile.SignData("CADENA DE DÓNDE SE GENERARÁ UNA FIRMA");
isValidSignature = privateCertificateFile.Public.VerifyData("CADENA DE DÓNDE SE GENERARÁ UNA FIRMA", "CADENA DE FIRMA ORIGINAL");
```
