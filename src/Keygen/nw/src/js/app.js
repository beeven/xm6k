var fs = require("fs");
//var process = require("process");
(function($){
  var output = $("#outputArea");
  $("#btnPatch").click(function(){
     output.val(fs.realpathSync("."));
  });
})(jQuery);
