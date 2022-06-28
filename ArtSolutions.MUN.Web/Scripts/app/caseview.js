var Statuses = {
    Deceased: 9
};
$(document).ready(function () {
    $("body").on("click", "#ddlStatusAction li a", function () {
        $("#hdnStatusIDTargetName").val($(this).text().trim());
        var formID = $(this).attr("data-formid");
        var currentStatusID = parseInt($(this).attr("data-statusid"));
        if (currentStatusID > 0) {
            if (formID > 0) {
                WorkflowFormGet(formID, function (responce) {
                    for (var i in responce) {
                        if (responce[i].ID == 3) {
                            if (responce[i].UsePopup) {
                                DocumentWorkflowHistoryLogGet(caseID, function (logs) {
                                    if (logs.status) {
                                        $("#vertical-timeline").html(GetTimeline(logs.data));
                                        FillReasonsByStatusID(currentStatusID, workflowID, false, false, function (reasons) {
                                            if (reasons) {
                                                $("#hdnStatusIDTarget").val(currentStatusID);
                                                $("#ddlCommentReasons").empty();
                                                $("#ddlCommentReasons").html(reasons);
                                                $("#ddlCommentReasons").trigger("change");
                                                $('#dvActivityModel').modal({ backdrop: 'static', keyboard: false })
                                                $('#dvActivityModel').modal('show');
                                            }
                                        });
                                    }
                                });
                            }
                        }
                        else if (responce[i].ID == 1) {
                            window.location.href = ROOTPath + "/Cases/Case/PrintLetter?caseids=" + caseID + "&&statuidtarget=" + currentStatusID + "&&statuidsource=" + statusID + "&&workflowid=" + workflowID;
                        }
                    }
                });
            }
            else {
                var workflowHistoryLogs = new Array();
                var workflowHistoryLog = {
                    CaseID: caseID,
                    StatuIdSource: statusID,
                    StatuIdTarget: currentStatusID,
                    WorkflowID: workflowID,
                    WorkflowVersionID: 1
                };
                workflowHistoryLogs.push(workflowHistoryLog);
                var model = {
                    WorkflowHistoryLogs: workflowHistoryLogs
                };
                $.post(ROOTPath + "/Cases/Case/CasesStatusUpdate", model, function (result) {
                    if (result.Status) {
                        if (result.Message) {
                            showAlert("success", result.Message);
                        }
                        setTimeout(function () {
                            window.location.reload(true);
                        }, 5000);
                    }
                    else {
                        if (result.Message) {
                            showAlert("error", result.Message);
                        }
                    }
                });
            }
        }
    });

    $("#btnAddComments").click(function () {
        var workflowHistoryLogs = new Array();
        var workflowHistoryLog = {
            CaseID: caseID,
            StatuIdSource: statusID,
            StatuIdTarget: parseInt($("#hdnStatusIDTarget").val()),
            WorkflowID: workflowID,
            ReasonID: $("#ddlCommentReasons").val(),
            Comment: $("#Comments").val(),
            AssignToID: $("#ddlCommentAssignTo").val(),
            StatuIdTargetName: $("#hdnStatusIDTargetName").val(),
            ReasonName: $("#ddlCommentReasons option:selected").text()
        };
        workflowHistoryLogs.push(workflowHistoryLog);
        var caseImages = new Array();
        $("#tblCommentFileUpload").find("tbody").find("tr").each(function (index, element) {
            var newFileName = $(element).find("td").find("input[name='txtNewFileName']").val().trim();
            var oldFileName = $(element).find("td").find("label[name='lblOldFileName']").text().trim();
            var caseImage = {
                FileName: newFileName,
                OldFileName: oldFileName
            };
            caseImages.push(caseImage);
        });
        var model = {
            WorkflowHistoryLogs: workflowHistoryLogs,
            CaseImages: caseImages
        };
        $.post(ROOTPath + "/Cases/Case/CasesCommentUpdate", model, function (result) {
            if (result.Status) {
                if (parseInt($("#hdnStatusIDTarget").val()) == Statuses.Deceased) {
                    var params = {
                        StatusID: parseInt($("#hdnStatusIDTarget").val()),
                        WorkflowID: workflowID,
                        IsJsTree: false,
                        ReasonID: $("#ddlCommentReasons").val()
                    };
                    GetReasonFormID(params, function (reasons) {
                        $('#dvActivityModel').modal('hide');
                        if (reasons.length > 0) {
                            if (reasons[0].FormID) {
                                switch (parseInt(reasons[0].FormID)) {
                                    case 4:
                                    case 5:
                                        if (parseInt(reasons[0].FormID) == 4) {
                                            $("#heirInformationTitle").removeClass("hidden");
                                            $("#spouceInformationTitle").addClass("hidden");
                                            $("#hdnContactTypeID").val("1");
                                        }
                                        else {
                                            $("#heirInformationTitle").addClass("hidden");
                                            $("#spouceInformationTitle").removeClass("hidden");
                                            $("#hdnContactTypeID").val("2");
                                        }
                                        $('#dvHeirInfo').modal({ backdrop: 'static', keyboard: false });
                                        $('#dvHeirInfo').modal('show');
                                        break;
                                    case 6:
                                        $('#dvCrimeInfoModel').modal({ backdrop: 'static', keyboard: false });
                                        $('#dvCrimeInfoModel').modal('show');
                                        break;
                                }
                            }
                        }
                        else {
                            ClearCommentFormControl();
                            $('#dvActivityModel').modal('hide');
                            if (result.Message) {
                                showAlert("success", result.Message);
                            }
                            setTimeout(function () {
                                window.location.reload(true);
                            }, 2000);
                        }
                    });
                }
                else {
                    ClearCommentFormControl();
                    $('#dvActivityModel').modal('hide');
                    if (result.Message) {
                        showAlert("success", result.Message);
                    }
                    setTimeout(function () {
                        window.location.reload(true);
                    }, 2000);
                }
            }
            else {
                if (result.Message) {
                    showAlert("error", result.Message);
                }
                $('#dvActivityModel').modal('hide');
            }
        });
    });

    CaseMeetingNotesGet(caseID);

    $('#txtAddDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });

    $('#txtAddDate').datepicker('update', new Date());

    $("#btnAddHistory").click(function () {
        var isFormValid = $("#form").valid();
        if (isFormValid) {
            var data = {
                CaseID: caseID,
                MeetingTypeID: $("#ddlAddMeetingTypes").val(),
                MeetingType: $("#ddlAddMeetingTypes option:selected").text(),
                Date: $('#txtAddDate').val(),
                Notes: $("#txtAddNote").val()
            };
            $.post(ROOTPath + "/Cases/Case/AddHistoryList", data, function (response) {
                if (response.Status) {
                    if (response.Message) {
                        SetLocalStorage("ActivityMsg", response.Message);
                    }
                    $('#dvHistoryModel').modal('hide');
                    setTimeout(function () {
                        window.location.reload(true);
                    }, 500);
                }
                else {
                    if (response.Message) {
                        showAlert("error", response.Message);
                    }
                }
            });
        }
        return isFormValid;
    });

    $('#dvTabs a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

    $("#btnCancelHistory").click(function () {
        ClearMeetingFormControls();
        $("#dvHistoryModel").modal("hide");
    });

    // store the currently selected tab in the hash value
    $("ul.nav-tabs > li > a").on("shown.bs.tab", function (e) {
        var activeTab = $(e.target).attr("href");
        localStorage.setItem("activeTab", activeTab);
    });
    SetSelctedTab();
    
});

function SetSelctedTab() {
    if (localStorage.getItem("activeTab")) {
        $('#dvTabs a[href="' + localStorage.getItem("activeTab") + '"]').tab('show');
    }
}

function CaseMeetingNotesGet(caseid) {
    var customSearch = {
        CaseID: caseid
    };
    $('#tblHistory').dataTable({
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
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ajax": {
            "url": ROOTPath + "/Cases/Case/HistoryList",
            "type": "POST",
            "data": function (data) {
                data.customSearch = customSearch
            }
        },
        "columns": [
            {
                name: "B.ID", title: historyID, "data": "ID", className: "col-lg-1"
            },
            {
                name: "Date", title: date, "data": "Date", className: "col-lg-2"
            },
            {
                name: "C.Name", title: meetingType, "data": "MeetingType", className: "col-lg-1"
            },
            {
                name: "Notes", title: notes, "data": "Notes", className: "col-lg-4"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    str = "<a onclick=\"VieWHistory(" + data.ID + "," + caseID + ")\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                    str = str + "<a onclick=\"EditHistory(" + data.ID + "," + caseID + ")\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                    return str;
                },
                className: "text-right col-lg-4", sortable: false,
                title: "<button class=\"btn btn-primary btn-xs no-margin-bottom\" id=\"btnNewHistory\" name=\"btnNewHistory\">New</button>"
            }
        ],
        "order": [[0, "asc"]],
        responsive: true,
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != $(nRow).find("td").length - 1) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        }
    });
    $(".select2dropdown").select2();

}

