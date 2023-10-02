
using MongoDB.Driver;
namespace Sytycc_Service.Domain;
public interface IPasswordProvider
   {
      (string passwordHash, byte[] passwordSalt) CreatePasswordHash(string password);
      bool VerifyPasswordHash(string password, string storedHash, byte[] storedSalt);
   }
