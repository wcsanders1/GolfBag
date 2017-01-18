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

    validateForm($(".writable-scorecard"), false);

    var validateThis = function () {
        validateCustom($(".writable-scorecard"));
    };

    $(document).on("click", ".custom-submit", validateThis);
});