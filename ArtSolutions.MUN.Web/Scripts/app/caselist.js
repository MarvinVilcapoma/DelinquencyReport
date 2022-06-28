
var isGlobalSearch = false;
var Statuses = {
    Deceased: 9
};
$(document).ready(function () {
    // Minimize SideBar
    

    var rightPanelHeight = $(document).height() - 62,
        rightPanelWidth = $(document).width() - 64,
        max = $(document).width() - 100,
        leftwidth = 0,
        rightWidth = 0;

    leftwidth = (Math.round(rightPanelWidth * (20 / 100)));
    rightWidth = (Math.round(rightPanelWidth * (77 / 100)));

    $("#dvSplitter").ejSplitter({
        height: rightPanelHeight,
        width: rightPanelWidth,
        properties: [{ maxSize: max }, { paneSize: 80 }, { expandable: true }, { collapsible: true }],
        orientation: ej.Orientation.Horizontal,
        expandCollapse: function (args) {
            $(".e-split-divider").removeClass("e-resize");
            if (args.collapsed.index == 0) {
                var rightPanWidth = $("#dvRightPane").width();
                rightPanWidth = (rightPanWidth + leftwidth);
                $('#dvRightPane').css('width', rightPanWidth + 'px');
                $(".e-arrow-sans-right").show();
            }
            else {
                var rightPanWidth = $("#dvRightPane").width();
                rightPanWidth = (rightPanWidth - leftwidth);
                $('#dvRightPane').css('width', rightPanWidth + 'px');
                $(".e-arrow-sans-right").hide();
            }
        }
    });
    $("#dvRightPane").slimscroll({
        height: rightPanelHeight + 'px'
    });
    $("#dvLeftPane").width(leftwidth);
    $("#dvRightPane").width(rightWidth);   
    $('#dvNodeTree').jstree({
        'core': {
            'data': {
                'url': function (node) {
                    var url = "";
                    if (node.a_attr) {
                        switch (node.a_attr.NextMethodName.toLowerCase()) {
                            case "getworkflowstatusreason":
                                url = ROOTPath + '/Cases/Case/GetWorkflowStatusReason';
                                break;
                            case "mastercasesget":
                                url = ROOTPath + '/Cases/Case/MasterCasesGet';
                                break;
                        }
                    }
                    else {
                        url = ROOTPath + '/Cases/Case/GetWorkflowStatus';
                    }
                    return url;
                },
                'data': function (node) {
                    var data = null;
                    var workflowID = $('.workflowSelected').length > 0 ? $('.workflowSelected').attr("data-workflowid") : 0;
                    if (node.a_attr) {
                        switch (node.a_attr.NextMethodName.toLowerCase()) {
                            case "getworkflowstatusreason":
                                data = {
                                    "statusid": node.a_attr.CurrentID,
                                    "workflowID": workflowID,
                                    "isJsTree": true
                                };
                                break;
                            case "mastercasesget":
                                data = {
                                    "reasonid": node.a_attr.CurrentID,
                                    "statusid": node.a_attr.StatusID,
                                    "workflowID": workflowID
                                };
                                break;
                        }
                    }
                    else {
                        data = {
                            "workflowID": workflowID
                        };
                    }
                    return data;
                },
                'success': function (data, status, xhr, auto) {
                    if (data && data.length > 0) {
                        if (data == "UnAuthorized") {
                            location.href = ROOTPath + "/Account/Login";
                        }
                    }
                },
                'error': function (jqXHR, textStatus, errorThrown) {
                }

            }
        }
    }).bind("loaded.jstree", function (event, data) {
        $('#dvNodeTree').jstree('select_node', 'ul > li:first');
    });
    $('#dvNodeTree').on('refresh.jstree', function () {
        if (localStorage.getItem("lastSelectedNode")) {
            $("#dvNodeTree").jstree("select_node", $("#" + localStorage.getItem("lastSelectedNode")));
        }
    });
    $('#dvNodeTree').on('open_node.jstree', function (event, data) {
        localStorage.setItem("lastSelectedNode", data.node.id);
    });
    $('#dvNodeTree').on('select_node.jstree', function (e, data) {
        if (data.node.a_attr) {
            isGlobalSearch = false;
            $("#hdnStatusID").val(data.node.a_attr.StatusID);
            switch (data.node.a_attr.NextMethodName.toLowerCase()) {
                case "#":
                    $("#CaseID").val(data.node.a_attr.CaseID);
            }
            FillActions(parseInt(parseInt(data.node.a_attr.WorkflowID)), parseInt(data.node.a_attr.StatusID), function (responce) {
                if (responce) {
                    FillReasonsByStatusID(parseInt(data.node.a_attr.StatusID), parseInt(parseInt(data.node.a_attr.WorkflowID)), false, true, function (result) {
                        if (result) {
                            $("#ReasonID").empty();
                            $("#ReasonID").html(result);
                            if (data.node.a_attr.NextMethodName.toLowerCase() == "mastercasesget") {
                                $('#ReasonID').val(data.node.a_attr.ReasonID).trigger('change');
                            }
                            else {
                                $('#ReasonID').val("-1").trigger('change');
                            }
                            GetCases(SetCustomeSearch());
                            $("#CaseID").val("");
                        }
                    });
                }
            });
        }
    });

    $(".e-arrow-sans-right").hide();

    $("#aAdvanceSearch").click(function () {
        if ($("#dvCaseSearchView").hasClass("hidden")) {
            $("#dvCaseSearchView").fadeIn().removeClass('hidden');
        }
        else {
            $("#dvCaseSearchView").fadeIn().addClass('hidden');
        }

    });

    $("#btnSearch").click(function () {
        isGlobalSearch = true;
        GetCases(SetGlobalSearch());
    });

    $('#FilterText').keypress(function (e) {
        if (e.keyCode == 13) {
            isGlobalSearch = true;
            $("#btnSearch").click();
        }
    });

    $("#ddlWorkflow").find("li").find("a").click(function () {
        isGlobalSearch = false;
        $('.workflowSelected').removeClass('workflowSelected');
        $(this).addClass('workflowSelected');
        var selText = $(this).text();
        $(this).parents('.btn-group').find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
        GetCases(SetCustomeSearch());
        $('#dvNodeTree').jstree("refresh");
    });

    $("#aRefresh").click(function () {
        isGlobalSearch = false;
        ClearControls();
        GetCases(SetCustomeSearch());
    });

    $("#btnNew").click(function () {
        var workFlowId = $('.workflowSelected').attr("data-workflowid");
        window.location = ROOTPath + "/Cases/Case/Add/" + workFlowId;
    });

    $(".select2dropdown").select2();

    $("#btnGo").click(function () {
        isGlobalSearch = false;
        GetCases(SetCustomeSearch());
    });

    $("body").on("click", "#ddlStatusAction li a", function () {
        $("#hdnStatusIDTargetName").val($(this).text().trim());
        var formID = $(this).attr("data-formid");
        var currentStatusID = parseInt($(this).attr("data-statusid"));
        var tempArray = new Array();
        var caseID = new Array();
        var oTable = $('#tblCases').dataTable();
        var rowSelectedcollection = oTable.$("input[name='chkItems']:checked");
        rowSelectedcollection.each(function (index, elem) {
            var d = oTable.api().row($(elem).closest("tr")).data();
            tempArray.push(d);
            caseID.push(d.CaseID);
        });
        if (tempArray.length > 0) {
            if (currentStatusID > 0) {
                if (formID > 0) {
                    WorkflowFormGet(formID, function (responce) {
                        for (var i in responce) {
                            if (responce[i].ID == 3) {
                                if (responce[i].UsePopup) {
                                    DocumentWorkflowHistoryLogGet(caseID.join(), function (logs) {
                                        if (logs.status) {
                                            $("#vertical-timeline").html(GetTimeline(logs.data));
                                            FillReasonsByStatusID(currentStatusID, parseInt($('.workflowSelected').attr("data-workflowid")), false, false, function (reasons) {
                                                if (reasons) {
                                                    $("#hdnStatusIDTarget").val(currentStatusID);
                                                    $("#ddlCommentReasons").empty();
                                                    $("#ddlCommentReasons").html(reasons);
                                                    $("#ddlCommentReasons").trigger("change");
                                                    $('#dvActivityModel').modal({ backdrop: 'static', keyboard: false })
                                                    $('#dvActivityModel').modal('show');
                                                    $(".e-arrow-sans-left").css("z-index", "0");
                                                }
                                            });
                                        }
                                    });
                                }
                            }
                            else if (responce[i].ID == 1) {
                                window.location.href = ROOTPath + "/Cases/Case/PrintLetter?caseids=" + caseID.join() + "&&statuidtarget=" + currentStatusID + "&&statuidsource=" + $("#hdnStatusID").val() + "&&workflowid=" + $('.workflowSelected').attr("data-workflowid");
                            }
                        }
                    });
                }
                else {
                    var workflowHistoryLogs = new Array();
                    for (var i in tempArray) {
                        var workflowHistoryLog = {
                            CaseID: tempArray[i].CaseID,
                            StatuIdSource: $("#hdnStatusID").val(),
                            StatuIdTarget: currentStatusID,
                            WorkflowID: parseInt($('.workflowSelected').attr("data-workflowid")),
                            WorkflowVersionID: 1
                        };
                        workflowHistoryLogs.push(workflowHistoryLog);
                    }
                    var model = {
                        WorkflowHistoryLogs: workflowHistoryLogs
                    };
                    $.post(ROOTPath + "/Cases/Case/CasesStatusUpdate", model, function (result) {
                        ActionResponce(result);
                    });
                }
            }
        }
        else {
            showAlert("warning", requiredGridValidationMsg);
        }
    });

    $("#tblCases").on('draw.dt', function () {
        iCheckboxinDatatable();
    });

    $("#btnAddComments").click(function () {
        var tempArray = new Array();
        var oTable = $('#tblCases').dataTable();
        var rowSelectedcollection = oTable.$("input[name='chkItems']:checked");
        rowSelectedcollection.each(function (index, elem) {
            var d = oTable.api().row($(elem).closest("tr")).data();
            tempArray.push(d);
        });
        if (tempArray.length > 0) {
            var workflowHistoryLogs = new Array();
            for (var i in tempArray) {
                var workflowHistoryLog = {
                    CaseID: tempArray[i].CaseID,
                    StatuIdSource: parseInt($("#hdnStatusID").val()),
                    StatuIdTarget: parseInt($("#hdnStatusIDTarget").val()),
                    WorkflowID: parseInt($('.workflowSelected').attr("data-workflowid")),
                    ReasonID: $("#ddlCommentReasons").val(),
                    Comment: $("#Comments").val(),
                    AssignToID: $("#ddlCommentAssignTo").val(),
                    StatuIdTargetName: $("#hdnStatusIDTargetName").val(),
                    ReasonName: $("#ddlCommentReasons option:selected").text()
                };
                workflowHistoryLogs.push(workflowHistoryLog);
            }

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
                            WorkflowID: parseInt($('.workflowSelected').attr("data-workflowid")),
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
                                            $(".e-arrow-sans-left").css("z-index", "0");
                                            break;
                                        case 6:
                                            $('#dvCrimeInfoModel').modal({ backdrop: 'static', keyboard: false });
                                            $('#dvCrimeInfoModel').modal('show');
                                            $(".e-arrow-sans-left").css("z-index", "0");
                                            break;
                                    }
                                }
                                else {
                                    if (result.Message) {
                                        showAlert("success", result.Message);
                                    }
                                    ClearCommentFormControl();
                                    GetCases(SetCustomeSearch());
                                    $('#dvNodeTree').jstree("refresh");
                                    $(".e-arrow-sans-left").css("z-index", "0");
                                    $('#dvActivityModel').modal('hide');
                                }
                            }
                        });
                    }
                    else {
                        if (result.Message) {
                            showAlert("success", result.Message);
                        }
                        ClearCommentFormControl();
                        GetCases(SetCustomeSearch());
                        $('#dvNodeTree').jstree("refresh");
                        $(".e-arrow-sans-left").css("z-index", "0");
                        $('#dvActivityModel').modal('hide');
                    }
                }
                else {
                    if (result.Message) {
                        showAlert("error", result.Message);
                    }
                    $(".e-arrow-sans-left").css("z-index", "0");
                    $('#dvActivityModel').modal('hide');
                }
            });
        }
    });

    $(".e-split-divider").removeClass("e-resize");

    $(window).unload(function () {
        localStorage.removeItem("lastSelectedNode");
    });

    $("#CaseID").ForceNumericOnly();
});

