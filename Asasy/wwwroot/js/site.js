// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function DataTableClientFun(controller, action, column) {

    debugger;

    var columns = [];

    // fields
    columns.push({ "data": column[1], "name": column[1], "autoWidth": true });
    columns.push({ "data": column[2], "name": column[2], "autoWidth": true });
    columns.push({ "data": column[3], "name": column[3], "autoWidth": true });
    columns.push({ "data": column[4], "name": column[4], "autoWidth": true });

    // GetImgProfile
    columns.push({
        "render": function (data, type, row, meta) { return `<img src="${row.imgProfile}" height="80" width="80px" />`; }
    });

    // status
    columns.push({
        "render": function (data, type, row, meta) {
            if (row.isActive == true)
                return `<label id="${row.id}" style="color:green;font-size: 17px;">مفعل</label>`;
            else
                return `<label id="${row.id}" style="color:red;font-size: 17px;">غير مفعل</label>`;
        }
    });

    // change status
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="تغير الحالة" onclick="Validation('${row.id}')" class="btn btn-primary btn-rounded" />`; }
    });

    // delete user
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="حذف" onclick="DeleteUser('${row.id}')" class="btn btn-danger btn-rounded" />`; }
    });


   

    
    






    $("#datatable-responsive_clients").DataTable({
        "dom": 'Bflrtip',
        "buttons": ['copy', 'csv', 'excel', 'pdf', 'print'],
        //"lengthMenu": [
        //    [100, 250, 500, -1],
        //    [100, 250, 500, 'All']
        //],
        "lengthMenu": [10, 50, 100, 200],


        "language": {
            "sProcessing": "جارٍ التحميل...",
            "sLengthMenu": "أظهر _MENU_ مدخلات",
            "sZeroRecords": "لم يعثر على أية سجلات",
            "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
            "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
            "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
            "sInfoPostFix": "",
            "sSearch": "ابحث:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "الأول",
                "sPrevious": "السابق",
                "sNext": "التالي",
                "sLast": "الأخير"
            }
        },


        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": true, // for disable multiple column at once
        "ajax": {
            "url": `/${controller}/${action}`,
            "type": "POST",
            "datatype": "json",
        },
        //"ordering": true,
        //"order": [[0, 'asc']],
        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    }],

        "columns": columns

    });



}
