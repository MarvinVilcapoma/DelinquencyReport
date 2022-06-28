
var DiagramConstraints = ej.datavisualization.Diagram.DiagramConstraints;

$(document).ready(function () {
    LoadDiagram();
});

var LoadDiagram = function () {
    var data = {
        ID: workflowID,
        Name: name,
        Version: version
    };
    $.post(ROOTPath + "/Workflows/Workflow/DiagramGenerator", data, function (result) {
        if (result.response.Status) {
            $("#DiagramContent").ejDiagram({
                width: "100%",
                height: "100%",
                enableContextMenu: false,
                click: onNodeClick,
                constraints: DiagramConstraints.None,
                nodes: result.viewModel.Nodes,
                connectors: result.viewModel.Connectors,
                defaultSettings: {
                    node: {
                        offsetX: 300, width: 140, height: 50, type: "flow", shape: {
                            type: "rectangle",
                            cornerRadius: 5
                        }
                    },
                    connector: {
                        targetDecorator: {
                            shape: "arrow"
                        }
                    },
                }
            });
        }
        else {
            showAlert("error", result.response.Message);
        }
    });
};


function onNodeClick(args) {
    if (args.element.type) {
        if (args.element.type.toLowerCase() === "flow") {
            if (args.element.labels !== null) {
                if (args.element.labels.length > 0) {
                    if (args.element.labels[0].isSequence) {
                        var groups = args.element.labels[0].groups.split("&");
                        if (groups.length > 0) {
                            var tempString = "";
                            for (var i in groups) {
                                tempString += "<tr>";
                                tempString += "<td>";
                                tempString += groups[i];
                                tempString += "</td>";
                                tempString += "</tr>";
                            }
                        }
                        else {
                            tempString += "<tr>";
                            tempString += "<td>";
                            tempString += noRecodsFound;
                            tempString += "</td>";
                            tempString += "</tr>";
                        }
                        $("#tblDiagramGroup").find("tbody").empty().html(tempString);
                        $("#dvDiagramGroup").modal("show");
                    }
                }
            }
        }
    }
}

var onDiagramClose = function () {
    $("#dvDiagramGroup").modal("hide");
};