function GetCases(customSearch) {
    $('#tblCases').dataTable({
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
        "pageLength": pageSize,
        "ajax": {
            "url": ROOTPath + "/Case/List",
            "type": "POST",
            "data": function (data) {
                data.customSearch = customSearch;
            }
        },
        destroy: true,
        "columns": [
            {
                title: "<input type=\"checkbox\" class=\"i-checks\" name=\"chkHeader\" />",
                sortable: false, width: "5%", "data": function (data, type) {
                    return "<input type=\"checkbox\" class=\"i-checks\" name=\"chkItems\" data-caseid=\"" + data.CaseID + "\" />"
                }
            },
            {
                name: "CaseID", title: caseID, "data": "CaseID", width: "8%"
            },
            {
                name: "KeyCode", title: key, "data": "KeyCode", width: "8%"
            },
            {
                name: "MuicipalityName", title: muicipality, "data": "MuicipalityName", width: "15%"
            },
            {
                name: "BusinessName", title: business, "data": "BusinessName", width: "10%"
            },
            {
                name: "ServiceType", title: serviceType, "data": "ServiceType", width: "8%"
            },
            {
                name: "PriorityName", title: priority, "data": "PriorityName", width: "6%"
            },
            {
                name: "Weight", title: weight, "data": "Weight", width: "6%"
            },
            {
                name: "AssignedTo", title: assignedTo, "data": "AssignedTo", width: "10%"
            },
            {
                name: "Balance", title: balance, "data": function (data, type) {
                    return data.BalanceWithCurrency;
                }, className: "text-right", width: "10%"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    var workflowID = parseInt($('.workflowSelected').attr("data-workflowid").trim());
                    var url = ROOTPath + "/Cases/Case/{0}?id=" + workflowID + "&&caseid=" + data.CaseID;
                    str = "<a href=\"" + url.format("View") + "\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                    str = str + "<a href=\"" + url.format("Edit") + "\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                    return str;
                },
                className: "text-right", sortable: false, title: actions, width: "14%"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != 10) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        }
    });
}

