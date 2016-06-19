var weapsy = weapsy || {};
weapsy.admin = weapsy.admin || {};
weapsy.utils = weapsy.utils || {};

weapsy.utils = (function ($) {
    function showError(event, request, settings, thrownError) {
        var title =  "Error";
        var message = "Something went wrong.";

        if (request.status === 0) {
            //alert("Not connect.n Verify Network.");
            title = "Not Connected";
            message = "Please verify your network connection.";
        } else if (request.status === 400) {
            title = "Bad Request";
            var badRequestText = request.responseJSON;
            if (badRequestText != null) message = badRequestText;
        } else if (request.status === 404) {
            //alert("Requested page not found. [404]");
            title = "Not Found";
            message = "Requested resource not found.";
        } else if (request.status === 500) {
            //alert("Internal Server Error [500].");
            //} else if (exception === 'parsererror') {
            //    alert('Requested JSON parse failed.');
            //} else if (exception === 'timeout') {
            //    alert('Time out error.');
            //} else if (exception === 'abort') {
            //    alert('Ajax request aborted.');
            var errorText = $(request.responseText).filter(".titleerror").text();
            if (errorText != null) message = errorText;
        }

        $("#error-title").text(title);
        $("#error-message").text(message);
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
        showError: showError,
        getUrlParameterByName: getUrlParameterByName,
        getUrlParts: getUrlParts
    };
}(jQuery));

(function ($) {
    $(document).ajaxError(function (event, request, settings, thrownError) {
        weapsy.utils.showError(event, request, settings, thrownError);
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
