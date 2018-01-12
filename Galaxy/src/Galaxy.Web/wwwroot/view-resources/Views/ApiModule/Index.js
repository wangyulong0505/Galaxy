(function () {
    /********************弹出框效果 begin******************/
    $(function () {
        $("[data-toggle='popover']").popover({
            html: true,
            content: function () {
                return $('#popover_content_wrapper').html();
            }
        });

        $('button[data-event]').click(function () {
            var type = $(this).attr('data-event');
            switch (type) {
                case 'add':
                    //添加参数
                    addParams();
                    break;
                case 'send':
                    break;
            }
        });

        //点击行后面的删除图标，移除整行
        function deleteRow(num) {
            var i = num.parent().index();
            $('#bulk').deleteRow(i);
        }
    });

    //这个是设置textarea左边数字框的样式
    $(".textline").setTextareaCount({
        width: "30px",
        bgColor: "#EBEBEB",
        color: "#969696",
        display: "inline-block",
    });
    //切换输入参数的事件
    $("a[data-effect='change']").on('click', function () {
        $('#bulk').toggle();
        $('#keyvalue').toggle();
    });
    $("a[data-effect='changeheader']").on('click', function () {
        $('#bulkheader').toggle();
        $('#keyvalueheader').toggle();
    });
    //点击Param添加新的tr并追加到table的tbody后面
    function addParams() {
        var html = '';
        console.log($('#bulk tbody').find('tr:last').attr('class'));
        var row = $('#bulk tbody').find('tr:last').attr('class') == 'odd' ? 'even' : 'odd';
        html += '<tr role="row" class="' + row + '">';
        html += '<td class="text-left"><input placeholder="New Key" class="form-control" type="text" likeoption="true" /></td >';
        html += '<td class="text-left" name="value"><input placeholder="Value" class="form-control" type="text" likeoption="true" /></td>';
        html += '<td class="text-left" name="description" colspan="2"><input placeholder="Description" class="form-control" type="text" likeoption="true" /></td>';
        html += '<td class="text-center" style="vertical-align:middle;" onclick="deleteRow(this)"><i class="fa fa-remove"></i></td>';
        html += '</tr>';
        $('#bulk').find('tbody').append(html);
    }
    
})(jQuery);