function iCheckboxinDatatable() {
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        increaseArea: '5%'
    });
    $("input[name='chkHeader']").on('ifClicked', function (event) {
        if ($(this)["0"].checked) { // checkbox unchecked
            $("input[name='chkItems']").iCheck('uncheck');
        }
        else {  // checkbox checked
            $("input[name='chkItems']").iCheck('check');
        }
        $("input[name='chkItems']").iCheck('update');
    });
    $("input[name='chkItems']").on('ifClicked', function (event) {
        if ($(this)["0"].checked) { // checkbox unchecked
            $("input[name='chkHeader']").iCheck('uncheck');
        }
        else { // checkbox checked
            if ((parseInt($("input[name='chkItems']").filter(':checked').length) + 1) == $("input[name='chkItems']").length) {
                $("input[name='chkHeader']").iCheck('check');
            }
            else {
                $("input[name='chkHeader']").iCheck('uncheck');
            }
        }
        $("input[name='chkHeader']").iCheck('update');
    });
    if (isGlobalSearch) {
        $("button[name='btnAction']").prop("disabled", true);
    }
    else {
        $("button[name='btnAction']").prop("disabled", false);
    }
    $("input[name='chkHeader']").iCheck('uncheck');
    $("input[name='chkHeader']").iCheck('update');
    $("#tblCases").removeClass("dataTable");
}

