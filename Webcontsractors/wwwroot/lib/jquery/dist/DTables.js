$(document).ready(function () {
    function Reloaddata() {
        var DT = $('.paginated').DataTable({
            lengthMenu: [10, 25, 50, 75, 100, { label: 'الكل', value: -1 }],
            "loadingRecords": "جاري جلب البيانات...",
            language: {
                lengthMenu: 'عدد الصفوف في الجدول _MENU_',
                "info": "اظهار من _START_ الى _END_ لكل _TOTAL_ في الصف",
            },
            "emptyTable": "لا يوجد بيانات متاحه في الجدول",
            order: [0, 'desc'],
            "columnDefs": [
                { "className": "dt-center", "targets": "_all" }
            ],
            "oLanguage": {
                "sSearch": "بحث",
                "className": "text-right"
            },
            "footerCallback": function (row, data, start, end, display) {
                var Proapi = this.api(), data;
                var Trnapi = this.api(), data;
                var Trndetapi = this.api(), data;

                // converting to interger to find total
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };

                // computing column Total of the complete result 
                var Proamount = Proapi
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                var Protax = Proapi
                    .column(3)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                var Trncred = Trnapi
                    .column(1)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                var Trndipt = Trnapi
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                var Trnvat = Trnapi
                    .column(3)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                var Trnbln = Trnapi
                    .column(4)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                var Trndetcred = Trndetapi
                    .column(1)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                var Trndetdipt = Trndetapi
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                var Trndetvat = Trndetapi
                    .column(3)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                // Update footer by showing the total with the reference of the column index 
                $(Proapi.column(0).footer()).html('الاجمالي');
                $(Proapi.column(2).footer()).html(Proamount);
                $(Proapi.column(3).footer()).html(Protax);

                $(Trnapi.column(0).footer()).html("الاجمالي");
                $(Trnapi.column(1).footer()).html(Trncred);
                $(Trnapi.column(2).footer()).html(Trndipt);
                $(Trnapi.column(3).footer()).html(Trnvat);
                $(Trnapi.column(4).footer()).html(Trnbln);

                $(Trndetapi.column(0).footer()).html("الاجمالي");
                $(Trndetapi.column(1).footer()).html(Trndetcred);
                $(Trndetapi.column(2).footer()).html(Trndetdipt);
                $(Trndetapi.column(3).footer()).html(Trndetvat);
            },
        });
    }
    // DataTables initialisation
    var DT = $('.paginated').DataTable({
        lengthMenu: [10, 25, 50, 75, 100, { label: 'الكل', value: -1 }],
        "loadingRecords": "جاري جلب البيانات...",
        language: {
            lengthMenu: 'عدد الصفوف في الجدول _MENU_',
            "info": "اظهار من _START_ الى _END_ لكل _TOTAL_ في الصف",
        },
        "emptyTable": "لا يوجد بيانات متاحه في الجدول",
        order: [0, 'desc'],
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
        "oLanguage": {
            "sSearch": "بحث",
            "className": "text-right"
        },
        "footerCallback": function (row, data, start, end, display) {
            var Proapi = this.api(), data;
            var Trnapi = this.api(), data;
            var Trndetapi = this.api(), data;
            
            // converting to interger to find total
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // computing column Total of the complete result 
            var Proamount = Proapi
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var Protax = Proapi
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var Trncred = Trnapi
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var Trndipt = Trnapi
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trnvat = Trnapi
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trnbln = Trnapi
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trndetcred = Trndetapi
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trndetdipt = Trndetapi
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trndetvat = Trndetapi
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trndetblc = Trndetapi
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var Trndetoblc = Trndetapi
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer by showing the total with the reference of the column index 
            $(Proapi.column(0).footer()).html('الاجمالي');
            $(Proapi.column(2).footer()).html(Proamount);
            $(Proapi.column(3).footer()).html(Protax);

            $(Trnapi.column(0).footer()).html("الاجمالي");
            $(Trnapi.column(1).footer()).html(Trncred);
            $(Trnapi.column(2).footer()).html(Trndipt);
            $(Trnapi.column(3).footer()).html(Trnvat);
            $(Trnapi.column(4).footer()).html(Trnbln);

            $(Trndetapi.column(0).footer()).html("الاجمالي");
            $(Trndetapi.column(1).footer()).html(Trndetcred);
            $(Trndetapi.column(2).footer()).html(Trndetdipt);
            $(Trndetapi.column(3).footer()).html(Trndetvat);
            $(Trndetapi.column(4).footer()).html(Trndetblc);
            $(Trndetapi.column(5).footer()).html(Trndetoblc);
        },
    });
    var DTD = $('.Defultpaginated').DataTable({
        lengthMenu: [10, 25, 50, 75, 100, { label: 'الكل', value: -1 }],
        "loadingRecords": "جاري جلب البيانات...",
        language: {
            lengthMenu: 'عدد الصفوف في الجدول _MENU_',
            "info": "اظهار من _START_ الى _END_ لكل _TOTAL_ في الصف",
        },
        "emptyTable": "لا يوجد بيانات متاحه في الجدول",
        order: [0, 'desc'],
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],

        "oLanguage": {
            "sSearch": "بحث",
            "className": "text-right"
        },
    });
    var Datepaginated = $('.Datepaginated').DataTable({
        lengthMenu: [[10, 25, 50, 75, 100, -1], ['10', '25', '50', '75', '100', 'الكل']],
        "loadingRecords": "جاري جلب البيانات...",
        language: {
            lengthMenu: 'عدد الصفوف في الجدول _MENU_',
            "info": "اظهار من _START_ الى _END_ لكل _TOTAL_ في الصف",
            "emptyTable": "لا يوجد بيانات متاحه في الجدول",
            "sSearch": "بحث"
        },
        order: [[5, 'desc'], [5, 'ASC']],
        "columnDefs": [
            {
                "className": "dt-center",
                "targets": [5], // رقم العمود الخاص بالتاريخ
                "type": "date-custom",
                "orderData": [5], // ترتيب العمود 6 (التاريخ) مع العمود 0 (ID),
                "orderable": true // التأكد من إمكانية الفرز بالضغط
            },
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();

            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // حساب الإجمالي لكل عمود
            var subAmount = api.column(1).data().reduce((a, b) => intVal(a) + intVal(b), 0);
            var totalAmount = api.column(2).data().reduce((a, b) => intVal(a) + intVal(b), 0);
            var totalTax = api.column(3).data().reduce((a, b) => intVal(a) + intVal(b), 0);

            // تحديث الفوتر بالقيم المحسوبة
            $(api.column(0).footer()).html('الاجمالي');
            $(api.column(1).footer()).html(subAmount);
            $(api.column(2).footer()).html(totalAmount);
            $(api.column(3).footer()).html(totalTax);
        }
    });
    var Trntblpaginated = $('.Trntblpaginated').DataTable({
        lengthMenu: [[10, 25, 50, 75, 100, -1], ['10', '25', '50', '75', '100', 'الكل']],
        "loadingRecords": "جاري جلب البيانات...",
        language: {
            lengthMenu: 'عدد الصفوف في الجدول _MENU_',
            "info": "اظهار من _START_ الى _END_ لكل _TOTAL_ في الصف",
            "emptyTable": "لا يوجد بيانات متاحه في الجدول",
            "sSearch": "بحث"
        },
        order: [[6, 'desc'], [6, 'ASC']],
        "columnDefs": [
            {
                "className": "dt-center",
                "targets": [6], // رقم العمود الخاص بالتاريخ
                "type": "date-custom",
                "orderData": [6], // ترتيب العمود 6 (التاريخ) مع العمود 0 (ID),
                "orderable": true // التأكد من إمكانية الفرز بالضغط
            },
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();

            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // حساب الإجمالي لكل عمود
            var totalAmount = api.column(2).data().reduce((a, b) => intVal(a) + intVal(b), 0);
            var totalTax = api.column(3).data().reduce((a, b) => intVal(a) + intVal(b), 0);

            // تحديث الفوتر بالقيم المحسوبة
            $(api.column(0).footer()).html('الاجمالي');
            $(api.column(2).footer()).html(totalAmount);
            $(api.column(3).footer()).html(totalTax);
        }
    });
    $("#Trntbl_wrapper .row").eq(0).append("<div id='AddOn' class='d-md-flex justify-content-between align-items-center dt-layout-end col-md-auto text-center'>" +
                                 "<div class='btn-toolbar' role='toolbar' aria-label='Toolbar with button groups'>" +
                                     "<div class='btn-group me-2' role='group' aria-label='First group'>" +
                                          "<button type='button' id='last100' class='btn btn-primary'>100</button>" +
                                     "</div>" +
                                     "<div class='btn-group me-2' role='group' aria-label='First group'>" +
                                        "<button type='button' id='last50' class='btn btn-primary'>50</button>" +
                                     "</div>" +
                                     "<div class='btn-group me-2' role='group' aria-label='First group'>" +
                                        "<button type='button' id='last10' class='btn btn-primary'>10</button>" +
                                     "</div>" +
                                     "<div class='btn-group me-2' role='group' aria-label='First group'>" +
                                        "<button type='button' id='Alltrn' class='btn btn-primary'>الكل</button>"+
                                     "</div>" +
                                    "</div>" +
        "</div>");

    $("#last50").click(function () {
        window.location.href = '/Transactions/last50';
    });
    $("#last10").click(function () { window.location.href = '/Transactions/last10'; });
    $("#last100").click(function () { window.location.href = '/Transactions/last100'; });
    $("#Alltrn").click(function () { window.location.href = '/Transactions/Alltrn'; });
});
