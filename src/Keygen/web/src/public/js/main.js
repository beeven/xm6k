;(function($){
    $(document).ready(function(){
        $("#sendmail-form").submit(function(event){
            event.preventDefault();
            $("#sendmail-form .btn").prop("disabled","disabled");

            $.post("sendmail/",$("#sendmail-form").serialize())
                .done(function(){
                    alert("mail sent");
                })
                .fail(function(err){
                    alert(err);
                })
                .always(function(){
                    $("#sendmail-form .btn").removeProp("disabled");
                });
        });
    });
})(jQuery);