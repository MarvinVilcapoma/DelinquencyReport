$(document).ready(function () {
    Chart.defaults.global.legend.display = false;
    var barOption = {
        responsive: true
    };
    //Cases Money by Status
    var caseBalance = JSON.parse($("#hdnCaseBalance").val());
    var balanceLabels = new Array();
    var balanceData = new Array();
    for (var i in caseBalance) {
        balanceLabels.push(caseBalance[i].StatusName);
        balanceData.push(caseBalance[i].Balance);
    }
    var caseBalanceDataSet = {
        labels: balanceLabels,
        datasets: [
            {
                //label: "Data 1",
                backgroundColor: 'rgba(220, 220, 220, 0.5)',
                pointBorderColor: "#fff",
                data: balanceData
            }
        ]
    };
    new Chart(document.getElementById("cnvCaseBalance").getContext("2d"),
        {
            type: 'bar',
            data: caseBalanceDataSet,
            options: barOption
        });

    //Cases Quantity by Status
    var caseCount = JSON.parse($("#hdnCaseCount").val());
    var countLabels = new Array();
    var countData = new Array();
    countData.push(0);
    for (var i in caseCount) {
        countLabels.push(caseCount[i].StatusName);
        countData.push(caseCount[i].Balance);
    }
    var caseCountDataSet = {
        labels: countLabels,
        datasets: [
            {
                //label: "Data 1",
                backgroundColor: 'rgba(220, 220, 220, 0.5)',
                pointBorderColor: "#fff",
                data: countData
            }
        ]
    };
    new Chart(document.getElementById("cnvCaseCount").getContext("2d"),
        {
            type: 'bar',
            data: caseCountDataSet,
            options: barOption
        });
});

