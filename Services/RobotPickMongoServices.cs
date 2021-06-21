using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Recycle.Models;

namespace Recycle.Services
{
    public class RobotPickMongoServices
    {
        private readonly IMongoCollection<MongoPickDBmodel> collection1S;

        
        //BottleType == Btype
        public enum Btype
        {
            P = 0,
            COLOR = 1,
            SOY=2,
            OIL=3,
            TRAY=4,
            CH=5,
            OTHER=6,
        }
		
        //BottleKind == Bkind
        public enum Bkind
        {
            PET = 0,
            PP = 1,
            PS=2,
            PLA=3,
            PC=4,
            PVC=5,
        }
        
        public RobotPickMongoServices()
        {
            var client = new MongoClient("mongodb://localhost:27017");//settings.ConnectionString);
            var database = client.GetDatabase("mongoDBrobot1");//settings.DatabaseName);
            collection1S = database.GetCollection<MongoPickDBmodel>("robot1db");//settings.DeltaRobotCollectionName);
            Console.WriteLine("\n--------Hi---------: robotmongodbServices.cs : !!\n");//" 1:{0}, 2:{1}, 3:{2}", settings.DatabaseName, settings.ConnectionString,settings.DeltaRobotCollectionName);
        }
        
        //C-R-U-D DB
        //C:Create PET DB data
        public MongoPickDBmodel Create(MongoPickDBmodel mongodbmodel)
        {
            collection1S.InsertOne(mongodbmodel);
            return mongodbmodel;
        }
        
        //C:Create PET DB data II
        public MongoPickDBmodel Dumpdata(string btName, int conf,string btType=nameof(Bkind.PET))
        {
            MongoPickDBmodel mongodbmodel = new MongoPickDBmodel
            {
                BottleName = btName,
                Category = btType,
                Confidence = conf,
                Datetimetag = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Timestamp = DateTime.Now.ToLocalTime(),
            };
            collection1S.InsertOne(mongodbmodel);
            return mongodbmodel;
        }

        //R:Read
        public List<MongoPickDBmodel> Get()
        {
            return collection1S.Find(model1 => true).ToList(); 
        }
        //R:Read I
        public MongoPickDBmodel Get(long timetag)
        {
            Console.WriteLine("\n--------: RobotMongoDbServices.cs --GET--ID==[{0}] !\n ",timetag);
            var model1 = Builders<MongoPickDBmodel>.Filter.Eq("Datetimetag", timetag);

            if (model1 == null)
            {
                return null;
            }else
            {
                return collection1S.Find(x => x.Datetimetag > (timetag-1000) ).FirstOrDefault();            
                // var result = _collection1s.Find(model1).FirstOrDefault();
                // return _collection1s.Find(bmodel => bmodel.Datetimetag == timetag).FirstOrDefault();
                // return _collection1s.Find(model1 => model1.Id == id).FirstOrDefault();
            }
        }
        public long GetTime(long timetag)
        {
            Console.WriteLine("\n--------: RobotMongoDbServices.cs --GET--ID==[{0}] !\n ",timetag);
            var model1 = Builders<MongoPickDBmodel>.Filter.Eq("Datetimetag", timetag);

            if (model1 == null)
            {
                return 0;
            }else
            {
                var f1 = Builders<MongoPickDBmodel>.Filter.Lte(x=> x.Datetimetag,timetag);
                var f2 = Builders<MongoPickDBmodel>.Filter.Gt(x=> x.Datetimetag,timetag-60000);
                return collection1S.Find(f1 & f2 ).CountDocuments();
                // return collection1S.Find(f1).ToCursor().FirstOrDefault();
                // var result = _collection1s.Find(model1).FirstOrDefault();
                // return _collection1s.Find(bmodel => bmodel.Datetimetag == timetag).FirstOrDefault();
                // return _collection1s.Find(model1 => model1.Id == id).FirstOrDefault();
            }
        }
        
        public long GetDayTime(long timetag)
        {
            Console.WriteLine("\n--------: RobotMongoDbServices.cs --GET--ID==[{0}] !\n ",timetag);
            var model1 = Builders<MongoPickDBmodel>.Filter.Eq("Datetimetag", timetag);

            if (model1 == null)
            {
                return 0;
            }else
            {
                var f1 = Builders<MongoPickDBmodel>.Filter.Lte(x=> x.Datetimetag,timetag);
                var f2 = Builders<MongoPickDBmodel>.Filter.Gt(x=> x.Datetimetag,timetag-28800000);
                return collection1S.Find(f1 & f2 ).CountDocuments();
                // return collection1S.Find(f1).ToCursor().FirstOrDefault();
                // var result = _collection1s.Find(model1).FirstOrDefault();
                // return _collection1s.Find(bmodel => bmodel.Datetimetag == timetag).FirstOrDefault();
                // return _collection1s.Find(model1 => model1.Id == id).FirstOrDefault();
            }
        }
        
        public  decimal GetConfidence(long timetag)
        {
            Console.WriteLine("\n--------: RobotMongoDbServices.cs --GET--ID==[{0}] !\n ",timetag);
            var model1 = Builders<MongoPickDBmodel>.Filter.Eq("Datetimetag", timetag);

            if (model1 == null)
            {
                return 0;
            }else
            {
                var f1 = Builders<MongoPickDBmodel>.Filter.Lte(x=> x.Datetimetag,timetag);
                var f2 = Builders<MongoPickDBmodel>.Filter.Gt(x=> x.Datetimetag,timetag-60000);
                return collection1S.Find(f1 & f2 ).FirstOrDefault().Confidence;
                // return collection1S.Find(f1).ToCursor().FirstOrDefault();
                // var result = _collection1s.Find(model1).FirstOrDefault();
                // return _collection1s.Find(bmodel => bmodel.Datetimetag == timetag).FirstOrDefault();
                // return _collection1s.Find(model1 => model1.Id == id).FirstOrDefault();
            }
        }
        
        //U
        public void Update(string id, MongoPickDBmodel moedl1In) =>
            collection1S.ReplaceOne(model1 => model1.Id == id, moedl1In);
        //D
        public void Remove(MongoPickDBmodel moedl1In) =>
            collection1S.DeleteOne(model1 => model1.Id == moedl1In.Id); 
        //D
        public void Remove(string id) =>
            collection1S.DeleteOne(model1 => model1.Id == id); 
    }
}