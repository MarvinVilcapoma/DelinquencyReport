$(document).ready(function () {
    $('#dvUpdate').hide();
    $(".select2dropdown").select2({ width: '300px' });
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnTransferType;

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
        return true;
    }
    return false;
});

$(window).resize(function () {
    if (validateForm()) {
        InitializeData();
        return true;
    }
    return false;
});

function SetResetFilterOption() {
    if (hdnTransferType != undefined) {
        $("#ddlTransferType").val(hdnTransferType).trigger('change');
    }
    else {
        $("#ddlTransferType").val("0").trigger('change');
    }
}

function clearSearch(filterCriteria) {
    $("#ddlTransferType").val("0").trigger('change');;
    $("#spnSelectedTransferType").addClass('hide');
    hdnTransferType = "";
    InitializeData();
}

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    var dt = new Date();
    $('#FromDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#ToDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

function validateForm() {
    var isvalid = true;
    $("#FromDate,#EndDate").removeClass("error");
    if ($("#FromDate").val() == undefined || $("#FromDate").val() == '') {
        showAlert('error', $("#FromDate").attr("data-required-msg"));
        $("#FromDate").addClass("error");
        isvalid = false;
    }
    if ($("#ToDate").val() == undefined || $("#ToDate").val() == '') {
        showAlert('error', $("#ToDate").attr("data-required-msg"));
        $("#ToDate").addClass("error");
        isvalid = false;
    }
    if (new Date($("#ToDate").datepicker('getDate')) < new Date($("#FromDate").datepicker('getDate'))) {
        showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
        isvalid = false;
    }

    if (isvalid) {
        hdnTransferType = $("#ddlTransferType").val();
        $("#TransferTypeId").val(hdnTransferType).trigger('change');
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

    if ($("#ddlTransferType").val() != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedTransferType").removeClass('hide');
        $("#TransferTypeIdSearchText").text(selectedTransferType + " : " + $("#ddlTransferType option:selected").text());
    }
    else {
        $("#spnSelectedTransferType").addClass('hide');
        $("#TransferTypeIdSearchText").text('');
    }
    if (isvalid) {
        hdnTransferType = $("#ddlTransferType").val();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    $("#ddlTransferType").val('0');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
    $("#ddlTransferType").focus();

    if (hdnTransferType <= 0) {
        $("#ddlTransferType").val("0").trigger("change");
    }
}

function initDataTable() {
    $('#tblTransfersReport').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false,
        fixedHeader: true,
        language: {
            "emptyTable": nodatamsg
        },
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,
        "fnDrawCallback": function () {
            $("#divBottomContent").removeClass("hide");

            $('#tblTransfersReport tbody tr td').each(function () {

                if ($(this).index() == 2) {
                    this.setAttribute('title', $(this).find('#hdPreviousOwner').val());
                }
                else if ($(this).index() == 4) {
                    this.setAttribute('title', $(this).find('#hdCurrentOwner').val());
                }
                else if ($(this).index() == 7) {
                    this.setAttribute('title', $(this).find('#hdService').val());
                }
                else if ($(this).index() == 11) {
                    this.setAttribute('title', $(this).find('#hdObservation').val());
                }
                else {
                    this.setAttribute('title', $(this).text());
                }
            });

            /* Apply the tooltips */
            $('#tblTransfersReport tbody tr td').tooltip({
                container: "body",
                html: true
            });
        }
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/TransfersReport",
        data: {
            'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString(),
            'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString(),
            'transferTypeID': $("#ddlTransferType").val()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divTransfersReport").html("").html(response.data);
                initDataTable();
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintTransfersReport",
            data: {
                'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString()
                , 'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString()
                , 'transferTypeID': $("#ddlTransferType").val()
            },
            beforeSend: function () {
                showLoading();
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    var fun = function () {
                        printWindow.document.close();
                        setTimeout(function () { printWindow.print(); }, 20);
                        printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
                    };
                    var img = printWindow.document.getElementById('img-responsive');
                    if (img.complete) {
                        fun.call(img);
                    } else {
                        img.onload = fun
                    }
                }
                else {
                    showAlert('error', response.data);
                }
            }
        });
    }
    else
        return false;
    return true;
}