function SetCustomeSearch() {
    var customSearch = {
        "CaseID": $("#CaseID").val(),
        "KeyCode": $("#KeyCode").val(),
        "CompanyID": $("#CompanyID").val(),
        "BusinessName": $("#BusinessName").val(),
        "PriorityID": $("#PriorityID").val(),
        "Weight": $("#Weight").val(),
        "AssignToID": $("#AssignToID").val(),
        "ReasonID": $("#ReasonID").val(),
        "FilterText": null,
        "StatusID": $("#hdnStatusID").val(),
        WorkflowID: parseInt($('.workflowSelected').attr("data-workflowid"))
    };
    return customSearch;
}

function SetGlobalSearch() {
    var customSearch = {
        "CaseID": 0,
        "KeyCode": "",
        "CompanyID": 0,
        "BusinessName": "",
        "PriorityID": -1,
        "Weight": -1,
        "AssignToID": "00000000-0000-0000-0000-000000000000",
        "ReasonID": -1,
        "FilterText": $("#FilterText").val(),
        "StatusID": -1
    };
    return customSearch;
}

function ClearControls() {
    $("#CaseID").val("");
    $("#KeyCode").val("");
    $("#BusinessName").val("");
    $("#PriorityID").val("-1").trigger('change');
    $("#Weight").val("-1").trigger('change');
    $("#AssignToID").val("00000000-0000-0000-0000-000000000000").trigger('change');
    $("#ReasonID").val("-1").trigger('change');
    $("#FilterText").val("");
}

