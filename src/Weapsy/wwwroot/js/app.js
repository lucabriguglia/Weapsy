var weapsy = weapsy || {};
weapsy.admin = weapsy.admin || {};
weapsy.utils = weapsy.utils || {};

weapsy.utils = (function ($) {
    function showLoading(text) {
        $("#main-success").hide();
        $("#main-loading").show();
        $("#main-loading-text").text(text !== "" ? text + "..." : "Loading...");
    }

    function showSuccess(text) {
        $("#main-loading").hide();
        $("#main-success").show().delay(2000).fadeOut();
        $("#main-success-text").text(text !== "" ? text : "Complete");
    }

    function showError(event, request, settings, thrownError) {
        $("#main-loading").hide();

        var title =  "An error occured";
        var message = "Something went wrong.";

        if (request.status === 0) {
            title = "Not Connected";
            message = "Please verify your network connection.";
        } else if (request.status === 400) {
            title = "Bad Request";
            var badRequestText = request.responseJSON;
            if (badRequestText != null)
                message = badRequestText;
        } else if (request.status === 404) {
            title = "Not Found";
            message = "Requested resource not found.";
        } else if (request.status === 500) {
            var errorText = $(request.responseText).filter(".titleerror").html();
            if (errorText != null) {
                message = errorText.replace("Exception: ", "");
            }
        }

        $("#error-title").text(title);
        $("#error-message").html(message);
        $("#error-modal").modal("show");
    }

    function getUrlParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return "";
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function getUrlParts(url) {
        if (!url) url = window.location.href;
        var urlParts = url.replace(/\/\s*$/, "").split("/");
        urlParts.shift();
        return urlParts;
    }

    return {
        showLoading: showLoading,
        showSuccess: showSuccess,
        showError: showError,
        getUrlParameterByName: getUrlParameterByName,
        getUrlParts: getUrlParts
    };
}(jQuery));

(function ($) {
    $("#main-loading").hide();
    $("#main-success").hide();

    $(document)
        .ajaxStart(function () {
          
        })
        .ajaxSuccess(function () {
         
        })
        .ajaxError(function (event, request, settings, thrownError) {
            weapsy.utils.showError(event, request, settings, thrownError);
        })
        .ajaxStop(function () {

        });

    //$.fn.serializeObject = function () {
    //    var o = {};
    //    var a = this.serializeArray();
    //    $.each(a, function () {
    //        if (o[this.name] !== undefined) {
    //            if (!o[this.name].push) {
    //                o[this.name] = [o[this.name]];
    //            }
    //            o[this.name].push(this.value || '');
    //        } else {
    //            o[this.name] = this.value || '';
    //        }
    //    });
    //    return o;
    //};

    //$.validator.setDefaults({
    //    errorClass: 'text-danger',
    //    highlight: function (element, errorClass, validClass) {
    //        $(element).removeClass(errorClass);
    //        var $formGroupElement = $(element).parent().parent();
    //        var $iconElement = $(element).next();
    //        $formGroupElement.addClass('has-error').removeClass('has-success');
    //        $iconElement.addClass('glyphicon-remove').removeClass('glyphicon-ok');
    //    },
    //    unhighlight: function (element, errorClass, validClass) {
    //        //$(element).addClass(validClass);
    //        var $formGroupElement = $(element).parent().parent();
    //        var $iconElement = $(element).next();
    //        $formGroupElement.addClass('has-success').removeClass('has-error');
    //        $iconElement.addClass('glyphicon-ok').removeClass('glyphicon-remove');
    //    }
    //});
}(jQuery));
