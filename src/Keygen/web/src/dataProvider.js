var MongoClient = require("mongodb").MongoClient;
var Q = require('q');

exports.log = function(data){
    var deferred = Q.defer();
    MongoClient.connect("mongodb://localhost:27017/xm6k", function(err,db){
        if(err) {
            deferred.reject(err);
            return;
        }
        db.collection("logs").insert(data,{w:1},function(err,result){
            if(err) { deferred.reject(err); db.close();return;}
            deferred.resolve(result);

            if(data.info && data.info.CPU && data.info.CPU.length > 0) {
                db.collection("blacklist").update({"processorID": data.info.CPU[0].ProcessorID},
                    {$inc: {count: 1}},
                    {upsert: true},function(err,result){
                        if(err) {deferred.reject(err);}
                        db.close();
                    });
            }
        });
    });
    return deferred.promise;
};

exports.checkBlacklist = function(postData){
    var processorID = "";
    var deferred = Q.defer();
    if(postData && postData.info && postData.info.CPU) {
        if(postData.info.CPU.length > 0) {
            processorID = postData.info.CPU[0].ProcessorID;
        }
    }

    MongoClient.connect("mongodb://localhost:27017/xm6k",function(err,db){
        if(err) {
            console.error(err);
            deferred.reject(err);
            return;
        }
        db.collection("blacklist").findOne({"processorID":processorID},function(err,result){
            if(err){ deferred.reject(err);db.close();return;}
            console.log("check result from db:",result);
            if(result && result.count > 10) {
                deferred.resolve(false);
            } else {
                deferred.resolve(true);
            }
            db.close();
        });
    });
    return deferred.promise;
};
