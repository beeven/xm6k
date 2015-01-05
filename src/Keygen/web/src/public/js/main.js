;(function($){
    $(document).ready(function(){
        $("#sendmail-form").submit(function(event){
            event.preventDefault();
            
            $.post("sendmail.html",$("#sendmail-form").serialize())
                .done(function(){
                    alert("mail sent");
                })
                .fail(function(err){
                    alert(err);
                });
        });
    });
})(jQuery);