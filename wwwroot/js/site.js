// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("document").ready(function() {
    console.log("ready!");
    
    $(".signin").on("click", function(){
        $(".signpop").show("fast");
    });

    $(".xout").on("click", function(){
        $(".signpop").toggle();
    });

    $(".userinfo label").on("click", function(){
        $("#account-toggle").toggle("fast");
    });
});