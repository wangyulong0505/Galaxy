(function () {
    $(function () {
        //daterangepicker 插件汉化
        var locale = {
            "format": 'YYYY-MM-DD',
            "separator": " -222 ",
            "applyLabel": "确定",
            "cancelLabel": "取消",
            "fromLabel": "起始时间",
            "toLabel": "结束时间'",
            "customRangeLabel": "自定义",
            "weekLabel": "W",
            "daysOfWeek": ["日", "一", "二", "三", "四", "五", "六"],
            "monthNames": ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            "firstDay": 1
        };
        //初始化显示当前时间
        $('#daterange-btn span').html(moment().subtract('hours', 1).format('YYYY-MM-DD') + ' - ' + moment().format('YYYY-MM-DD'));
        $('#daterange-btn').daterangepicker(
            {
                'locale': locale,
                ranges: {
                    '今日': [moment(), moment()],
                    '昨日': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    '最近7日': [moment().subtract(6, 'days'), moment()],
                    '最近30日': [moment().subtract(29, 'days'), moment()],
                    '本月': [moment().startOf('month'), moment().endOf('month')],
                    '上月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                },
                //通过控制startDate和endDate的值控制初始化
                //startDate: moment().subtract(29, 'days'),
                startDate: moment(),
                endDate: moment()
            },
            function (start, end) {
                $('#daterange-btn span').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
            }
        );
        $('.sparkbar').each(function () {
            var $this = $(this);
            $this.sparkline('html', {
                type: 'bar',
                height: $this.data('height') ? $this.data('height') : '30',
                barColor: $this.data('color')
            });
        });
        var options = {
            map: 'cn_mill',
            backgroundColor: 'transparent',
            regionStyle: {
                initial: {
                    fill: 'rgba(210, 214, 222, 1)',
                    'fill-opacity': 1,
                    stroke: 'none',
                    'stroke-width': 0,
                    'stroke-opacity': 1
                },
                hover: {
                    'fill-opacity': 0.7,
                    cursor: 'pointer'
                },
                selected: {
                    fill: 'yellow'
                },
                selectedHover: {}
            },
            series: {
                markers: [{
                    attribute: 'fill',
                    scale: ['#C8EEFF', '#0071A4'],
                    normalizeFunction: 'polynomial',
                    values: [408, 512, 550, 781],
                    legend: {
                        vertical: true
                    }
                }],
                regions: [
                    {
                        scale: ['#ebf4f9', '#92c1dc'],
                        normalizeFunction: 'polynomial'
                    }
                ]
            },
            onRegionLabelShow: function (e, el, code) {
                var html = '';
                html += '<div style="width:120px;">';
                html += '<div style="border-bottom:1px solid;padding-bottom:5px;">' + el.html() + '</div>';
                html += '<div style="margin-top:5px;"><i class="fa fa-circle text-success margin-r-5"></i>浏览量(PV)<span class="pull-right">';
                if (typeof visitorsData[code] != 'undefined') {
                    html += visitorsData[code];
                } else {
                    html += 0;
                }
                html += '</div>';
                html += '<div style="margin-top:5px;"><i class="fa fa-circle text-primary margin-r-5"></i>占比<span class="pull-right">';
                if (typeof pecentData[code] != 'undefined') {
                    html += pecentData[code];
                } else {
                    html += 0;
                }
                html += ' %</div>';
                el.html(html);
            }
        }

        $('#map-markers').vectorMap(options);
    });
})();