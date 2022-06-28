$(document).ready(function () {
    WorkflowStatusSequence();
});
window.onbeforeunload = function () {
    OnUpdateStatusSort(function () {

    });
}

function OnUpdateStatusSort(callback) {
    var tempArray = new Array();
    $('#tblStatusSequence').find("tbody").find("tr").each(function (i, elmnt) {
        tempArray.push({
            ID: parseInt($(elmnt).find("span[name=spnName]").data("dataid")),
            SequenceID: (i + 1)
        });
    });
    var data = {
        WorkflowID: $("#hdnWorkflowID").val(),
        WorkflowVersionID: $("#hdnWorkflowVersionID").val(),
        StatusList: tempArray
    };
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        url: (ROOTPath + "/Workflows/WorkflowStatus/SequenceUpdate"),
        data: JSON.stringify(data),
        success: function (result) {
            if (result.Status) {
            }
        }
    });
}

function WorkflowStatusSequence() {
    var model = {
        WorkflowID: $("#hdnWorkflowID").val(),
        WorkflowVersionID: $("#hdnWorkflowVersionID").val()
    };
    $('#tblStatusSequence').dataTable({
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
        "paging": false,
        "ordering": false,
        "ajax": {
            "url": ROOTPath + "/Workflows/WorkflowStatusSequence/List",
            "type": "POST",
            "contentType": "application/json",
            "data": function (data) {
                data.model = model;
                return JSON.stringify(data);
            }
        },
        "destroy": true,
        "columns": [
            {
                name: "UpAndDown", title: "", "data": function (data, type, val, meta) {
                    var tempString = "";
                    if (meta.row == 0) {
                        tempString = "<i class=\"fa fa-arrow-circle-down fa-lg dtMoveDown\"></i>";
                    }
                    else {
                        tempString = "<i class=\"fa fa-arrow-circle-up fa-lg dtMoveUp\"></i> <br /> <i class=\"fa fa-arrow-circle-down fa-lg dtMoveDown\"></i>";
                    }
                    return tempString;
                }, ordable: false, width: "5%"
            },
            {
                name: "C.Name", title: fromStatus, "data": function (data, type) {
                    return "<span name=\"spnName\" data-dataid=\"" + data.WorkflowStatusID + "\">" + data.FromStatus + "</span>";
                }, ordable: false, width: "16%"
            },
            {
                name: "ToStatus", title: toStatus, "data": function (data, type) {
                    var tempString = "";
                    if (data.Sequence != null && data.Sequence != "") {
                        var sequence = data.Sequence.split(",");
                        for (var i in sequence) {
                            var tempArray = sequence[i].split("^");
                            if (tempArray.length > 0)
                                tempString += tempArray[0] +
                                    "<button class=\"btn btn-primary btn-xs m-l-xs\" onclick=\"OnDeleteSequence(" + tempArray[2] + "," + data.WorkflowStatusID + ")\" type=\"button\" name=\"btnDelete\"><i class=\"fa fa-trash\"></i></button> <br />";
                        }
                    }
                    return tempString;
                },
                width: "16%", sortable: false
            },
            {
                name: "Screen", title: screen, "data": function (data, type) {
                    var tempString = "";
                    if (data.Sequence != null && data.Sequence != "") {
                        var sequence = data.Sequence.split(",");
                        for (var i in sequence) {
                            var tempArray = sequence[i].split("^");
                            if (tempArray.length > 0)
                                tempString += tempArray[4] + "<br />";
                        }
                    }
                    return tempString;
                },
                sortable: false, width: "18%"
            },
            {
                name: "Role", title: group, "data": function (data, type) {
                    var tempString = "";
                    if (data.Sequence != null && data.Sequence != "") {
                        var sequence = data.Sequence.split(",");
                        for (var i in sequence) {
                            var tempArray = sequence[i].split("^");
                            if (tempArray.length > 0) {
                                var group = tempArray[5].split("&");
                                if (group.length > 0) {
                                    for (var j in group) {
                                        if (group[j] != "" || group[j] != null)
                                            tempString += "<span class=\"label label-primary m-r-xs display_inline_block\">" + group[j] + "</span>";
                                    }
                                }
                                tempString += "<br />";
                            }
                        }
                    }
                    return tempString;
                },
                sortable: false, width: "35%"
            },
            {
                "data": function (data, type) {
                    str = "<a onclick=\"OnOpenSequenceModal(" + data.WorkflowStatusID + ");\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + add + " </a>";
                    return str;
                },
                className: "text-right", sortable: false,
                title: action, width: "10%"
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != $(nRow).find("td").length - 2) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        },
        'drawCallback': function (settings) {

            $('#tblStatusSequence tr:last .dtMoveDown').remove();
            // Remove previous binding before adding it
            $('.dtMoveUp').unbind('click');
            $('.dtMoveDown').unbind('click');

            // Bind click to function
            $('.dtMoveUp').click(moveUp);
            $('.dtMoveDown').click(moveDown);
        }
    });
}

// Move the row up
function moveUp() {
    var tr = $(this).parents('tr');
    moveRow(tr, 'up');
}

// Move the row down
function moveDown() {
    var tr = $(this).parents('tr');
    moveRow(tr, 'down');
}

