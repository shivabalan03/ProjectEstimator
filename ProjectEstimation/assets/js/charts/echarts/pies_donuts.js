/* ------------------------------------------------------------------------------
 *
 *  # Echarts - pies and donuts
 *
 *  Pies and donuts chart configurations
 *
 *  Version: 1.0
 *  Latest update: August 1, 2015
 *
 * ---------------------------------------------------------------------------- */

$(function () {

    // Set paths
    // ------------------------------

    require.config({
        paths: {
            echarts: '../assets/js/plugins/visualization/echarts'
        }
    });


    // Configuration
    // ------------------------------

    require(
        [
            'echarts',
            'echarts/theme/limitless',
            'echarts/chart/pie',
            'echarts/chart/funnel'
        ],


        // Charts setup
        function (ec, limitless) {


            // Initialize charts
            // ------------------------------

            var basic_pie = ec.init(document.getElementById('basic_pie'), limitless);


            // Charts setup
            // ------------------------------                    

            //
            // Basic pie options
            //

            basic_pie_options = {

                // Add title
                title: {
                    text: 'Project Estimation',
                    subtext: 'Based on the Team Productivity',
                    x: 'center'
                },

                // Add tooltip
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b}: {c} ({d}%)"
                },

                // Add legend
                legend: {
                    orient: 'vertical',
                    x: 'left',
                    data: ['Database Design', 'UI Development', 'Unit Testing', 'Techinical Testing', 'Bug Fixing']
                },

                // Display toolbox
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
                    name: 'Browsers',
                    type: 'pie',
                    radius: '70%',
                    center: ['50%', '57.5%'],
                    data: [
                        {value: 335, name: 'Database Design'},
                        {value: 310, name: 'UI Development'},
                        {value: 234, name: 'Unit Testing'},
                        {value: 135, name: 'Techinical Testing'},
                        {value: 1548, name: 'Bug Fixing'}
                    ]
                }]
            };


            
            //Important
            //basic_pie.setOption(basic_pie_options);

            // Resize charts
            // ------------------------------

            window.onresize = function () {
                setTimeout(function (){
                    basic_pie.resize();
                    //basic_donut.resize();
                    //nested_pie.resize();
                    //infographic_donut.resize();
                    //rose_diagram_hidden.resize();
                    //rose_diagram_visible.resize();
                    //lasagna_donut.resize();
                    //pie_timeline.resize();
                    //multiple_donuts.resize();
                }, 200);
            }
        }
    );
});
