var mongoclient = require("mongodb").MongoClient,
    assert = require("assert");
mongoclient.connect("mongodb://localhost:27017/xm6k",function(err,db){
    assert.equal(null,err);
    assert.ok(db!=null);
    db.close();
});