// Move up or down (depending...)
function moveRow(row, direction) {
    var table = $('#tblStatusSequence').dataTable();
    var index = table.api().row(row).index();

    var order = -1;
    if (direction === 'down') {
        order = 1;
    }

    var data1 = table.api().row(index).data();
    data1.order += order;

    var data2 = table.api().row(index + order).data();
    data2.order += -order;

    table.api().row(index).data(data2);
    table.api().row(index + order).data(data1);
    table.api().settings()[0].oFeatures.bServerSide = false;
    table.api().settings()[0].ajax = false;
    table.api().page(0).draw(false);
}

function OnCloseSequence() {
    $("#dvWorkFlowModal").modal("hide");
}

function OnOpenSequenceModal(id) {
    var data = {
        workflowID: parseInt($("#hdnWorkflowID").val()),
        id: id
    };
    $.get(ROOTPath + "/Workflows/WorkflowStatusSequence/Get", data, function (result) {
        $("#dvWorkFlowModalContent").empty().html(result);
        $(".select2dropdown").select2({
            width: "100%"
        });
    });

    $.validator.addMethod("ondropdownReq", function (value, element) {
        return $(element).val() == null || $(element).val() == "" || parseInt($(element).val()) > 0;
    }, requiredField);
    $("#dvWorkflowModaldialog").removeClass("modal-md").addClass("modal-lg");
    $("#dvWorkFlowModal").modal("show");
}

function OnAddGridStatusSeq() {
    var isFormValid = $("#frmSequence").valid();
    if (isFormValid) {
        var count = 0;
        $("#tblAddStatusSeq").find("tbody").find("tr").each(function (i, elmnt) {
            if (parseInt($(elmnt).find("input[name=hdnStatusSequences]").data("targetid")) == parseInt($("#WorkflowStatusTargetID").val())) {
                count++;
            }
        });
        if (count == 0) {
            var row = "<tr>";
            row += "<td>";
            row += $("#WorkflowStatusTargetID").find("option:selected").text();
            row += "<input type=\"hidden\" name=\"hdnStatusSequences\" data-targetid=\"" + $("#WorkflowStatusTargetID").val() + "\" data-formid=\"" + $("#FormID").val() + "\" data-groupid=\"" + $("#GroupID").val() + "\" />";
            row += "</td>";
            row += "<td>";
            row += $("#FormID").find("option:selected").text();
            row += "</td>";
            row += "<td>";
            var tempString = "";
            $("#GroupID").find("option:selected").each(function (i, elmnt) {
                tempString += "<span class=\"label label-primary m-r-xs display_inline_block\">" + $(elmnt).text() + "</span>";
            });
            row += tempString;
            row += "</td>";
            row += "<td>";
            row += "</td>";
            row += "</tr>";
            $("#tblAddStatusSeq").find("tbody").append(row);

            $('#WorkflowStatusTargetID').val($('#WorkflowStatusTargetID').find("option").eq(0)).trigger('change.select2');
            $('#FormID').val($('#WorkflowStatusTargetID').find("option").eq(0)).trigger('change.select2');
            $('#GroupID').val("").trigger('change.select2');
        }
    }
    return isFormValid;
}

function CreateWorkflowStatusSequenceModal() {
    var tempArray = new Array();
    $("#tblAddStatusSeq").find("tbody").find("tr").each(function (i, elmnt) {
        tempArray.push({
            WorkflowStatusTargetID: parseInt($(elmnt).find("input[name=hdnStatusSequences]").data("targetid")),
            FormID: parseInt($(elmnt).find("input[name=hdnStatusSequences]").data("formid")),
            Groups: $(elmnt).find("input[name=hdnStatusSequences]").attr("data-groupid")
        });
    });
    var data = {
        WorkflowID: parseInt($("#hdnWorkflowID").val()),
        WorkflowVersionID: parseInt($("#hdnWorkflowVersionID").val()),
        WorkflowStatusID: $("#hdnCurrentStatusID").val(),
        WorkflowStatusSequenceList: tempArray
    };
    return data;
}

function OnAddSequence() {
    if (ValidateSeq()) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            url: (ROOTPath + "/Workflows/WorkflowStatusSequence/Add"),
            data: JSON.stringify(CreateWorkflowStatusSequenceModal()),
            success: function (result) {
                if (result.Status) {
                    showAlert("success", result.Message);
                    $("#dvWorkFlowModal").modal("hide");
                    WorkflowStatusSequence();
                    WorkflowGroupGet();
                    WorkflowGroupUsersGet();
                }
                else {
                    showAlert("error", result.Message);
                }
            }
        });
    }
}

function OnDeleteSequence(targetID, statusID) {
    swal({
        title: warningMsg,
        type: "warning",
        showCancelButton: true,
        cancelButtonText: cancel,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: deletetext,
        closeOnConfirm: true
    }, function () {
        var data = {
            WorkflowID: parseInt($("#hdnWorkflowID").val()),
            WorkflowVersionID: parseInt($("#hdnWorkflowVersionID").val()),
            WorkflowStatusID: statusID,
            WorkflowStatusTargetID: targetID
        };
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            url: (ROOTPath + "/Workflows/WorkflowStatusSequence/Delete"),
            data: JSON.stringify(data),
            success: function (result) {
                if (result.Status) {
                    showAlert("success", result.Message);
                    WorkflowStatusSequence();
                }
                else {
                    showAlert("error", result.Message);
                }
            }
        });
    });

}

function ValidateSeq() {
    var isValid = true;
    isValid = $("#tblAddStatusSeq").find("tbody").find("tr").length > 0;
    return isValid;
}