function FillActions(workflowID, statusID, callback) {
    var data = {
        workflowID: workflowID,
        statusID: statusID
    };
    $.get(ROOTPath + "/Cases/Case/StatusActivityGet", data, function (responce) {
        var tempString = "";
        if (responce.length > 0) {
            for (var i in responce) {
                tempString += "<li><a data-statusid=\"" + responce[i].ID + "\" data-formid=\"" + (responce[i].FormID ? responce[i].FormID : 0) + "\">" + responce[i].Name + "</a></li>";
            }
        }
        else {
            tempString = "<li><a href=\"#\" data-statusid=\"-1\" data-formid=\"-1\">" + noActionFound + "</a></li>";
        }
        $("#ddlStatusAction").html(tempString);
        callback(tempString);
    });
}

function ActionResponce(result) {
    if (result.Status) {
        if (result.Message) {
            showAlert("success", result.Message);
        }
        ClearCommentFormControl();
        GetCases(SetCustomeSearch());
        $('#dvNodeTree').jstree("refresh");
    }
    else {
        if (result.Message) {
            showAlert("error", result.Message);
        }
    }
    $(".e-arrow-sans-left").css("z-index", "0");
    $('#dvActivityModel').modal('hide');
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
function SaveCrimInfo() {
    $.post(ROOTPath + "/cases/case/ContactInsert", CreateCrimModel(), function (responce) {
        SaveCrimeResponce(responce);
    });
}
function CreateCrimModel() {
    var oTable = $('#tblCases').dataTable();
    var rowSelectedcollection = oTable.$("input[name='chkItems']:checked");
    var contactViewModels = new Array();
    rowSelectedcollection.each(function (index, elem) {
        var d = oTable.api().row($(elem).closest("tr")).data();
        var contactViewModel = {
            CaseID: d.CaseID,
            WorkflowID: parseInt($(".workflowSelected").attr("data-workflowid")),
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
    });
    return {
        ContactViewModel: contactViewModels
    };
}
function SaveHeirInfo() {
    $.post(ROOTPath + "/cases/case/ContactInsert", CreateHeirModel(), function (responce) {
        SaveHeirResponce(responce);
    });
}
function CreateHeirModel() {
    var oTable = $('#tblCases').dataTable();
    var rowSelectedcollection = oTable.$("input[name='chkItems']:checked");
    var contactViewModels = new Array();
    rowSelectedcollection.each(function (index, elem) {
        var d = oTable.api().row($(elem).closest("tr")).data();
        var contactViewModel = {
            CaseID: d.CaseID,
            WorkflowID: parseInt($(".workflowSelected").attr("data-workflowid")),
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
    });
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
        GetCases(SetCustomeSearch());
        $('#dvNodeTree').jstree("refresh");
    }
    else {
        if (result.Message) {
            showAlert("error", result.Message);
        }
    }
    $(".e-arrow-sans-left").css("z-index", "0");
    $('#dvCrimeInfoModel').modal('hide');
}
function SaveHeirResponce(result) {
    if (result.Status) {
        if (result.Message) {
            showAlert("success", result.Message);
        }
        ClearHeirInfoControl();
        ClearCommentFormControl();
        GetCases(SetCustomeSearch());
        $('#dvNodeTree').jstree("refresh");
    }
    else {
        if (result.Message) {
            showAlert("error", result.Message);
        }
    }
    $(".e-arrow-sans-left").css("z-index", "0");
    $('#dvHeirInfo').modal('hide');
}