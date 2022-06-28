$(document).ready(function () {
    WorkflowGroupUsersGet();
});

function WorkflowGroupUsersGet() {
    var model = {
        WorkflowID: $("#hdnWorkflowID").val(),
        WorkflowVersionID: $("#hdnWorkflowVersionID").val()
    };
    $('#tblUsers').dataTable({
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
            "url": ROOTPath + "/Workflows/WorkflowGroup/UserGet",
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
                name: "DisplayName", title: displayName, "data": "DisplayName", className: "col-lg-3"
            },
            {
                name: "Group", title: group, "data": function (data, type) {
                    var tempString = "";
                    if (data.Groups != null) {
                        var groups = data.Groups.split(",");
                        for (var i in groups) {
                            tempString += "<span class=\"tag label label-primary m-r-xss\">" + groups[i].split("^")[1] + "</span>";
                        }
                    }
                    return tempString;
                }, className: "col-lg-9", sortable: false
            }
        ],
        "order": [[0, "asc"]],
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