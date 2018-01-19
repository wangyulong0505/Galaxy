(function () {
    $(function () {
        window.location.href= $('#basePath').val() + "home/index";
        $('button[data-btn-type]').click(function () {
            //button event
            var action = $(this).attr('data-btn-type');
            switch (action) {
                case 'add':
                    window.location.href = "../Document/Edit";
                    break;
                case 'search':
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
        window.location.href = "../Document/Edit/" + id;
    }
    /**
     * 删除
     * @param {any} id
     */
    function deleteMD(id){
        modals.confirm("是否要删除该行数据？", function () {
            $.ajax({
                url: '../Document/Delete/' + id,
                data: null,
                type: 'POST',
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (data) {
                        modals.info("删除成功");
                        window.location.href = '../Document/Index?pa';
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