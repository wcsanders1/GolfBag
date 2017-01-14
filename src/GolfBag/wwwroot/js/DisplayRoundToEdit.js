/*********************************************************************************************
                        STRIPES ROW OF SELECTED TEEBOX
*********************************************************************************************/
$(function () {
    "use strict";

    var stripe = function () {
        stripeRow($(this));
    }

    $(document).on("change", ".teebox-selector", stripe);
});



/*********************************************************************************************
               JQUERY VALIDATOR ON FORM
*********************************************************************************************/

$(function () {
    "use strict";

    var $scorecard = $(".writable-scorecard");

    $scorecard.removeData("validator").removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse($(".writable-scorecard"));
    validateForm($(".writable-scorecard"));
});