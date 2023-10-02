
namespace Sytycc_Service.Domain;
public class AppPassword
{
   public string Password { get; set; }
   public string PasswordHash { get; set; }
   public byte[] PasswordSalt { get; set; }
      
}
