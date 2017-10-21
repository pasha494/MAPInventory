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
            "Formatters": {
                "Decimal": DecimalFormatter
            }
        }
    });

    function DecimalFormatter(row, cell, value, columnDef, dataContext) { 
        if (value == undefined || value == "")
            return value;
        else
            return parseFloat(value).toFixed(2);
    }

})(jQuery);
