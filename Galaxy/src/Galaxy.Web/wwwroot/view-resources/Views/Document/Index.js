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
        $('a[data-btn-type]').click(function () {
            //a event
            var action = $(this).attr('data-btn-type');
            switch (action) {
                case 'delete':
                    var id = $(this).parents('tr').attr("id");
                    deleteMD(id);
                    break;
                case 'preview':
                    var id = $(this).parents('tr').attr("id");
                    previewMD(id);
                    break;
            }
        })
        $('#markdown_page').change(function () {
            var vs = this.value;
            console.log(this.value);
            window.location.href = appPath + 'Documet/Index?pageIndex=1&pageSize=' + vs + '&strKey=' + $('#key').val();
        })
    });
    /**
     * 删除
     * @param {any} id
     */
    function deleteMD(id) {
        modals.confirm("是否要删除该行数据？", function () {
            $.ajax({
                url: appPath + 'Document/Delete/' + id,
                data: null,
                type: 'POST',
                dataType: 'HTML',
                success: function (data, textStatus) {
                    if (data) {
                        modals.info("删除成功");
                        window.location.href = appPath + 'Document/Index?pageIndex=' + $('#pageIndex').val() + '&pageSize=' + $('#pageSize').val() + '&strKey=' + $('#key').val();
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
    function previewMD(id) {
        window.location.href = appPath + "../Document/Preview/" + id;
    }
})()