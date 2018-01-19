(function () {
    $(function () {
        $('button[data-btn-type]').click(function () {
            //button event
            var action = $(this).attr('data-btn-type');
            switch (action) {
                case 'add':
                    window.location.href = appPath + "Document/Edit";
                    break;
                case 'search':
                    //
                    break;
                case 'reset':
                    $('#searchDiv input[name="title"]').val('');
                    $('#searchDiv input[name="keywords"]').val('');
                    break;
            }
        });
    });
    /**
     * 编辑
     * @param {any} id
     */
    function editMD(id) {
        window.location.href = appPath + "Document/Edit/" + id;
    }
    /**
     * 删除
     * @param {any} id
     */
    function deleteMD(id){
        modals.confirm("是否要删除该行数据？", function () {
            $.ajax({
                url: appPath + 'Document/Delete/' + id,
                data: null,
                type: 'POST',
                dataType: 'HTML',
                success: function (data, textStatus) {
                    if (data) {
                        modals.info("删除成功");
                        window.location.href = appPath + 'Document/Index?pageIndex=' + $('#pageIndex').val() + '&pageSzie=' + $('#pageSize').val() + '&strKey=' + $('#key').val();
                    }
                },
                error: function (XMLHttpRequest, textStatus, erronThrown) {
                    modals.error(erronThrown);
                }
            })
        });
    }
    /**
     * 预览
     * @param {any} id
     */
    function previewMD(id, path) {
        window.location.href = "../Documnt/Preview/" + id;
    }
})()