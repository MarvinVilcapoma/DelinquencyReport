$(document).ready(function () {
    GetPrintLetter(SetSearchModel());

    $("#btnrefresh").click(function () {
        ClearControls();
        GetPrintLetter(SetSearchModel());
    });

    $("#btnSearch").click(function () {
        GetPrintLetter(SetSearchModel());
    });

    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            $("#btnSearch").click();
        }
    });
});

function SetSearchModel() {
    return {
        FilterText: $("#txtSearch").val()
    };
}

function ClearControls() {
    $("#txtSearch").val('');
}

function GetPrintLetter(model) {
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
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "pageLength": pageSize,
        "ajax": {
            "url": ROOTPath + "/case/PrintLetterList",
            "type": "POST",
            "data": function (data) {
                data.model = model;
            }
        },
        destroy: true,
        "columns": [
            {
                name: "KeyCode", title: keycode, "data": "KeyCode", width: "10%"
            },
            {
                name: "C.Name", title: template, "data": "TemplateName", width: "15%"
            },
            {
                name: "OutputFormat", title: format, "data": "Format", width: "10%"
            },
            {
                name: "UserName", title: user, "data": "UserName", width: "10%", "orderable": false
            },
            {
                name: "Date", title: date, "data": "PrintDate", width: "10%"
            },
            {
                name: "CaseCount", title: cases, "data": "CaseCount", width: "8%"
            },
            {
                name: "Balance", title: estimateAmount, "data": "EstimateAmount", className: "text-right", width: "17%"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    str = "<a href=\"" + ROOTPath + "/cases/case/PrintLetterDownload?id=" + data.FileID + "&&actionid=2" + "\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-download\"></i> " + download + " </a>";
                    str = str + "<a href=\"" + ROOTPath + "/cases/case/PrintLetterDownload?id=" + data.FileID + "&&actionid=1" + "\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-print\"></i> " + print + " </a>";
                    return str;
                },
                className: "text-right", sortable: false, title: actions, width: "20%"
            }
        ],
        "order": [[0, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != 6) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        }
    });
}
