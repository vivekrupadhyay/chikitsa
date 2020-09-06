$(document).ready(function () {
    $(".sidebar-menu li.active").parents(".treeview").addClass("active");
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "2000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        "tapToDismiss": true
    }
    $("body").on("click", ".kdtable th:not('.nosort')", function () {
        //$(".kdtable th:not('.nosort')").click(function () {
        var qBOSort = $(".dvFilter [name='objFilter.Sort']").val();
        var qBOSortColumn = qBOSort.split(' ')[0];
        var qBOOrder = qBOSort.split(' ').length > 1 ? qBOSort.split(' ')[1] : "";
        var column = $(this).attr('data-sort') ? $(this).attr('data-sort') : $(this).text().trim();
        var order = "asc";
        if (qBOSortColumn == column) { if (qBOOrder == "asc") order = "desc" }
        $(".dvFilter [name='objFilter.Sort']").val(column + ' ' + order);
        $(".dvFilter .btnFilter").click();
    });
    $("body").on("click", ".btnFilter", function () {
        //$(".btnFilter").click(function () {
        $("input[name='objFilter.PageNumber']").val('1');
    })
    $("body").on("click", ".btnClear", function () {
        //$(".btnClear").click(function () {
        $("input[type='text']").val('');
        $("select").val('');
    })
    $("body").on("click", ".kdtable .btnAction", function () {
        debugger;
        $(this).parent().find(".FilterBO").append($(".dvFilter input"));
    });
    $("body").on("click", "input[type='button'],button:not(.noloader),a[href!='#']", function () {
        if (this.type == "submit") {
            if ($("form").valid())
                ShowProgress();
        }
        else
            //if (!this.hasClass(".noloader"))
            ShowProgress();
    });
    EMSPageLoad();
});
function EMSPageLoad() {
    debugger;
    var tdActionData = "";
    $(".kdtable .tdAction").each(function (i, item) {
        tdActionData += $(item).text();
    });
    if (tdActionData.trim() == "") {
        $(".kdtable .thAction, .kdtable .tdAction").remove();
    }

    var elems = document.querySelectorAll('.js-switch');
    if (elems) {
        elems.forEach(function (html) {
            var switchery = new Switchery(html, { size: 'small' });
        });
    }
    $.validator.unobtrusive.parse("#dvBody");
}


function Unloader() {
    $(".modalload").hide();
    $(".loading").hide();
    $('body').css("overflow-y", "auto");
    var modal = $(".modal-dialog");
    modal.css({ "z-index": "2050" });
}

function ShowProgress() {
    var modal = $('<div />');
    modal.addClass("modalload");
    $('body').append(modal);
    $('body').css("overflow-y", "hidden");
    var loading = $(".loading");
    loading.show();
    var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
    var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
    loading.css({ top: top, left: left });
    var modal = $(".modal-dialog");
    modal.css({ "z-index": "1001" });
}

function CancelForm(url) {
    location.href = url;
}

function SwalConfirm(msg, evntId) {
    swal({
        title: "Are you sure?",
        text: msg,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
    function () {
        evntId.attr("onclick", "");
        evntId.click();
    });
    return false;
}