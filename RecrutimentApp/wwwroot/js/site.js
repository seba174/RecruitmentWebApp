
(function ($) {

    function isValidDate(d) {
        return d instanceof Date && !isNaN(d);
    }

    jQuery.validator.addMethod('greaterthanzero',
        function (value, _element, _params) {
            if (value && value <= 0)
                return false;
            return true;
        });

    jQuery.validator.addMethod('notpastdate',
        function (value, _element, _params) {
            if (value) {
                var dateFromInput = new Date(value);
                if (!isValidDate(dateFromInput))
                    return true;

                var currentDate = new Date();
                if (dateFromInput < currentDate)
                    return false;
            }
            return true;
        });

    jQuery.validator.addMethod('moneynotgreaterthan',
        function (value, _element, params) {
            var $property = $('#' + params.otherattribute);
            var propertyVal = $property.val();

            if (value && propertyVal) {
                var propertyMoneyValue = parseFloat(propertyVal);
                var thisMoneyValue = parseFloat(value);

                if (!isNaN(propertyMoneyValue) && !isNaN(thisMoneyValue) && thisMoneyValue > propertyMoneyValue)
                    return false;
            }

            return true;
        });

    jQuery.validator.unobtrusive.adapters.add('greaterthanzero', [],
        function (options) {
            options.rules['greaterthanzero'] = [];
            options.messages['greaterthanzero'] = options.message;
        });

    jQuery.validator.unobtrusive.adapters.add('notpastdate', [],
        function (options) {
            options.rules['notpastdate'] = [];
            options.messages['notpastdate'] = options.message;
        });

    jQuery.validator.unobtrusive.adapters.add('moneynotgreaterthan', ['otherattribute'],
        function (options) {
            options.rules['moneynotgreaterthan'] = {
                otherattribute: options.params['otherattribute']
            };
            options.messages['moneynotgreaterthan'] = options.message;
        });

})(jQuery);