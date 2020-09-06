$(function () {
    $.validator.methods.date = function (value, element) {
        if (navigator.userAgent.indexOf("webkit") > 0) {
            var d = new Date();
            return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        }
        else {
            return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
        }
    };
});