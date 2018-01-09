(function () {
    //tableId,queryId,conditionContainer
    var markdownTable;
	var selectId="0";

    $(function () {
        //表格中有按钮，不进行行选中
        var md_config = {
            singleSelect: null
        };
        //init table and fill data
        markdownTable = new CommonTable("markdown_table", "markdown_list", "searchDiv", md_config);

        $('button[data-btn-type]').click(function () {
            //button event
            var action = $(this).attr('data-btn-type');
            switch (action) {
                case 'add':
                    window.loadPage(basePath + "/Document/Edit");
                    break;
            }
        });
        if (selectId != "0") {
            setTimeout(function () { markdownTable.selectRow(selectId); }, 100);
        }
    });

    function fnRenderOperator(value,type,rowObj,oSetting){
		var oper="<a href='#' onclick='previewMD(\""+rowObj.id+"\")'><i class='fa'>浏览</i></a>";
        oper+="&nbsp;&nbsp;";
        oper+="<a href='#' onclick='editMD(\""+rowObj.id+"\")'><i class='fa'>编辑</i></a>";
		oper+="&nbsp;&nbsp;";
		oper+="<a href='#' onclick='deleteMD(\""+rowObj.id+"\")'><i class='fa'>删除</i></a>";
		return oper;
	}

    function editMD(id){
        window.loadPage("/Document/Edit/" + id);
    }


    function deleteMD(id){
        modals.confirm("是否要删除该行数据？", function () {
            ajaxPost(basePath + "/markdown/delete/" + id, null, function (data) {
                if (data.success) {
                    markdownTable.reloadRowData();
                } else {
                    modals.error("用户数据被引用，不可删除！");
                }
            });
        });
    }

    function previewMD(id){
        window.loadPage(basePath + "/markdown/preview?id=" + id);
    }
})()