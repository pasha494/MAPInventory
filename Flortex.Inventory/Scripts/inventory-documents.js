

(function ($) {
    $.extend(true, window, {
        InventoryDocs: {
            ParseToSlickColumns: _parseToSlickColumns
        }
    });



    function _parseToSlickColumns(_columns, _autocompleteListViewOptions) { 
        var options = {
            columns:[],
            autocompleteListViewOptions:{},
            slickcolumns: []

        };
         
        var init=function()   {   
            options.columns=_columns;
            options.autocompleteListViewOptions = _autocompleteListViewOptions;

            options.slickcolumns = _.each(options.columns, function (item, i, arr) {
                if (item.editor) {

                    if (item.editor == "Slick.Editors.AutocompleteListView") {
                        item.editorOptions = options.autocompleteListViewOptions;
                    }

                    item.editor = executeFunctionByName(item.editor, window);
                }

                if (item.formatter) {
                    item.formatter = executeFunctionByName(item.formatter, window);
                }

                if (item.validator) {
                    item.validator = executeFunctionByName(item.validator, window);
                }

                if (item.width) {
                    item.width = parseFloat(item.width);
                }
            }); 
        }

        
        function executeFunctionByName(functionName, context /*, args */) {
            var args = [].slice.call(arguments).splice(2);
            var namespaces = functionName.split(".");
            var func = namespaces.pop();
            for (var i = 0; i < namespaces.length; i++) {
                context = context[namespaces[i]];
            }
            return context[func];
        }

        function getOptions(name)
        {
            if (options[name])
                return options[name];
            else
                throw name + " is not part of the options";

        }

        $.extend(this, {
            // Added by Pasha
            // set wareHouse id 
            "getOptions": getOptions
        });

        init();
    }

}(jQuery));