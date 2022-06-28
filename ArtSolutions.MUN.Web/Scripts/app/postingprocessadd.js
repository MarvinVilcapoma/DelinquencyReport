$(document).ready(function () {
    $("#btnSearch").focus();
    if ($(".select2dropdown").length)
        $(".select2dropdown").select2();
    initializeDatePicker();
});
var table;
$(document).on('click', '#btnSearch', function () {
    InitializeData();
});

$("#btnrefresh").click(function () {
    $('#AsOfDate').val('');
    $("#DocumentTypeID").val("").trigger("change.select2");
    $("#btnSearch").click();
});

function initializeDatePicker() {
    $('.periodDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    $('.periodDate').datepicker('update', new Date());
}

function getDocumentTypeIDs() {
    var selectDocumentTypeList = [];
    if ($("#DocumentTypeID").select2('data').length) {
        $.each($("#DocumentTypeID").select2('data'), function (key, item) {
            if (item.id != 0)
                selectDocumentTypeList += "," + item.id;
        });
    }
    var selectedDocumentTypeIDs = "";
    if (selectDocumentTypeList.length > 0)
        selectedDocumentTypeIDs = selectDocumentTypeList.substring(1, selectDocumentTypeList.length);
    return selectedDocumentTypeIDs;
}


function getJournalIDs() {
    var JournalID = "";    
    table.rows().nodes().to$().find('input[type="checkbox"]:checked').each(function (index, item) {
        if (index == 0)
            JournalID = $(item).closest("td").data("journalid");
        else
            JournalID += "," + $(item).closest("td").data("journalid");
    });
    return JournalID;
}
var indexGroup;
function InitializeData() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + '/PostingProcess/GetJournalListForPosting',
            data: {
                'AsOfDate': $("#AsOfDate").val() != '' ? new Date($("#AsOfDate").datepicker('getDate')).toDateString() : null
                , 'DocumentTypeID': getDocumentTypeIDs()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#JournalListForPosting").html("").html(response.data);
                    indexGroup = null;
                    var groupColumn = 13;
                    table = $('#tblJournalDetail').dataTable({
                        "oLanguage": {
                            "sEmptyTable": nodatamsg,
                            oPaginate: {
                                sPrevious: previous,
                                sNext: next
                            }
                        },
                        "bFilter": false,
                        "bInfo": false,
                        "bLengthChange": false,
                        "autoWidth": false,
                        "ordering": false,
                        "columnDefs": [
                            { "visible": false, "targets": groupColumn }
                        ],
                        "pageLength": pagelen,
                        "drawCallback": function (settings) {
                            var api = this.api();
                            var pageInfo = api.page.info();
                            var rows = api.rows({
                                page: 'current'
                            }).nodes();
                            var last = null;
                            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                                var isAvailableColspan = true;
                                if (i == 0) {
                                    isAvailableColspan = $(api.rows({ page: 'current' }).nodes()[0]).find('td[colspan="7"]').length > 0;
                                }
                                if (last !== group && indexGroup != null && i == 0 && pageInfo.page != 0 && isAvailableColspan) {
                                    var $TRColumn = api.rows().nodes().to$().find('td[data-journalid="' + group + '"]').closest("tr");
                                    var html = '<tr class="group">' +
                                        '<td>' + $TRColumn.find('td:eq(0)').html() + '</td>' +
                                        '<td>' + $TRColumn.find('td:eq(1)').html() + '</td>' +
                                        '<td>' + $TRColumn.find('td:eq(2)').html() + '</td>' +
                                        '<td>' + $TRColumn.find('td:eq(3)').html() + '</td>' +
                                        '<td>' + $TRColumn.find('td:eq(4)').html() + '</td>' +
                                        '<td>' + $TRColumn.find('td:eq(5)').html() + '</td>' +
                                        '<td>' + $TRColumn.find('td:eq(6)').html() + '</td>' +
                                        '<td colspan="6"></td>' +
                                        '</tr>';
                                    $(rows).eq(i).before(
                                        html
                                    );

                                    if ($("#chkAll").is(":checked")) {
                                        $(".chkJournal").prop("checked", true);
                                    }
                                    last = group;
                                }
                                indexGroup = group;
                            });
                        }
                    }).api();
                }
                else {
                    showAlert('error', response.message);
                }
            }
        });
    }
}

