'use strict';


(function () {


    // Add Errors to the Validation Summary
    var showSummary = function (response) {
        $('#validationSummary').empty().removeClass('hidden');

        var verboseErrors = _.flatten(_.map(response, 'Errors')),
            errors = [];

        var nonNullErrors = _.reject(verboseErrors, function (error) {
            return error.ErrorMessage.indexOf('must not be empty') > -1;
        });

        _.each(nonNullErrors, function (error) {
            errors.push(error.ErrorMessage);
        });

        if (nonNullErrors.length !== verboseErrors.length) {
            errors.push('The highlighted fields are required to submit this form.');
        }

        var $ul = $('#validationSummary').append('<ul></ul>');

        _.each(errors, function (error) {
            var $li = $('<li></li>').text(error);
            $li.appendTo($ul);
        });
    };

    // Iterate over each of the errors in the response, find the form-group and add the has-error class.
    var highlightFields = function (response) {

        $('.form-group').removeClass('has-error');

        $.each(response, function (propName, val) {
            var nameSelector = '[name = "' + propName.replace(/(:|\.|\[|\])/g, "\\$1") + '"]',
                idSelector = '#' + propName.replace(/(:|\.|\[|\])/g, "\\$1");
            var $el = $(nameSelector) || $(idSelector);
            var errorSpan = $('span[data-valmsg-for="' + val.Key + '"]');
            errorSpan.text("");
            if (val.Errors.length > 0) {
                $el.closest('.form-group').addClass('has-error');
                var errorMessage = val.Errors[0].ErrorMessage;

                errorSpan.addClass('text-danger');
                // Update Html instead of text in case any html is returned in the message
                errorSpan.html(errorMessage);
            }
        });
    };
    // If the Ajax returns an error (e.g. 400), then run this coe to hightlight
    var highlightErrors = function (xhr) {
        try {
            var data = JSON.parse(xhr.responseText);
            highlightFields(data);
            showSummary(data);
            window.scrollTo(0, 0);
        } catch (e) {
            // (Hopefully) caught by the generic error handler in `config.js`.
        }
    }




    // When the Ajax returns a 200 Reponse, it will either redirect or reload the screen
    var redirect = function (data) {

        if (data.redirect) {
            window.location = data.redirect;
        } 
    };

    $('form[method=post]').not('.no-ajax').on('submit',
        function () {
            
/*            if (!$(this).valid()) {
                return 0;
            }*/

            var submitBtn = $(this).find('[type="submit"]');

            submitBtn.prop('disabled', true);
            $(window).unbind();



            var $this = $(this),
                formData = $this.serializeArray();


            $this.find('div').removeClass('has-error');

            $.ajax({
                url: $this.attr('action'),
                type: 'post',
                data: $.param(formData),
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: 'json',
                statusCode: {
                    200: redirect
                },
                complete: function () {
                    submitBtn.prop('disabled', false);
                }
            }).error(highlightErrors);

            return false;
        });
}());