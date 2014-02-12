$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() > $('#header-featured').height() + $('#header-wrapper').height() + 312) {
            if (!$("#header-wrapper").hasClass("LightHeader")) {
                $("#header-wrapper").addClass("LightHeader");
                $("#menu").removeClass("Menu-Color");
            }
        }
        else {
            if ($("#header-wrapper").hasClass("LightHeader")) {
                $("#header-wrapper").removeClass("LightHeader");
                $("#header-wrapper").hide();
                $("#header-wrapper").fadeIn(250);
                $("#menu").addClass("Menu-Color");
            }
        }
    });
});