function validateForm() {
    //var isvalid = false;
    //if ($("#AsOfDate").val() == undefined || $("#AsOfDate").val() == '')
    //    showAlert('error', $("#AsOfDate").attr("data-required-msg"));
    //else
    //    isvalid = true;
    //return isvalid;
    return true;
}

$("#JournalListForPosting").on("click", "#chkAll", function () {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        table.rows().nodes().to$().find('input[type="checkbox"]').prop("checked", true);
        $(".chkJournal").prop("checked", true);
    }
    else {
        table.rows().nodes().to$().find('input[type="checkbox"]').prop("checked", false);
        $(".chkJournal").prop("checked", false);
    }
});

$("#JournalListForPosting").on("click", ".chkJournal", function () {

    if (!$(this).is(":checked")) {
        if ($("#chkAll").is(":checked"))
            $("#chkAll").prop("checked", false);
        table.rows().nodes().to$().find('td[data-journalid="' + $(this).closest("td").data("journalid") + '"]').closest("tr").find('input[type="checkbox"]').prop("checked", false);
        table.rows().nodes().to$().find('td[data-journalid="' + $(this).closest("td").data("journalid") + '"]').closest("tr").find('input[type="checkbox"]').removeAttr("checked");
    }
    else {
        table.rows().nodes().to$().find('td[data-journalid="' + $(this).closest("td").data("journalid") + '"]').closest("tr").find('input[type="checkbox"]').prop("checked", true);
        table.rows().nodes().to$().find('td[data-journalid="' + $(this).closest("td").data("journalid") + '"]').closest("tr").find('input[type="checkbox"]').attr("checked", "checked");
    }
    
    if (table.rows().nodes().to$().find('input[type="checkbox"]:checked').length == table.rows().nodes().to$().find('input[type="checkbox"]').length) {
        $('#chkAll').prop('checked', true);
    }

});

$("#btnSave").on("click", function () {
    var retval = true;
    if ($(".chkJournal:checked").length <= 0) {
        retval = false;
        showAlert('error', RequiredSelectionMsg);
    }
    if (retval) {
        $.ajax({
            type: "POST",
            url: ROOTPath + '/PostingProcess/Add',
            data: {
                'DocumentTypeDetail': $("#DocumentTypeID option:selected").text()
                , 'JournalID': getJournalIDs()
                , 'AsOfDate': $("#AsOfDate").val()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    window.location.href = ROOTPath + '/Collections/PostingProcess/View?ID=' + response.postingProcessID;
                }
                else {
                    showAlert('error', response.message);
                }
            }
        });
    }
});

function ValidateWorkFlowStatus($this) {   
    var retval = true;
    if ($(".chkJournal:checked").length <= 0) {
        retval = false;
        showAlert('error', RequiredSelectionMsg);
    }
    if (retval) {
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: $($this).text(),
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed) {
                $.ajax({
                    type: "POST",
                    url: ROOTPath + '/PostingProcess/Add',
                    data: {
                        'DocumentTypeDetail': $("#DocumentTypeID option:selected").text()
                        , 'JournalID': getJournalIDs()
                        , 'AsOfDate': $("#AsOfDate").val()                        
                        , 'workflowID': $("#hdnWorkflowID").val()
                        , 'workflowStatusID': $($this).val()
                    },
                    success: function (response) {
                        if (response.status) {
                            hideLoading();
                            window.location.href = ROOTPath + '/Collections/PostingProcess/View?ID=' + response.postingProcessID;
                        }
                        else {
                            showAlert('error', response.message);
                        }
                    }
                });
            }
            else {
                return false;
            }
        });
    }
}