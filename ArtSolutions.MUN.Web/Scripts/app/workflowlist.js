$(document).ready(function () {
    WorkflowGet();
    $("#btnrefresh").click(function () {
        $("#txtSearch").val('');
        $("#DocumentTypeID").val("0").trigger("change.select2");
        WorkflowGet();
    });
    $("#btnSearch").click(function () {
        WorkflowGet();
    });
    $('#txtSearch').bind("keypress", function (e) {
        if (e.keyCode == 13) {
            WorkflowGet();
            return false;
        }
    });
    $(".select2dropdown").select2({
        width: "100%"
    });
    $("#DocumentTypeID").change(function () {
        WorkflowGet();
    });
});

function WorkflowGet() {
    $('#ListTable').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                "sFirst": first,
                "sLast": last,
                "sNext": next,
                "sPrevious": previous
            }
        },
        "serverSide": true,
        "processing": false,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ajax": {
            "url": ROOTPath + "/Workflows/Workflow/List?IsBack=" + isBack,
            "type": "POST",
            "contentType": "application/json",
            "data": function (data) {
                data.model = {
                    FilterText: $("#txtSearch").val(),
                    DocumentTypeID: parseInt($("#DocumentTypeID").val())
                };
                return JSON.stringify(data);
            }
        },
        "destroy": true,
        "columns": [
            {
                "data": function (data, type) {
                    var str = "";
                    if (!data.IsPublic) {
                        if (data.IsActive) {
                            str = "<a onclick=\"ConfirmActivedeactive(\'Active\'," + data.ID + ")\"><span class=\"label label-primary\">" + active + "</span></a>";
                        }
                        else {
                            str = "<a onclick=\"ConfirmActivedeactive(\'Deactive\'," + data.ID + ")\"><span class=\"label label-warning\">" + inActivebtn + "</span></a>";
                        }
                    }
                    return str;
                },
                className: "col-lg-1", sortable: false,
                title: ""
            },
            {
                name: "D.Name", title: documentType, "data": "DocumentType", className: "col-lg-2"
            },
            {
                name: "C.Name", title: name, "data": "Name", className: "col-lg-2"
            },
            {
                name: "C.Description", title: description, "data": function (data, type) {
                    return data.Description;
                }, className: "col-lg-5"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    if (data.IsPublic) {
                        str = "<a href=\"" + ROOTPath + "/Workflows/Workflow/Editor/" + data.ID + "?documentTypeID=" + data.DocumentTypeID + "\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                    }
                    else {
                        str = "<a href=\"" + ROOTPath + "/Workflows/Workflow/Editor/" + data.ID + "?documentTypeID=" + data.DocumentTypeID + "\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                        str += "<a href=\"#\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                    }
                    return str;
                },
                className: "text-right col-lg-2", sortable: false,
                title: action
            }
        ],
        "order": [[1, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != $(nRow).find("td").length - 1) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        }
    });
}
function ConfirmActivedeactive(str, val) {
    var textmessage = "",
        confMessage = "",
        action = true;
    if (str.trim().toLowerCase() == "active") {
        confMessage = inActive;
        action = false;
    }
    else {
        confMessage = activeConfirm;
        action = true;
    }
    swal({
        title: sureMessage,
        text: textmessage,
        type: "warning",
        showCancelButton: true,
        cancelButtonText: cancel,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confMessage,
        closeOnConfirm: true
    }, function () {
        InActiveAccount(val, action);
    });
}

function InActiveAccount(id, isActive) {
    var data = {
        ID: id,
        IsActive: isActive
    };
    $.ajax({
        type: "POST",
        url: ROOTPath + "/workflows/workflow/Active",
        "contentType": "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            var Message = response.message;
            if (response.status) {
                if (isActive == true) {
                    Message = activeMessage;
                }
                else {
                    Message = deActiveMessage;
                }
            }
            showAlert(response.ErrorType, Message);
            WorkflowGet();
        }, error: function () { }
    }).always(function () {
    });
}