$("#tblHistory").on('draw.dt', function () {
    $("#btnNewHistory").click(function () {
        $('#dvHistoryModel').modal({ backdrop: 'static', keyboard: false });
        $('#dvHistoryModel').modal('show');
    });
});

function ClearMeetingFormControls() {
    $("#ddlAddMeetingTypes").val($("#ddlAddMeetingTypes option:first").val());
    $("#ddlAddMeetingTypes").trigger("change");
    $('#txtAddDate').datepicker('update', new Date());
    $("#txtAddNote").val("");
}
function GetReasonFormID(params, callback) {
    var data = {
        statusid: params.StatusID,
        workflowID: params.WorkflowID,
        isJsTree: params.IsJsTree,
        reasonID: params.ReasonID
    };
    $.get(ROOTPath + "/Cases/Case/GetWorkflowStatusReason", data, function (result) {
        callback(result);
    });
}

function SaveHeirInfo() {
    $.post(ROOTPath + "/cases/case/ContactInsert", CreateHeirModel(), function (responce) {
        SaveHeirResponce(responce);
    });
}
function CreateHeirModel() {
    var contactViewModels = new Array();
    var contactViewModel = {
        CaseID: caseID,
        WorkflowID: workflowID,
        StatusID: parseInt($("#hdnStatusIDTarget").val()),
        ReasonID: parseInt($("#ddlCommentReasons").val()),
        FirstName: $("#txtHeirFirstName").val(),
        LastName: $("#txtHeirLastName").val(),
        Telephone: $("#txtHeirTelephone").val(),
        Fax: $("#txtHeirFax").val(),
        Address1: $("#txtHeirAddress1").val(),
        Address2: $("#txtHeirAddress2").val(),
        CountryID: $("#ddlHeirCountry").val(),
        CountryStateID: $("#ddlHeirState").val(),
        ZipPostalCode: $("#txtHeirZipPostalCode").val(),
        Comment: $("#txtHeirComments").val(),
        ContactTypeID: $("#hdnContactTypeID").val()
    };
    contactViewModels.push(contactViewModel);
    return {
        ContactViewModel: contactViewModels
    };
}
function SaveHeirResponce(result) {
    if (result.Status) {
        if (result.Message) {
            showAlert("success", result.Message);
        }
        ClearHeirInfoControl();
        ClearCommentFormControl();
        setTimeout(function () {
            window.location.reload(true);
        }, 2000);
    }
    else {
        if (result.Message) {
            showAlert("error", result.Message);
        }
    }
    $('#dvHeirInfo').modal('hide');
}

