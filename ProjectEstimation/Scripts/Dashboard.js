// Auther   : Sivabalan S
// Date     : 24July'19
// Purpose  : Dashboard screen control activity
// !---------------------------------------------------------------------!

$(document).ready(function () {
    LoadALLProject();
    BindChart();
    var id = [
        "sqlCounts",
        "sqlHours",
        "uiCounts",
        "uiHours",
        "backendCounts",
        "backendHours",
        "unittestCounts",
        "unittestHours",
        "technicalTestCounts",
        "technicalTestHours",
        "bugFixCounts",
        "bugFixHours"
    ];

    function AJAX(url, method, formData, returnMethod, isReset) {
        $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(formData),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function () {
                
            },
            complete: function (result) {

                if (returnMethod == "LoadProject") {
                    var tblContent = "";
                    $.each(result.responseJSON, function (index, items) {
                        var activity = JSON.parse(items.devActivityHours);
                        var hours = 0;
                        $.each(activity, function (key, value) {
                            if (key.indexOf("Hours") != -1) {
                                hours += parseInt(value);
                            }
                        });
                        tblContent += "<tr>";
                        tblContent += "<td>" + ++index + "</td>";
                        tblContent += "<td>" + items.projectName + "</td>";
                        tblContent += "<td>" + hours + "</td>";
                        tblContent += "<td><button data-toggle='modal' data-target='#addProjectModal' data-content=" + items.devActivityHours + " class='btn btn-xs btn-warning editProject' data-editProject=" + items.sno + ">Edit</button>&nbsp&nbsp<button class='btn btn-xs btn-danger deleteProject' data-ProjectID='" + items.sno + "'>Delete</button></td>";
                        tblContent += "</tr>";
                    });
                    $("#projectDetails tbody").html(tblContent);
                } else if (returnMethod == "alert") {
                    alert(result.responseText);
                    if (isReset) {
                        $("#reset").trigger("click");
                        $('#addProjectModal').modal('toggle');
                        $('.modal-backdrop').remove();
                        LoadALLProject();
                    }
                } else if (returnMethod == "estimateHours") {
                    BindChart(result.responseJSON[1], result.responseJSON[0]);
                } else if (returnMethod == "editProject") {
                    appendValueToModal(JSON.parse(result.responseJSON.devActivityHours));
                }
            },
            error: function () {

            }
        });
    }

    function appendValueToModal(projectDetails){
        $("#projectName").val(projectDetails.ProjectName);
        $("#sqlCounts").val(projectDetails.SQLCount);
        $("#sqlHours").val(projectDetails.SQLHours);
        $("#uiCounts").val(projectDetails.UICount);
        $("#uiHours").val(projectDetails.UIHours);
        $("#backendCounts").val(projectDetails.ControllerCount);
        $("#backendHours").val(projectDetails.ControllerHours);

        $("#unittestCounts").val(projectDetails.UnitTestCount);
        $("#unittestHours").val(projectDetails.UnitTestHours);
        $("#technicalTestCounts").val(projectDetails.TechnicalTestCount);
        $("#technicalTestHours").val(projectDetails.TechnicalTestHours);
        $("#bugFixCounts").val(projectDetails.BugCounts);
        $("#bugFixHours").val(projectDetails.BugHours);
    }

    function AddNewProject() {
        var formValue = {};
        var validForm = true;
        $.each(id, function (index, ids) {
            var value = $("#" + ids).val();
            var name = $("#" + ids).data("name");
            if (value == "") {
                alert("Mandatory to enter " + name + " value");
                validForm = false;
                return false;
            }
        });
        if (validForm) {
            var projectName = $("#projectName").val();
            if (projectName == "") {
                alert("Please Enter Project Name")
            } else {
                var projectDetails = {
                    ProjectName: projectName,
                    SQLCount: $("#sqlCounts").val(),
                    SQLHours: $("#sqlHours").val(),
                    UICount: $("#uiCounts").val(),
                    UIHours: $("#uiHours").val(),
                    ControllerCount: $("#backendCounts").val(),
                    ControllerHours: $("#backendHours").val(),
                    UnitTestCount: $("#unittestCounts").val(),
                    UnitTestHours: $("#unittestHours").val(),
                    TechnicalTestCount: $("#technicalTestCounts").val(),
                    TechnicalTestHours: $("#technicalTestHours").val(),
                    BugCounts: $("#bugFixCounts").val(),
                    BugHours: $("#bugFixHours").val(),
                }
                AJAX("Dashboard/AddProject?sno=" + parseInt($("#projectID").val()), "POST", projectDetails, "alert", true);
            }
        }
    }

    function EstimateProject() {
        var sqlTables = $("#SqlTables").val();
        var uiScreens = $("#UIScreens").val();
        var startDate = $("#startData").val();
        var developers = $("#developers").val();
        if (sqlTables == "") {
            alert("Please Enter no of SQL Tables Required");
        } else if (uiScreens == "") {
            alert("Please Enter no of UI Screens Required");
        } else if (developers == "") {
            alert("Please Enter no of Developers")
        } else if (startDate == "") {
            alert("Please Enter Project Start Date");
        }
        else {
            var details = {
                sqlTable: sqlTables,
                uiScreens: uiScreens,
                backendCode: $("#chkBackendCoding").is(":checked"),
                unitTesting: $("#chkUnitTesting").is(":checked"),
                technicalTesting: $("#chkTechnicalTesting").is(":checked"),
                bugFixing: $("#chkBugFixing").is(":checked"),
                startDate: startDate,
                developers: developers
            }
            AJAX("Dashboard/EstimateProject", "POST", details, "estimateHours");
        }
    }

    var buttonIDs = "#addProject, #estimatehours";
    $(buttonIDs).click(function () {
        switch ($(this).attr('id')) {
            case "addProject":
                AddNewProject();
                break;
            case "estimatehours":
                EstimateProject();
                break;
        }
    });

    $(".deleteProject").click(function () {
        var projectID = $(this).attr("ProjectID").val();
        AJAX("Dashboard/DeleteProject", "POST", "{}", "alert");
    });

    $('#addProjectModal').on('shown.bs.modal', function (e) {
        $("#projectID").val(parseInt($(e.relatedTarget).attr("data-editproject")));
        AJAX("Dashboard/EditProject?sno=" + parseInt($(e.relatedTarget).attr("data-editproject")), "GET", "{}", "editProject");
    })

    function LoadALLProject() {
        AJAX("Dashboard/LoadProject", "GET", "{}", "LoadProject");
    }


    function BindChart(dataValue, durationChart) {
        require.config({
            paths: {
                echarts: '../assets/js/plugins/visualization/echarts'
            }
        });
        require(
        [
            'echarts',
            'echarts/theme/limitless',
            'echarts/chart/pie',
            'echarts/chart/funnel',
            'echarts/chart/line',
            'echarts/chart/bar',

            'echarts/chart/scatter',
            'echarts/chart/k',
            'echarts/chart/radar',
            'echarts/chart/gauge'
        ],


        // Charts setup
        function (ec, limitless) {
            var basic_pie = ec.init(document.getElementById('basic_pie'), limitless);
            var line_bar = ec.init(document.getElementById('line_bar'), limitless);
            basic_pie_options = {
                title: {
                    text: 'Project Estimation',
                    subtext: 'Based on the Team Productivity',
                    x: 'center'
                },

                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b}: {c} ({d}%)"
                },

                legend: {
                    orient: 'vertical',
                    x: 'left',
                    data: ['Database Design', 'UI Development', 'Unit Testing', 'Techinical Testing', 'Bug Fixing']
                },

                toolbox: {
                    show: true,
                    orient: 'vertical',
                    feature: {
                        mark: {
                            show: true,
                            title: {
                                mark: 'Markline switch',
                                markUndo: 'Undo markline',
                                markClear: 'Clear markline'
                            }
                        },
                        dataView: {
                            show: true,
                            readOnly: false,
                            title: 'View data',
                            lang: ['View chart data', 'Close', 'Update']
                        },
                        magicType: {
                            show: true,
                            title: {
                                pie: 'Switch to pies',
                                funnel: 'Switch to funnel',
                            },
                            type: ['pie', 'funnel'],
                            option: {
                                funnel: {
                                    x: '25%',
                                    y: '20%',
                                    width: '40%',
                                    height: '60%',
                                    funnelAlign: 'left',
                                    max: 1548
                                }
                            }
                        },
                        restore: {
                            show: true,
                            title: 'Restore'
                        },
                        saveAsImage: {
                            show: true,
                            title: 'Same as image',
                            lang: ['Save']
                        }
                    }
                },

                // Enable drag recalculate
                calculable: true,

                // Add series
                series: [{
                    name: 'Development Activity',
                    type: 'pie',
                    radius: '70%',
                    center: ['50%', '57.5%'],
                    data: [
                        { value: 335, name: 'Database Design' },
                        { value: 310, name: 'UI Development' },
                        { value: 234, name: 'Unit Testing' },
                        { value: 135, name: 'Techinical Testing' },
                        { value: 1548, name: 'Bug Fixing' }
                    ],
                    //data: dataValue
                }]
            };

            line_bar_options = {

                // Setup grid
                grid: {
                    x: 55,
                    x2: 45,
                    y: 35,
                    y2: 25
                },

                // Add tooltip
                tooltip: {
                    trigger: 'axis'
                },

                // Enable drag recalculate
                calculable: true,

                // Add legend
                legend: {
                    data: ['SQL', 'UI', 'Backend Development', 'Unit Testing', 'Technical Testing', 'Bug Fixing']
                },

                // Horizontal axis
                xAxis: [{
                    type: 'category',
                    //data: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
                    data: ['12Aug19', '13Aug19', '14Aug19', '15Aug19', '16Aug19', '17Aug19']
                }],

                // Vertical axis
                yAxis: [
                    {
                        type: 'value',
                        name: 'Hours',
                        axisLabel: {
                            formatter: '{value} Hour'
                        }
                    },
                    {
                        //type: 'value',
                        //name: 'Temp',
                        //axisLabel: {
                        //    formatter: '{value} °C'
                        //}
                    }
                ],

                // Add series
                series: [
                    {
                        name: 'Evaporation',
                        type: 'bar',
                        data: [2.0, 4.9, 7.0, 23.2, 25.6, 76.7, 135.6, 162.2, 32.6, 20.0, 6.4, 3.3]
                    },
                    {
                        name: 'SQL',
                        type: 'bar',
                        data: [2.6, 5.9, 9.0, 26.4, 28.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3]
                    },
                    {
                        name: 'UI',
                        type: 'line',
                        yAxisIndex: 1,
                        data: [2.0, 2.2, 3.3, 4.5, 6.3, 10.2, 20.3, 23.4, 23.0, 16.5, 12.0, 6.2]
                    },
                    {
                        name: 'Backend Development',
                        type: 'bar',
                        data: [2.6, 5.9, 9.0, 26.4, 28.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3]
                    },
                    {
                        name: 'Unit Testing',
                        type: 'line',
                        yAxisIndex: 1,
                        data: [2.0, 2.2, 3.3, 4.5, 6.3, 10.2, 20.3, 23.4, 23.0, 16.5, 12.0, 6.2]
                    },
                    {
                        name: 'Technical Testing',
                        type: 'bar',
                        data: [2.6, 5.9, 9.0, 26.4, 28.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3]
                    },
                    {
                        name: 'Bug Fixing',
                        type: 'line',
                        yAxisIndex: 1,
                        data: [2.0, 2.2, 3.3, 4.5, 6.3, 10.2, 20.3, 23.4, 23.0, 16.5, 12.0, 6.2]
                    }
                ]
                //series: durationChart
            };

            basic_pie.setOption(basic_pie_options);
            line_bar.setOption(line_bar_options);

            window.onresize = function () {
                setTimeout(function () {
                    basic_pie.resize();
                    line_bar.resize();
                }, 200);
            }
        });
    }
});