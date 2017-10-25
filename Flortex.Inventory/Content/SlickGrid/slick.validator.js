/***
 * Contains basic SlickGrid formatters.
 * 
 * NOTE:  These are merely examples.  You will most likely need to implement something more
 *        robust/extensible/localizable/etc. for your use!
 * 
 * @module Formatters
 * @namespace Slick
 */

(function ($) {
    // register namespace
    $.extend(true, window, {
        "Slick": {
            "Validator": {
                "Decimal": decimalFieldValidator
            }
        }
    });

    function decimalFieldValidator(value) {
        if (value == null || value == undefined || !value.length || !$.isNumeric(value)) {
            return { valid: false, msg: "This is a required field" };
        }
        else {
            return { valid: true, msg: null };
        }
    }

})(jQuery);