function SaveCrimInfo() {
    $.post(ROOTPath + "/cases/case/ContactInsert", CreateCrimModel(), function (responce) {
        SaveCrimeResponce(responce);
    });
}
function CreateCrimModel() {
    var contactViewModels = new Array();
    var contactViewModel = {
        CaseID: caseID,
        WorkflowID: workflowID,
        StatusID: parseInt($("#hdnStatusIDTarget").val()),
        ReasonID: parseInt($("#ddlCommentReasons").val()),
        FirstName: $("#txtCRIMFirstName").val(),
        LastName: $("#txtCRIMLastName").val(),
        Telephone: $("#txtCRIMTelephone").val(),
        Fax: $("#txtCRIMFax").val(),
        Address1: $("#txtCRIMAddress1").val(),
        Address2: $("#txtCRIMAddress2").val(),
        CountryID: $("#ddlCRIMCountry").val(),
        CountryStateID: $("#ddlCRIMState").val(),
        ZipPostalCode: $("#txtCRIMZipPostalCode").val(),
        Comment: $("#txtCRIMComments").val(),
        ContactTypeID: 3,
        ReferenceID: $("#txtCRIMReferenceCode").val(),
        ReferenceDate: $("#txtCRIMReferenceDate").val()
    };
    contactViewModels.push(contactViewModel);
    return {
        ContactViewModel: contactViewModels
    };
}
function SaveCrimeResponce(result) {
    if (result.Status) {
        if (result.Message) {
            showAlert("success", result.Message);
        }
        ClearCrimeInfoControl();
        ClearCommentFormControl();
        setTimeout(function () {
            window.location.reload(true);
        }, 2000);
    }
    else {
        if (result.Message) {
            showAlert("error", result.Message);
        }
    }
    $('#dvCrimeInfoModel').modal('hide');
}

