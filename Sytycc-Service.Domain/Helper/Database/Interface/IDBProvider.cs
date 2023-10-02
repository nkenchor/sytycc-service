
using MongoDB.Driver;
namespace Sytycc_Service.Domain;
public interface IDBProvider
   {
      public IMongoDatabase Connect();
      public int GetPageLimit();
   }